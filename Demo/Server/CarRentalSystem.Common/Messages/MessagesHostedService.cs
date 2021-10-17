namespace CarRentalSystem.Common.Messages
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Models;
    using Hangfire;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Hosted services in ASP.Net Core are long running background services.
    /// </summary>
    public class MessagesHostedService : IHostedService
    {
        // This Hangfire job manager
        private readonly IRecurringJobManager recurringJob;
        private readonly IBus publisher;
        private readonly IServiceProvider services;

        public MessagesHostedService(
            IRecurringJobManager recurringJob, 
            IBus publisher, IServiceProvider services)
        {
            this.recurringJob = recurringJob;
            this.publisher = publisher;
            this.services = services;
        }

        /// <summary>
        /// What to do when hosted service starts
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // We tell hangfire to do this job with Cron based expression on given interval
            this.recurringJob.AddOrUpdate(
                nameof(MessagesHostedService),
                () => this.ProcessPendingMessages(),
                "*/5 * * * * *");

            return Task.CompletedTask;
        }

        /// <summary>
        /// What to do when hosting service stops
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        // What this cron job will do.
        public void ProcessPendingMessages()
        {
            var scope = this.services.CreateScope();
            var data = scope.ServiceProvider.GetService<DbContext>();

            var messages = data
                .Set<Message>()
                .Where(m => !m.Published)
                .OrderBy(m => m.Id)
                .ToList();

            foreach (var message in messages)
            {
                this.publisher.Publish(message.Data, message.Type);

                message.MarkAsPublished();

                data.SaveChanges();
            }
        }
    }
}
