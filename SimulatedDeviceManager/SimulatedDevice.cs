using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Cloud;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.SimulatedDeviceManager
{
    public class SimulatedDevice
    {
        private DeviceClient s_deviceClient;
        private string s_myDeviceId = "";
        private readonly static string s_iotHubUri = "ContosoTestHub4445.azure-devices.net";
        private static string s_deviceKey = "";

        private IoTDevice iotDevice;
        bool x = false;


        public SimulatedDevice(string _deviceId, string _deviceKey, IoTDevice _iotDevice)
        {
            s_myDeviceId = _deviceId;
            s_deviceKey = _deviceKey;
            iotDevice = _iotDevice;
        }


        public async Task Startsimulating()
        {
            s_deviceClient = DeviceClient.Create(s_iotHubUri,
            new DeviceAuthenticationWithRegistrySymmetricKey(s_myDeviceId, s_deviceKey), TransportType.Mqtt);

            SendCloudToDevice receiver = new SendCloudToDevice(s_myDeviceId);
            using var cts = new CancellationTokenSource();
            var messages = SendDeviceToCloudMessagesAsync(cts.Token, receiver);
            ReceiveC2dAsync();
            while (true) {

                if (!x) continue;

                cts.Cancel();
                await messages;
                break;
            }

          
        }

        public Tuple<double, double> MakeCoffe(double MaxCoffeeWeight, double MaxWaterAmount, double GrainWeightRequiredForOneCoffe, double WaterAmountRequiredForOneCoffe)
        {
            double CurrentWaterAmount = MaxCoffeeWeight - WaterAmountRequiredForOneCoffe;
            double CurrentGrainWeight = MaxCoffeeWeight - GrainWeightRequiredForOneCoffe;

            return Tuple.Create(CurrentWaterAmount, CurrentGrainWeight);
        }

        public bool CanMakeCoffee()
        {
            if (iotDevice.CurrentWaterWeight < iotDevice.WaterNessessaryForCoffee || iotDevice.CurrentCoffeeWeight < iotDevice.CoffeeNessessaryForCoffee)  return false;
            else return true;
        }
        
        public bool CanTurnONOFF()
        {
            if (iotDevice.CurrentWaterWeight < iotDevice.WaterNessessaryForLavage) return false;
            else return true;
        }

        public bool newcoffee(int progress)
        {
            if (progress <= iotDevice.WaterNessessaryForCoffee) return true;
            else return false;
        }
        public Tuple<int, string> TurnONOFF (IoTDevice ioTDevice)
        {
            iotDevice.CurrentWaterWeight -= iotDevice.WaterNessessaryForLavage;
            if (iotDevice.CurrentWaterWeight>0)
            {
                return Tuple.Create(iotDevice.CurrentWaterWeight, "Wszystko w porządku ");
            }
            else
            {
                return Tuple.Create(0, "wymagane uzupełnienie wody");
            }
              
        }


        /// <summary> 
        /// Send message to the Iot hub. This generates the object to be sent to the hub in the message.
        /// </summary>
        public async Task SendDeviceToCloudMessagesAsync(CancellationToken token, SendCloudToDevice receiver)
        {
            Random rand = new Random();

            int Coffeprogress = 0;
            int CurrentCoffeeAmount = iotDevice.CurrentCoffeeWeight;
            int CurrentWaterAmount = iotDevice.CurrentWaterWeight;
            int CoffeeAmountNessessaryForCupOfCoffee = iotDevice.CoffeeNessessaryForCoffee;
            int WaterAmountNessessaryForCupOfCoffee = iotDevice.WaterNessessaryForCoffee;

            while (!token.IsCancellationRequested)
            {
                string infoString;
                string levelValue;


                if (newcoffee(Coffeprogress))
                {
                    levelValue = "normal";
                    infoString = "normal message";
                }
                else
                {
                    levelValue = "critical";
                    infoString = "critical message";
                    x = true;
                }

                Coffeprogress += 20;

                var telemetryDataPoint = new
                {
                    deviceId = s_myDeviceId,
                    CurrentWater = CurrentWaterAmount - Coffeprogress,
                    CurrentCoffe = CurrentCoffeeAmount - CoffeeAmountNessessaryForCupOfCoffee,
                    CoffeeInCup = Coffeprogress,
                    pointInfo = infoString
                };

                var telemetryDataString = JsonConvert.SerializeObject(telemetryDataPoint);

                using var message = new Message(Encoding.UTF8 .GetBytes(telemetryDataString));

                message.Properties.Add("level", levelValue);

                await s_deviceClient.SendEventAsync(message);
                System.Diagnostics.Debug.WriteLine("{0} > Sent message: {1}", DateTime.Now, telemetryDataString);
                receiver.SendCloudToDeviceMessageAsync().Wait(1000);
                await Task.Delay(1000);
            }
        }


        public async void ReceiveC2dAsync()
        {
            //Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await s_deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                System.Diagnostics.Debug.WriteLine("Received message: {0}",
                Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                Console.ResetColor();
                await s_deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
