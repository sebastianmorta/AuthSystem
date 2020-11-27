using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            ReceiveFeedbackAsync();
            //Thread.Sleep(10000);
            //SendCloudToDeviceMessageAsync().Wait();
            //Thread.Sleep(2000);
        }

        public async Task SendCloudToDeviceMessageAsync()
        {
            await Task.Delay(4000);
            var commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync(targetDevice, commandMessage);
            commandMessage.Ack = DeliveryAcknowledgement.Full;
        }

        private async void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            System.Diagnostics.Debug.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                System.Diagnostics.Debug.WriteLine("Received feedback: {0}",
                  string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
    }
}
