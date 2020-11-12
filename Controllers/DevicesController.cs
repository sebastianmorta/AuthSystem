using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Migrations;
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
        private static List<AddDevice>addDevices = new List<AddDevice>();

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

                var xdd = _context.Users.FirstOrDefault(x => x.Id == userId);
                IoTdevice.UserId = userId;
                if (IoTdevice.DeviceId == 0) {
                    //_context.Add(IoTdevice);
                    //deviceName = devicenumber++ + "device" + userId;
                    //AddDevice addDevice = new AddDevice(deviceName);
                    ////IoTdevice.deviceAdd = addDevice;
                    //IoTdevice.ConnectionString = addDevice.GetConnectionSrting();
                    //await _context.SaveChangesAsync();
                    _context.Add(IoTdevice);
                    deviceName = IoTdevice.ModelName.Replace(" ", "") + userId;
                    AddDevice addDevice = new AddDevice(deviceName);
                    addDevices.Add(addDevice);
                    //IoTdevice.deviceAdd = addDevice;
                    IoTdevice.ConnectionString = addDevice.GetConnectionSrting();
                }
                else
                    _context.Update(IoTdevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(IoTdevice);
        }


        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var IoTdevice = await _context.IoTDevices.FindAsync(id);
   
            AddDevice addDevice = addDevices.Find(x => x.id == (IoTdevice.ModelName.Replace(" ", "") + IoTdevice.UserId));

            //IoTdevice.deviceAdd.DeleteDevice(IoTdevice.ConnectionString);
            await addDevice.DeleteDevice(IoTdevice.ModelName.Replace(" ", "") + IoTdevice.UserId);
            _context.IoTDevices.Remove(IoTdevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
