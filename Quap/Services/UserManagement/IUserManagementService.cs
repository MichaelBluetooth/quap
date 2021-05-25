using Quap.Models;

namespace Quap.Services.UserManagement
{
    public interface IUserManagementService
    {
        User register(RegisterRequest req);
        bool userExists(RegisterRequest req);
    }
}