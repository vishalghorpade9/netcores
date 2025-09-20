using System.ComponentModel.DataAnnotations;

namespace mvc_apps.Models
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
