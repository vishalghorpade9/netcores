using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc_apps.Data;
using mvc_apps.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mvc_apps.Controllers
{
    public class PartialViewController : Controller
    {
        private readonly DatabaseContext databaseContext;

        public PartialViewController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetMachineStatusPartialView(Machines machines)
        {
            return PartialView("GetMachineStatusPV");
        }

        public IActionResult MachineDashboard()
        {
            var machineList= databaseContext.Machines.ToList();
            return View(machineList);
        }

        public IActionResult MachineData()
        {
            List<Machines> machineList = null;
            using (var transaction = databaseContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                machineList = databaseContext.Machines.ToList();
            }
                
            return Json(machineList);
        }
    }
}
