using System.ComponentModel.DataAnnotations;

namespace EMSRESTApi.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public required string Department { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }
    }
}
