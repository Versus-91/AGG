using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AFIAT.TST.EntityFrameworkCore
{
    public static class TSTDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<TSTDbContext> builder, string connectionString)
        {
            builder.UseMySQL(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<TSTDbContext> builder, DbConnection connection)
        {
            builder.UseMySQL(connection);
        }
    }
}