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

            using var cts = new CancellationTokenSource();
            var messages = SendDeviceToCloudMessagesAsync(cts.Token);

            ReceiveC2dAsync();

            cts.Cancel();
            await messages;
        }

        public Tuple<double, double> MakeCoffe(double MaxCoffeeWeight, double MaxWaterAmount, double GrainWeightRequiredForOneCoffe, double WaterAmountRequiredForOneCoffe)
        {
            double CurrentWaterAmount = MaxCoffeeWeight - WaterAmountRequiredForOneCoffe;
            double CurrentGrainWeight = MaxCoffeeWeight - GrainWeightRequiredForOneCoffe;

            return Tuple.Create(CurrentWaterAmount, CurrentGrainWeight);
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

        public Message CheckStatus()
        { 
            var msg = new
            {
                deviceId = "s",
                temperature = "s",
                humidity = "s",
                pointInfo = "s"
            };
            var telemetryDataString = JsonConvert.SerializeObject(msg);
            using var message = new Message(Encoding.UTF8.GetBytes(telemetryDataString));
            return message;
        }


        /// <summary> 
        /// Send message to the Iot hub. This generates the object to be sent to the hub in the message.
        /// </summary>
        public async Task SendDeviceToCloudMessagesAsync(CancellationToken token)
        {
            Random rand = new Random();

            int CurrentWaterAmount = iotDevice.CurrentWaterWeight;
            int CurrentCoffeeAmount = iotDevice.CurrentCoffeeWeight;
            int CoffeeAmountNessessaryForCupOfCoffee = rand.Next(5, 20);
            int WaterAmountNessessaryForCupOfCoffee = rand.Next(40, 240);



            while (!token.IsCancellationRequested && (iotDevice.CurrentWaterWeight-CurrentCoffeeAmount>=WaterAmountNessessaryForCupOfCoffee))
            {
                string infoString;
                string levelValue;

                CurrentCoffeeAmount -= rand.Next(5, 10);

                if (rand.NextDouble() > 0.7)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        levelValue = "critical";
                        infoString = "This is a critical message.";
                    }
                    else
                    {
                        levelValue = "storage";
                        infoString = "This is a storage message.";
                    }
                }
                else
                {
                    levelValue = "normal";
                    infoString = "This is a normal message.";
                }


                var telemetryDataPoint = new
                {
                    deviceId = s_myDeviceId,
                    PoziomWody = CurrentWaterAmount,
                    IlośćKawy = CurrentCoffeeAmount,
                    pointInfo = infoString
                };

                var telemetryDataString = JsonConvert.SerializeObject(telemetryDataPoint);

                using var message = new Message(Encoding.UTF8.GetBytes(telemetryDataString));

                message.Properties.Add("level", levelValue);

                await s_deviceClient.SendEventAsync(message);

                await Task.Delay(rand.Next(5000));
            }



                double minTemperature = 20;
            double minHumidity = 60;
            

            while (!token.IsCancellationRequested)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                string infoString;
                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        levelValue = "critical";
                        infoString = "This is a critical message.";
                    }
                    else
                    {
                        levelValue = "storage";
                        infoString = "This is a storage message.";
                    }
                }
                else
                {
                    levelValue = "normal";
                    infoString = "This is a normal message.";
                }

                var telemetryDataPoint = new
                {
                    deviceId = s_myDeviceId,
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    pointInfo = infoString
                };
                // serialize the telemetry data and convert it to JSON.
                var telemetryDataString = JsonConvert.SerializeObject(telemetryDataPoint);

                // Encode the serialized object using UTF-32. When it writes this to a file, 
                //   it encodes it as base64. If you read it back in, you have to decode it from base64 
                //   and utf-32 to be able to read it.

                // You can encode this as ASCII, but if you want it to be the body of the message, 
                //  and to be able to search the body, it must be encoded in UTF with base64 encoding.

                // Take the string (telemetryDataString) and turn it into a byte array 
                //   that is encoded as UTF-32.
                using var message = new Message(Encoding.UTF8.GetBytes(telemetryDataString));
                //Add one property to the message.
                message.Properties.Add("level", levelValue);

                // Submit the message to the hub.
                await s_deviceClient.SendEventAsync(message);

                // Print out the message.
                //msg+= ("{0} > Sent message: {1}", DateTime.Now, telemetryDataString);
                //Console.WriteLine("{0} > Sent message: {1}", DateTime.Now, telemetryDataString);

                await Task.Delay(1000);
            }
        }


        /// <summary>
        /// This method was written to enable you to decode one of the messages sent to the hub
        ///   and view the body of the message.
        /// Route the messages to storage (they get written to blob storage). 
        /// Send messages to the hub, by running this program with readthefile set to false.
        /// After some messages have been written to storage, download one of the files 
        /// to somewhere you can find it, put the path in this method, and run the program 
        /// with readthefile set to true. 
        /// This method will read in the output file, then convert the first line to a message body object
        /// and write it back out to a new file that you can open and view.
        /// </summary>
        //private static void ReadOneRowFromFile()
        //{
        //    string filePathAndName = "C:\\Users\\username\\Desktop\\testfiles\\47_utf32.txt";

        //    // Set the output file name. 
        //    // Read in the file to an array of objects. These were encoded in Base64 when they were
        //    //   written.
        //    string outputFilePathAndName = filePathAndName.Replace(".txt", "_new.txt");
        //    string[] fileLines = System.IO.File.ReadAllLines(filePathAndName);

        //    // Parse the first line into a message object. Retrieve the body as a string.
        //    //   This string was encoded as Base64 when it was written.
        //    var messageObject = JObject.Parse(fileLines[0]);
        //    var body = messageObject.Value<string>("Body");

        //    // Convert the body from Base64, then from UTF-32 to text, and write it out to the new file
        //    //   so you can view the result.
        //    string outputResult = System.Text.Encoding.UTF32.GetString(System.Convert.FromBase64String(body));

        //    System.IO.File.WriteAllText(outputFilePathAndName, outputResult);
        //}

        public async void ReceiveC2dAsync()
        {
            //Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await s_deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine("Received message: {0}",
                //Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                //Console.ResetColor();

                await s_deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
