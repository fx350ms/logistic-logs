using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Logistics.Logs.EntityFrameworkCore;

public static class LogsDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<LogsDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<LogsDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
