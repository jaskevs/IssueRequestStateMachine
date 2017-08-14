using Automatonymous;
using MassTransit;

namespace SagaStateMachine
{
    public class IssueRequestSagaStateMachine: MassTransitStateMachine<IssueRequest>
    {
        public IssueRequestSagaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            State(() => IssueType);
            State(() => IssueLocation);
            State(() => IssueDetails);
            State(() => RequestSubmit);
            State(() => RequestAcknowledgement);


            Event(() => NewIssueTypeSubmitted,
                cfg => cfg.CorrelateById(instance => instance.UserId, dtocontext => dtocontext.Message.UserId)
                .SelectId(dtocontext =>
                {
                    return NewId.NextGuid();
                }));

            Event(() => IssueTypeAccepted,
                cfg => cfg.CorrelateById(dtocontext => dtocontext.Message.CorrelationId));

            Event(() => IssueLocationAccepted,
                cfg => cfg.CorrelateById(dtocontext => dtocontext.Message.CorrelationId));

            Event(() => RequestSubmittedToBackend,
                cfg => cfg.CorrelateById(dtocontext => dtocontext.Message.CorrelationId));

            Event(() => BackendAcknowledged,
                cfg => cfg.CorrelateById(dtocontext => dtocontext.Message.CorrelationId));

            Initially(
                When(NewIssueTypeSubmitted)
                .Then(context =>
                {
                    context.Instance.IssueTypeName = context.Data.IssueTypeName;
                    context.Instance.IssueTypeText = context.Data.IssueTypeText;
                })
                .TransitionTo(IssueType));

            During(IssueType,
                When(IssueTypeAccepted)
                .Then(context => {
                    //This is creating new Issue Location every time when come back to the state, this issue location 
                    //expected to be updated everytime coming back to this state
                context.Instance.IssueLocation = new IssueLocation(context.Instance.CorrelationId)
                    {
                        State = context.Data.IssueLocation.State,
                        PostCode = context.Data.IssueLocation.PostCode
                    };

                }).TransitionTo(IssueLocation));


            During(IssueLocation,
                When(IssueLocationAccepted)
                .Then(context => {
                    context.Instance.IssueDetailsText = context.Data.IssueDetailsText;
                }).TransitionTo(IssueDetails));


            During(IssueDetails,
                When(RequestSubmittedToBackend)
                .Then(context =>
                {
                    context.Instance.ApplicationNumber = context.Data.ApplicationNumber;
                    context.Instance.ApplicationDate = context.Data.ApplicationDate;
                }).TransitionTo(RequestSubmit));


            During(RequestSubmit,
                When(BackendAcknowledged)
                .Then(context =>
                {
                    context.Instance.AckNumber = context.Data.AckNumber;

                }).TransitionTo(RequestAcknowledgement));

        }


        public State IssueType { get; set; }
        public State IssueLocation { get; set; }
        public State IssueDetails { get; set; }
        public State RequestSubmit { get; set; }
        public State RequestAcknowledgement { get; set; }
        

        public Event<IssueTypeDTO> NewIssueTypeSubmitted { get; set; }
        public Event<IssueLocationDTO> IssueTypeAccepted { get; set; }
        public Event<IssueDetailsDTO> IssueLocationAccepted { get; set; }
        public Event<RequestSubmitDTO> RequestSubmittedToBackend { get; set; }
        public Event<RequestAckDTO> BackendAcknowledged { get; set; }
        public Event Back { get; set; }
        public Event Cancel { get; set; }
    }
}
