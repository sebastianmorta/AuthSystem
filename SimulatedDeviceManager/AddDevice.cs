using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.SimulatedDeviceManager
{
    public class AddDevice
    {
        
        static string connectionString = "HostName=ContosoTestHub4445.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=bg+kXtRLOxl0QiyIDz2z2GxAvCdW8SGO5dAPNUGGq1c=";
        static string deviceId = "threedevice";
        static RegistryManager registryManager;
        private static string deviceConnectionString;
        
        public AddDevice(string _deviceId)
        {
            deviceId = _deviceId;
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
        }
        static async Task AddDeviceAsync()
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            deviceConnectionString = device.Authentication.SymmetricKey.PrimaryKey;
            //Console.WriteLine("Generated device key: {0}", );
        }

        public string GetConnectionSrting()
        {

            return deviceConnectionString;
        }
    }
}
