using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Data;
using EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfficientIoTDataAcquisitionAndProcessingBasedOnCloudServices.Controllers
{
    public class DevicesController : Controller 
    {
        private readonly DeviceDbContext _context;

        public DevicesController(DeviceDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Devices.ToListAsync());
        }


        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Device());
            else
                return View(_context.Devices.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("DeviceId,ModelName,MaxWaterAmount,MaxCoffeeWeight,Status")] Device device)
        {
            if (ModelState.IsValid)
            {
                if (device.DeviceId == 0)
                    _context.Add(device);
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
