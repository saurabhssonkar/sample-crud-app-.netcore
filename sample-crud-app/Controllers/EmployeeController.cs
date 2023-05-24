
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_crud_app.Models;

namespace sample_crud_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _employeeContext;
        public EmployeeController(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeContext.Employees.ToListAsync();
                if (employees == null || employees.Count == 0)
                {
                    return NotFound();
                }
                return employees;
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return employee;
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            try
            {
                _employeeContext.Employees.Add(employee);
                await _employeeContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.ID }, employee);
            }
            catch(Exception ex)
            {
                  return BadRequest(ex.Message);
            }
          /*  {
                return StatusCode(500, "Internal server error");
            }*/
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.ID)
                {
                    return BadRequest();
                }
                _employeeContext.Entry(employee).State = EntityState.Modified;
                await _employeeContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Concurrency error");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                _employeeContext.Employees.Remove(employee);
                await _employeeContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


