using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;

namespace ReceiveEndpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = @"Server=;Database=IssueDb;User ID=sa;Password=123password";
            HostFactory.Run(c =>
            {
                //c.UseMicrosoftDependencyInjection(Provider)
                c.Service(() => new IssueRequestStateMachineServiceControl(connectionString))
                .EnableServiceRecovery(r =>
                {
                    r.RestartComputer(5, "Server Restarted");
                    r.RestartService(0);
                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                });
            });
        }
    }
}
