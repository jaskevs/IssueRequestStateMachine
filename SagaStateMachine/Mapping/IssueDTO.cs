using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SagaStateMachine
{
    public interface IssueTypeDTO
    {
        [Required]
        Guid UserId { get; set; }

        [Required]
        string IssueTypeName { get; set; }

        string IssueTypeText { get; set; }
    }

    public interface IssueLocationDTO
    {
        Guid CorrelationId { get; set; }

        IssueLocation IssueLocation { get; set; }
    }

    public interface IssueDetailsDTO
    {
        Guid CorrelationId { get; set; }

        string IssueDetailsText { get; set; }
    }

    public interface RequestSubmitDTO
    {
        Guid CorrelationId { get; set; }

        [Required]
        string ApplicationNumber { get; set; }
        
        DateTime ApplicationDate { get; set; }
    }

    public interface RequestAckDTO
    {
        Guid CorrelationId { get; set; }

        string AckNumber { get; set; }
    }
}
