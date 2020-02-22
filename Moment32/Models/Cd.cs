using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moment32.Models
{
    public class Cd
    {
        public Cd()
        {
        }

        public int Id { get; set; }

        [Display(Name = "Artist")]
        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        [Display(Name = "Titel på albumet")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Genre { get; set; }

        [Display(Name = "Utgivningsår")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1000, 2100, ErrorMessage = "Vänligen ange ett korrekt årtal")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int ReleaseYear { get; set; }

        [Display(Name = "Antal låtar")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1, 250, ErrorMessage = "Minst 1 och max 250 låtar")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int NoOfSongs { get; set; }

        public ICollection<Borrow> Borrows { get; set; }
      

    }
}