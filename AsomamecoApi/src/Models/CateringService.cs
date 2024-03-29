﻿using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class CateringService
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
