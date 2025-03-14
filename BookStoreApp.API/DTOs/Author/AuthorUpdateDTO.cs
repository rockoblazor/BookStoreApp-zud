﻿using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.Author
{
    public class AuthorUpdateDTO : BaseDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(250)]
        public string Bio { get; set; }
    
    }
}
