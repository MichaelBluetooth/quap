using System.Threading.Tasks;

namespace Quap.Services.UserManagement
{
    public interface IAuthService
    {
        Task<bool> login(AuthRequest authRequest);
        Task logout();
    }
}