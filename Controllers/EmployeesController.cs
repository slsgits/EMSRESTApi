using EMSRESTApi.Data;
using EMSRESTApi.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace EMSRESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext appDbContext = appDbContext;

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = appDbContext.Employees.ToList();

            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = appDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {
            appDbContext.Employees.Add(employee);

            appDbContext.SaveChanges();

            return CreatedAtAction(
                                    nameof(GetEmployeeById),
                                    new { id = employee.Id },
                                    employee);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = appDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updatedEmployee.Name;
            employee.Department = updatedEmployee.Department;
            employee.Salary = updatedEmployee.Salary;

            appDbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = appDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            appDbContext.Employees.Remove(employee);

            appDbContext.SaveChanges();

            return NoContent(); // Return 204 No Content
        }

        [HttpPatch("{id:int}")]
        [Consumes("application/json-patch+json")]
        public IActionResult PatchEmployee(int id, 
                                           [FromBody] JsonPatchDocument<Employee> patchDoc)
        {
            var employee = appDbContext.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(employee);

            TryValidateModel(employee);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            appDbContext.SaveChanges();

            return Ok(employee);
        }
    }
}
