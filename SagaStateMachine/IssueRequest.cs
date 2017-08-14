using Automatonymous;
using System;
using System.ComponentModel.DataAnnotations;

namespace SagaStateMachine
{
    public class IssueRequest : SagaStateMachineInstance
    {
        public IssueRequest(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public IssueRequest()
        {

        }

        public static IssueRequest Instance()
        {
            return new IssueRequest();
        }

        public Guid UserId { get; set; }

        public Guid CorrelationId { get; set; }

        [Required]
        public string CurrentState { get; set; }

        [Required]
        public string IssueTypeName { get; set; }

        [Required]
        public string IssueTypeText { get; set; }

        public IssueLocation IssueLocation { get; set; }

        public string IssueDetailsText { get; set; }

        public string Email { get; set; }

        public string ApplicationNumber { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string AckNumber { get; set; }
    }
}
