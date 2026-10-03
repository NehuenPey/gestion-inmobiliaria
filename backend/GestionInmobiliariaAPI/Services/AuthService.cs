using GestionInmobiliariaAPI.Data;
using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionInmobiliariaAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarUsuario(RegistroDto dto)
        {
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
            {
                return false;
            }

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = "Usuario",
                Activo = true
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ValidarCredenciales(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
            {
                return false;
            }

            if (!usuario.Activo)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(
                dto.Password,
                usuario.PasswordHash
            );
        }
    }
}