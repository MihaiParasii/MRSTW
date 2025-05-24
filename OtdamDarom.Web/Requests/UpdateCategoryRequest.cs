using System.ComponentModel.DataAnnotations;

namespace OtdamDarom.Web.Requests
{
    public class UpdateCategoryRequest
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
    }
}