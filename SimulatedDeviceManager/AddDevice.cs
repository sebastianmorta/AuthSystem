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
        public  string deviceId = "";
        RegistryManager registryManager;
        private  string deviceConnectionString;
        public string id;
        //private SimulatedDevice simulatedDevice;
        
        public AddDevice(string _deviceId, bool addNew)
        {
            deviceId = _deviceId;
            id = deviceId;
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            if(addNew)
                AddDeviceAsync().Wait();
            System.Diagnostics.Debug.WriteLine("hujwgeje");
        }
        public AddDevice()
        {

        }
        public async Task AddDeviceAsync()
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
        }

        public string GetConnectionSrting()
        {
            return deviceConnectionString;
        }

        public async Task DeleteDevice(string devicesToDelete)
        {
            await registryManager.RemoveDeviceAsync(devicesToDelete);
        }        
    }
}
