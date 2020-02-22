using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic.CompilerServices;

namespace Moment32.Models
{
    public class Borrow
    {
        public Borrow()
        {
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [Display(Name = "Användare")]
        public string User { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [Display(Name = "Utlåningsdatum")]
        public DateTime BorrowDate { get; set; }

        [Display(Name = "Vilken skiva ska lånas?")]
        public int CdId { get; set; }
        public Cd Cd { get; set; }
    }
}