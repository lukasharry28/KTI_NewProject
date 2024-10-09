using System;
using SecureWeb.Models;

namespace SecureWeb.Data;

public interface IUser
{
    User Registratiion(User user);
    User Login(User user);
}
