using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.SimulatedDeviceManager;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models
{
    public class IoTDevice
    {
        [Key]
        public int DeviceId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [Required]
        public string ModelName { get; set; }
        public int MaxWaterAmount { get; set; }
        public int MaxCoffeeWeight { get; set; }
        public double CurrentWaterAmount { get; set; }
        public double CurrentGrainAmount { get; set;}
        [Column(TypeName = "nvarchar(100)")]
        public string Status{ get; set; }
        public string ConnectionString { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


    }
}
