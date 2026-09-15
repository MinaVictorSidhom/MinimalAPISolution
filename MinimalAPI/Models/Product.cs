using Microsoft.AspNetCore.Hosting.Server;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Models
{
    public class Product
    {
        [Required(ErrorMessage="Id can't be blank")]
        [Range(0,int.MaxValue,ErrorMessage="Id should be between 0 and maximum int value")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Product Name can't be blank")]
        public string ProductName { get; set; }

        public override string ToString()
        {
            return $"Product Id:{Id}, Product Name {ProductName}";
        }
    }
}
