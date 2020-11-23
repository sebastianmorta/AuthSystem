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
        public int WaterHopperCapacity { get; set; }
        public int CoffeeHopperCapacity { get; set; }
        public int CurrentWaterWeight { get; set; }
        public int CurrentCoffeeWeight { get; set;}
        public int WaterSlopCapacity { get; set; }
        public int CoffeeSlopCapacity { get; set; }
        public int WaterNessessaryForLavage { get; set; }
        public int WaterNessessaryForCoffee { get; set; }
        public int CoffeeNessessaryForCoffee { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public bool Status{ get; set; }
        public string ConnectionString { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

   
        
    }
}
