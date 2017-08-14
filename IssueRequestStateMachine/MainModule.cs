using MassTransit;
using Nancy;
using Nancy.ModelBinding;
using SagaStateMachine;
using System;

namespace IssueRequestHost
{

    public class CustomerRequestModule : NancyModule
    {
        public CustomerRequestModule(IBus bus)
        {
            Post("/api/issue-request/request-type", _ =>
            {
                IssueTypeDTO arg = this.Bind<MockedIssueTypeDTOImp>();

                bus.Publish(arg);

                return HttpStatusCode.OK;
            });

            Post("/api/issue-request/issue-location", _ =>
            {
                IssueLocationDTO arg = this.Bind<MockedIssueLocationDTOImp>();

                bus.Publish(arg);

                return HttpStatusCode.OK;
            });

            Post("/api/issue-request/issue-details", _ =>
            {
                IssueDetailsDTO arg = this.Bind<MockedIssueDetailsDTOImp>();

                bus.Publish(arg);

                return HttpStatusCode.OK;
            });

            Post("/api/customer-request/request-submit", _ =>
            {
                RequestSubmitDTO arg = this.Bind<MockedRequestSubmitDTOImp>();

                bus.Publish(arg);

                return HttpStatusCode.OK;
            });
        }
    }

    public class MockedIssueTypeDTOImp : IssueTypeDTO
    {
        public MockedIssueTypeDTOImp()
        {
            UserId = new Guid("C69C4C4A-1289-44B1-A3B9-BF5C14C9CE1B");
            IssueTypeName = "Test Issue 1";
            IssueTypeText = "Test Issue 1 Text";
        }
        public Guid UserId { get; set; }

        public string IssueTypeName { get; set; }
        public string IssueTypeText { get; set; }

    }

    internal class MockedIssueLocationDTOImp : IssueLocationDTO
    {
        public MockedIssueLocationDTOImp()
        {
            IssueLocation = new IssueLocation()
            {
                PostCode = "2421",
                State = "NSW"
            };
        }

        public Guid CorrelationId { get; set; }
        public IssueLocation IssueLocation { get; set; }
    }


    internal class MockedIssueDetailsDTOImp : IssueDetailsDTO
    {
        public MockedIssueDetailsDTOImp()
        {
            IssueDetailsText = "xyz";
        }

        public Guid CorrelationId { get; set; }
        public string IssueDetailsText { get; set; }
    }


    internal class MockedRequestSubmitDTOImp : RequestSubmitDTO
    {
        public MockedRequestSubmitDTOImp()
        {
            ApplicationNumber = "ABC123";
            ApplicationDate = DateTime.UtcNow;
        }

        public Guid CorrelationId { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
