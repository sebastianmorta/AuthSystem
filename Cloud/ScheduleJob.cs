using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using System.Threading;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Cloud
{

    public class ScheduleJob
    {
        JobClient jobClient = null;
        string connString = "HostName=ContosoTestHub4445.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=QrW58WyWdF5TjF1B+imFWfn5/sot/i+tGAo8JssHzC8=";
        string deviceId = "";

        public ScheduleJob(string _deviceId)
        {
            deviceId = _deviceId;
                System.Diagnostics.Debug.WriteLine("Press ENTER to start running jobs.");
                Thread.Sleep(2000);

            jobClient = JobClient.CreateFromConnectionString(connString);

            string methodJobId = Guid.NewGuid().ToString();

            StartMethodJob(methodJobId);
            MonitorJob(methodJobId).Wait();
                System.Diagnostics.Debug.WriteLine("Press ENTER to run the next job.");
                Thread.Sleep(2000);

            string twinUpdateJobId = Guid.NewGuid().ToString();

            StartTwinUpdateJob(twinUpdateJobId);
            MonitorJob(twinUpdateJobId).Wait();
                System.Diagnostics.Debug.WriteLine("Press ENTER to exit.");
                Thread.Sleep(2000);
        }
        public async Task MonitorJob(string jobId)
        {
            JobResponse result;
            do
            {
                result = await jobClient.GetJobAsync(jobId);
                    System.Diagnostics.Debug.WriteLine("Job Status : " + result.Status.ToString());
                Thread.Sleep(2000);
            } while ((result.Status != JobStatus.Completed) &&
              (result.Status != JobStatus.Failed));
        }
        public async Task StartMethodJob(string jobId)
        {
            CloudToDeviceMethod directMethod =
              new CloudToDeviceMethod("LockDoor", TimeSpan.FromSeconds(5),
              TimeSpan.FromSeconds(5));

            JobResponse result = await jobClient.ScheduleDeviceMethodAsync(jobId,
                $"DeviceId IN ['{deviceId}']",
                directMethod,
                DateTime.UtcNow,
                (long)TimeSpan.FromMinutes(2).TotalSeconds);

            System.Diagnostics.Debug.WriteLine("Started Method Job");
        }
        public async Task StartTwinUpdateJob(string jobId)
        {
            Twin twin = new Twin(deviceId);
            twin.Tags = new TwinCollection();
            twin.Tags["Building"] = "43";
            twin.Tags["Floor"] = "3";
            twin.ETag = "*";

            twin.Properties.Desired["LocationUpdate"] = DateTime.UtcNow;

            JobResponse createJobResponse = jobClient.ScheduleTwinUpdateAsync(
                jobId,
                $"DeviceId IN ['{deviceId}']",
                twin,
                DateTime.UtcNow,
                (long)TimeSpan.FromMinutes(2).TotalSeconds).Result;

            System.Diagnostics.Debug.WriteLine("Started Twin Update Job");
        }
    }
}
