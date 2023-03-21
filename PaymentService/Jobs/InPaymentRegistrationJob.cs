using System;
using System.Threading.Tasks;
using PaymentService.Domain;

namespace PaymentService.Jobs;

public class InPaymentRegistrationJob
{
    private readonly IDataStore dataStore;
    private readonly BackgroundJobsConfig jobConfig;

    public InPaymentRegistrationJob(IDataStore dataStore, BackgroundJobsConfig jobConfig)
    {
        this.dataStore = dataStore;
        this.jobConfig = jobConfig;
    }

    public async Task Run()
    {
        Console.WriteLine($"InPayment import started. Looking for file in {jobConfig.InPaymentFileFolder}");

        var importService = new InPaymentRegistrationService(dataStore);
        await importService.RegisterInPayments(jobConfig.InPaymentFileFolder, DateTimeOffset.Now);

        Console.WriteLine("InPayment import finished.");
    }
}