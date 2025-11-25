using Abp.Authorization;
using Logistics.Logs.Authorization.Roles;
using Logistics.Logs.Authorization.Users;

namespace Logistics.Logs.Authorization;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
