using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Areas.Identity.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data;
//using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Migrations;
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

            var devices = await _context.Devices.Where(x => x.UserId == userId).ToListAsync();

            return View(devices);
        }


        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new IoTDevice());
            else
                return View(_context.Devices.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(IoTDevice device)
        {

            if (ModelState.IsValid)
            {
                string deviceName;
                var userId = _httpContextAccessor.HttpContext.User?.Claims?
                 .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                var xdd = _context.Users.FirstOrDefault(x => x.Id == userId);
                device.UserId = userId;
                if (device.DeviceId == 0) { 
                    _context.Add(device);
                    var deviceid = device.DeviceId;
                    deviceName = deviceid + "device" + userId;
                    AddDevice addDevice = new AddDevice(deviceName);
                    device.ConnectionString = addDevice.GetConnectionSrting();
                }
                else
                    _context.Update(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
