using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using Microsoft.AspNetCore.Identity;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Nick { get; set; }

        public ICollection<Device> Devices { get; set; }

        //[PersonalData]
        //[Column(TypeName = "nvarchar(100)")]
        //public string User{ get; set; }
    }
}
