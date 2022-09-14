using Microsoft.EntityFrameworkCore;

namespace caLibProdStat;

public class CaDb: DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=progerx;Database=CryptoAlert;UID=ca;PWD=1qaz!QAZ;TrustServerCertificate=true;");
    }
    public DbSet<Product>? Products { get; set; }
}
