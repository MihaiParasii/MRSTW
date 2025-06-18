using System;
using System.ComponentModel.DataAnnotations;
using System.Web; 

namespace OtdamDarom.BusinessLogic.Dtos
{
    public class DealDto
    {
        [Required]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Numele anunțului este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele nu poate depășască 100 de caractere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descrierea anunțului este obligatorie.")]
        [StringLength(1000, ErrorMessage = "Descrierea nu poate depășască 1000 de caractere.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImageURL { get; set; } 

        [Display(Name = "Imagine Anunț")]
        public HttpPostedFileBase ImageFile { get; set; } 

        [Display(Name = "Șterge imaginea existentă")]
        public bool DeleteExistingImage { get; set; }

        [Required(ErrorMessage = "Selectați o categorie.")]
        [Display(Name = "Categorie")]
        public int? SelectedCategoryId { get; set; }

        [Required(ErrorMessage = "Selectați o subcategorie.")]
        [Display(Name = "Subcategorie")]
        public int SelectedSubcategoryId { get; set; } 
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}