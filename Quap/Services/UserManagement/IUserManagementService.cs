using System;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Services.UserManagement
{
    public interface IUserManagementService
    {
        User register(RegisterRequest req);
        bool userExists(RegisterRequest req);
        UserDetails getUserDetails(Guid id);
        UserDetails getUserDetails(string username);
    }
}