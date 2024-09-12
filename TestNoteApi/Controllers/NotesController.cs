namespace SampleProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SampleProject.DbContext;
    using SampleProject.Models;
    using SampleProject.Models.Dto;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly SampleDbContext _context;

        public NotesController(SampleDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = 1
            };

            try
            {
                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetNotesByUserId), new { userId = note.UserId }, note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create note: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            var notes = await _context.Notes.ToListAsync();
            if (notes == null || !notes.Any()) return NotFound("No notes found");

            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound("Note not found");

            return Ok(note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] CreateNoteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound("Note not found");

            note.Title = dto.Title;
            note.Content = dto.Content;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update note: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound("Note not found");

            try
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
                return Ok("Note deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete note: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetNotesByUserId(int userId)
        {
            var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
            if (notes == null || !notes.Any()) return NotFound("No notes found for the user");

            return Ok(notes);
        }
    }
}
