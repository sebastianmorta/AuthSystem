using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data;

using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.SimulatedDeviceManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Controllers
{
    public class DevicesController : Controller
    {
        private readonly DeviceDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private static List<AddDevice>addDevices = new List<AddDevice>();

        public DevicesController(DeviceDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User?.Claims?
                 .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var devices = await _context.IoTDevices.Where(x => x.UserId == userId).ToListAsync();

            return View(devices);
        }


        // GET: Device/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new IoTDevice());
            else
                return View(_context.IoTDevices.Find(id));
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(IoTDevice IoTdevice)
        {

            if (ModelState.IsValid)
            {
                string deviceName;
                var userId = _httpContextAccessor.HttpContext.User?.Claims?
                 .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                var Uid = _context.Users.FirstOrDefault(x => x.Id == userId);
                IoTdevice.UserId = userId;
                if (IoTdevice.DeviceId == 0) {
                    IoTdevice.CurrentCoffeeWeight = IoTdevice.CoffeeHopperCapacity;
                    IoTdevice.CurrentWaterWeight = IoTdevice.WaterHopperCapacity;
                    _context.Add(IoTdevice);
                    deviceName = IoTdevice.ModelName.Replace(" ", "") + userId;
                    AddDevice addDevice = new AddDevice(deviceName, true);
                    //addDevices.Add(addDevice);
                    IoTdevice.ConnectionString = addDevice.GetConnectionSrting();
                }
                else
                    _context.Update(IoTdevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(IoTdevice);
        }

        //public async Task<IActionResult> ActionsDevice(int? id)
        public IActionResult ActionsDevice(int? id)
        {
            return View(_context.IoTDevices.Find(id));
        }

        public async Task<IActionResult> TurnONOFF(int? id)
        {
            var iotDevice = await _context.IoTDevices.FindAsync(id);
            //var device =  _context.IoTDevices.Where(x => x.UserId == id).FirstOrDefault();
            //device.Status = !device.Status;
            iotDevice.Status = !iotDevice.Status;

            if (iotDevice.Status)
            {

            }

            _context.Update(iotDevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task MakeCoffee(int id)
        {
            var iotDevice = await _context.IoTDevices.FindAsync(id);

            //var iotDevice = _context.IoTDevices.Where(x => x.DeviceId == id).FirstOrDefault();
            SimulatedDevice simulatedDevice = new SimulatedDevice(iotDevice.ModelName.Replace(" ", "") + iotDevice.UserId, iotDevice.ConnectionString, iotDevice);
            await simulatedDevice.Startsimulating();


        }


        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var IoTdevice = await _context.IoTDevices.FindAsync(id);
            string deviceName = IoTdevice.ModelName.Replace(" ", "") + IoTdevice.UserId;
            //AddDevice addDevice = addDevices.Find(x => x.id == deviceName); 
            //await addDevice.DeleteDevice(deviceName);
            AddDevice adddevice = new AddDevice(deviceName,false);
            await adddevice.DeleteDevice(deviceName);
            //addDevices.Remove(adddevice);
           
            _context.IoTDevices.Remove(IoTdevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
