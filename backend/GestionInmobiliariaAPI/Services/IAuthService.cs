using GestionInmobiliariaAPI.DTOs;

namespace GestionInmobiliariaAPI.Services
{
    public interface IAuthService
    {
        Task<bool> RegistrarUsuario(RegistroDto dto);
        Task<bool> ValidarCredenciales(LoginDto dto);
    }
}