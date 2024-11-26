using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;
using UserApi.DTOs;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // Crea un nuevo usuario.
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("El cuerpo de la solicitud no puede ser nulo.");

                if (_context.Users.Any(u => u.Email == user.Email))
                    return BadRequest("El correo ya está registrado.");

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Obtiene un usuario por su ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null || user.IsDeleted)
                    return NotFound("El usuario no fue encontrado.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Obtiene una lista paginada de usuarios.
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            try
            {
                if (page <= 0 || limit <= 0)
                    return BadRequest("Los valores de página y límite deben ser mayores a 0.");

                var users = await _context.Users
                    .Where(u => !u.IsDeleted)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Actualiza uno o más campos de un usuario.
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updatedUser)
        {
            try
            {
                if (updatedUser == null)
                    return BadRequest("El cuerpo de la solicitud no puede ser nulo.");

                var user = await _context.Users.FindAsync(id);

                if (user == null || user.IsDeleted)
                    return NotFound("El usuario no fue encontrado.");

                // Actualiza solo los campos permitidos
                if (!string.IsNullOrWhiteSpace(updatedUser.FirstName))
                    user.FirstName = updatedUser.FirstName;

                if (!string.IsNullOrWhiteSpace(updatedUser.LastName))
                    user.LastName = updatedUser.LastName;

                if (!string.IsNullOrWhiteSpace(updatedUser.Email))
                {
                    if (_context.Users.Any(u => u.Email == updatedUser.Email && u.Id != id))
                        return BadRequest("El correo ya está registrado.");
                    user.Email = updatedUser.Email;
                }

                // Password no se procesa aquí porque no está en el DTO

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Elimina lógicamente un usuario.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null || user.IsDeleted)
                    return NotFound("El usuario no fue encontrado.");

                user.IsDeleted = true;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
