using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas
{
    public class SimulateDeviceMethods
    {
        string DeviceConnectionString = "HostName=ContosoTestHub4445.azure-devices.net;DeviceId=delonghi3caa1f7b-a878-401a-8e28-3826b03697a0;SharedAccessKey=mjIK0NifwwWbznHcsIk951UOpqwkJLsGd4QqiXfWqWs=";
        //DeviceClient Client = null; 

        public SimulateDeviceMethods(string connstring ,DeviceClient deviceClient)
        {
            DeviceConnectionString = connstring;
            try
            {
                    System.Diagnostics.Debug.WriteLine("Connecting to hub");
                //deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);

                deviceClient.SetMethodHandlerAsync("LockDoor", MakeNewCoffee, null);
                deviceClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null);

                    System.Diagnostics.Debug.WriteLine("Waiting for direct method call and device twin update\n Press enter to exit.");
                Thread.Sleep(2000);

                System.Diagnostics.Debug.WriteLine("Exiting...");

                deviceClient.SetMethodHandlerAsync("LockDoor", null, null);
                deviceClient.CloseAsync().Wait();
            }
            catch (Exception ex)
            {
                    System.Diagnostics.Debug.WriteLine("Error in sample: {0}", ex.Message);
            }
        }
        public Task<MethodResponse> MakeNewCoffee(MethodRequest methodRequest, object userContext)
        {
                System.Diagnostics.Debug.WriteLine("Locking Door!");
                System.Diagnostics.Debug.WriteLine("\nReturning response for method {0}", methodRequest.Name);

            string result = "'kawa została zrobiona.'";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }
        public async Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            System.Diagnostics.Debug.WriteLine("Desired property change:");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(desiredProperties));
        }
    }
}
