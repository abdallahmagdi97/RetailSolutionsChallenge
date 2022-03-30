using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WFM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFM.Data;

namespace WFM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkHoursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkHoursController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkHour>>> Get()
        {
            return await _context.WorkHours.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkHour>> GetWorkHours(int id)
        {
            var WorkHours = await _context.WorkHours.FindAsync(id);

            if (WorkHours == null)
            {
                return NotFound();
            }

            return WorkHours;
        }
        [HttpPost]
        public async Task<ActionResult<WorkHour>> PostWorkHours(WorkHour workHours)
        {
            CalculateWorkHours(workHours);
            _context.WorkHours.Add(workHours);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkHours", new { id = workHours.Id }, workHours);
        }
        public async void CalculateWorkHours(WorkHour workHour)
        {
            var employee = await _context.Employees.FindAsync(workHour.EmployeeId);
            if (employee.Shift == 1)
            {
                 workHour.TotalHoursWorked = (workHour.EndTime.Hour - workHour.StartTime.Hour) - ((workHour.EndTime.Hour - workHour.StartTime.Hour) * (1/6)) - (((workHour.EndTime.Hour - workHour.StartTime.Hour) / 4) * (1/3));
            } else if (employee.Shift == 2)
            {
                 workHour.TotalHoursWorked = (workHour.EndTime.Hour - workHour.StartTime.Hour) - ((workHour.EndTime.Hour - workHour.StartTime.Hour) * (1 / 4)) - (((workHour.EndTime.Hour - workHour.StartTime.Hour) / 4) * (1 / 2));
            } else
            {
                throw new Exception("Employee shift not supported, please choose either 1 for dayshift or 2 for night shift");
            }
        }
    }
}
