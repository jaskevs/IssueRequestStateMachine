using System;

namespace SagaStateMachine
{
    public class IssueLocation
    {
        public IssueLocation(Guid correlationId)
        {
        }

        public IssueLocation()
        {
        }

        public Guid CorrelationId { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
    }
}
