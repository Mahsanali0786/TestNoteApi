using System;
using System.ComponentModel.DataAnnotations;

namespace SampleProject.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public required string Title { get; set; }

        public string? Content { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
    }
}
