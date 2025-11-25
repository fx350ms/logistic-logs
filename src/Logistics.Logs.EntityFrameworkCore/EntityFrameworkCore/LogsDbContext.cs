using Abp.Zero.EntityFrameworkCore;
using Logistics.Logs.Authorization.Roles;
using Logistics.Logs.Authorization.Users;
using Logistics.Logs.Entities;
using Logistics.Logs.MultiTenancy;
using Microsoft.EntityFrameworkCore;


namespace Logistics.Logs.EntityFrameworkCore;

public class LogsDbContext : AbpZeroDbContext<Tenant, Role, User, LogsDbContext>
{
    /* Define a DbSet for each entity of the application */

    public DbSet<EntityAuditLog> EntityAuditLogs { get; set; }

    public LogsDbContext(DbContextOptions<LogsDbContext> options)
        : base(options)
    {
    }
}
