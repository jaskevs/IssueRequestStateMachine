using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachine;

namespace IssueRequestData
{
    public class IssueRequestMapping : IEntityTypeConfiguration<IssueRequest>
    {
        public IssueRequestMapping()
        {
        }

        public void Configure(EntityTypeBuilder<IssueRequest> entityTypeBuilder)
        {
            entityTypeBuilder.Property(x => x.CorrelationId);
            entityTypeBuilder.Property(x => x.CurrentState).HasMaxLength(250).IsRequired();

            entityTypeBuilder.HasOne(a => a.IssueLocation).WithOne().HasForeignKey<IssueLocation>(a => a.CorrelationId);
        }
    }
}