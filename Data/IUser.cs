using System;
using SecureWeb.Models;

namespace SecureWeb.Data;

public interface IUser
{
    User Registratiion(User user);
    User Login(User user);
    User GetUserByUsername(string username);
    void UpdatePassword(User user);
}
