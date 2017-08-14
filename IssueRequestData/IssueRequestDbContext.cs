using MassTransit.EntityFrameworkCoreIntegration;
using SagaStateMachine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IssueRequestData
{
    //dotnet ef migrations add IssueRequestMigrationV1.1 -c IssueRequestDbContext -o Migrations -p IssueRequestData.csproj -s ..\ReceiveEndpoint\ReceiveEndpoint
    //dotnet ef database update -p IssueRequestData.csproj -s ..\ReceiveEndpoint\ReceiveEndpoint
    public class IssueRequestDbContext : SagaDbContext<IssueRequest, IssueRequestMapping>
    {
        public IssueRequestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IssueRequest>().ToTable("IssueRequest", "test");

            modelBuilder.Entity<IssueLocation>().ToTable("IssueLocation", "test").HasKey(e => e.CorrelationId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<IssueRequest> IssueRequest { get; set; }
        public DbSet<IssueLocation> IssueLocation { get; set; }
    }

    public class IssueRequestDbContextFactory : IDbContextFactory<IssueRequestDbContext>
    {
        public IssueRequestDbContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IssueRequestDbContext>();
            optionsBuilder.UseSqlServer("Server=192.168.90.98\\sql2016;Database=TRIBE;User ID=sa;Password=redland1;");

            return new IssueRequestDbContext(optionsBuilder.Options);
        }
    }
}
