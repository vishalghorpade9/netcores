using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opc_ua_worker_service.Models
{
    public class Machines
    {
        public Machines()
        {
        }

        public Machines(int machineId, string machineName, int machineStatus)
        {
            MachineId = machineId;
            MachineName = machineName;
            MachineStatus = machineStatus;
        }

        [Key]
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int MachineStatus { get; set; }
        public string? MachineStatusTag { get; set; }
    }
}
