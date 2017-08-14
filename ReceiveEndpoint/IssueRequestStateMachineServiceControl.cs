using Automatonymous;
using IssueRequestData;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration.Saga;
using MassTransit.RabbitMqTransport;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine;
using System;
using Topshelf;

namespace ReceiveEndpoint
{
    public class IssueRequestStateMachineServiceControl: ServiceControl
    {
        private IBusControl bus;
        private readonly string connectionString;

        public IssueRequestStateMachineServiceControl(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool Start(HostControl hostControl)
        {
            bus = ConfigureBus();
            bus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            bus?.Stop(TimeSpan.FromSeconds(30));

            return true;
        }

        private IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                IRabbitMqHost host = cfg.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("issue_request_state", endpoint =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<IssueRequestDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);


                    var repository = new EntityFrameworkSagaRepository<IssueRequest>(() => new IssueRequestDbContext(optionsBuilder.Options));
                   
                    //var repository = new InMemorySagaRepository<CustomerRequest>();

                    var machine = new IssueRequestSagaStateMachine();

                    endpoint.StateMachineSaga(machine, repository);
                });
            });
        }
    }
}
