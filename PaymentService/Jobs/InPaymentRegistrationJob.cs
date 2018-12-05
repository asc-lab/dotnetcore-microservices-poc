using System;
using Microsoft.Extensions.Logging;
using PaymentService.Domain;

namespace PaymentService.Jobs
{
    public class InPaymentRegistrationJob
    {
        private readonly IUnitOfWork uow;
        private readonly BackgroundJobsConfig jobConfig;

        public InPaymentRegistrationJob(IUnitOfWork uow, BackgroundJobsConfig jobConfig)
        {
            this.uow = uow;
            this.jobConfig = jobConfig;
        }

        public void Run()
        {
            Console.WriteLine($"InPayment import started. Looking for file in {jobConfig.InPaymentFileFolder}");

            var importService = new InPaymentRegistrationService(uow);
            importService.RegisterInPayments(jobConfig.InPaymentFileFolder, DateTimeOffset.Now);
            
            Console.WriteLine("InPayment import finished.");

        }
    }
}