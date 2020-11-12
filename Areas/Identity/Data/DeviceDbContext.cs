using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data
{
    public class DeviceDbContext : IdentityDbContext<ApplicationUser>
    {
        public DeviceDbContext(DbContextOptions<DeviceDbContext> options): base(options)
        {
        }
        public virtual DbSet<IoTDevice> IoTDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasMany(x => x.IoTDevices)
                .WithOne(z => z.ApplicationUser);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    var builder = new ConfigurationBuilder();
        //    builder.SetBasePath(Directory.GetCurrentDirectory());
        //    builder.AddJsonFile("appsettings.json");
        //    IConfiguration Configuration = builder.Build();

        //    optionsBuilder.UseSqlServer(
        //        Configuration.GetConnectionString("AuthDbContextConnection"));
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
