using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas
{
    public class SimulateDeviceMethods
    {
        string DeviceConnectionString = "";
        DeviceClient Client = null;

        public SimulateDeviceMethods()
        {
            try
            {
                    System.Diagnostics.Debug.WriteLine("Connecting to hub");
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);

                Client.SetMethodHandlerAsync("LockDoor", LockDoor, null);
                Client.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null);

                    System.Diagnostics.Debug.WriteLine("Waiting for direct method call and device twin update\n Press enter to exit.");
                    Console.ReadLine();

                    System.Diagnostics.Debug.WriteLine("Exiting...");

                Client.SetMethodHandlerAsync("LockDoor", null, null);
                Client.CloseAsync().Wait();
            }
            catch (Exception ex)
            {
                    System.Diagnostics.Debug.WriteLine("Error in sample: {0}", ex.Message);
            }
        }
        public Task<MethodResponse> LockDoor(MethodRequest methodRequest, object userContext)
        {
                System.Diagnostics.Debug.WriteLine("Locking Door!");
                System.Diagnostics.Debug.WriteLine("\nReturning response for method {0}", methodRequest.Name);

            string result = "'Door was locked.'";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }
        public async Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            System.Diagnostics.Debug.WriteLine("Desired property change:");
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(desiredProperties));
        }
    }
}
