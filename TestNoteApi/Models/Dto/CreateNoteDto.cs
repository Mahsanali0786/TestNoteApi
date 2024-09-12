using System.ComponentModel.DataAnnotations;

namespace SampleProject.Models.Dto
{
    public class CreateNoteDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters.")]
        public required string Content { get; set; }
    }
}
