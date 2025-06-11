using System.Collections.Generic;

namespace OtdamDarom.Web.Requests
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<SubcategoryResponse> Subcategories { get; set; } = new List<SubcategoryResponse>();
    }
}