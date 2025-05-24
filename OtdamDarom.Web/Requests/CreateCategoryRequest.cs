using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.Web.Requests
{
    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}