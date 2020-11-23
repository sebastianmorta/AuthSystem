using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Cloud
{
    public class SendCloudToDevice
    {
        ServiceClient serviceClient;
        string connectionString = "HostName=ContosoTestHub4445.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=QrW58WyWdF5TjF1B+imFWfn5/sot/i+tGAo8JssHzC8=";
        string targetDevice = "Contoso-Test-Device";

        public SendCloudToDevice(string deviceId)
        {
            System.Diagnostics.Debug.WriteLine("Send Cloud-to-Device message\n");
            targetDevice = deviceId;
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            SendCloudToDeviceMessageAsync().Wait();

        }

        public async Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new
             
            Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync(targetDevice, commandMessage);
            await Task.Delay(1000);
        }
    }
}
