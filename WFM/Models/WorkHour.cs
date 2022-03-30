using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.Models
{
    public class WorkHour
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalHoursWorked { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
    }
}