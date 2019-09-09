﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApiProject.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 10, ErrorMessage = "HeadLine must be between 10 and 200 characters")]
        public string HeadLine { get; set; }

        [StringLength(2000, MinimumLength = 50, ErrorMessage = "ReviewText must be between 50 and 2000 characters")]
        public string ReviewText { get; set; }

        [Required]
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public int Rating { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Book Book { get; set; }
    }
}