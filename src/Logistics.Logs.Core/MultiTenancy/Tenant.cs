using Abp.MultiTenancy;
using Logistics.Logs.Authorization.Users;

namespace Logistics.Logs.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
