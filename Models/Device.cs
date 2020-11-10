using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [Required]
        public string ModelName { get; set; }
        public int MaxWaterAmount { get; set; }
        public int MaxCoffeeWeight { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Status{ get; set; }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
