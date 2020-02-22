using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moment32.Models
{
    public class Artist
    {
        public Artist()
        {
        }

        public int Id { get; set; }

        [Display(Name = "Namn på artist/band")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Name { get; set; }

        [Display(Name = "Nationalitet")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Nationality { get; set; }

        public ICollection<Cd> Cds { get; set; }


    }
}