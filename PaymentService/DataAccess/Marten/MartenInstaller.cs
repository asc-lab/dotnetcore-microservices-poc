using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Domain;

namespace PaymentService.DataAccess.Marten
{
    public static class MartenInstaller
    {
        public static void AddMarten(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(CreateDocumentStore(cnnString));
            services.AddScoped<Domain.IUnitOfWork, MartenUnitOfWork>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(cn);
                _.DatabaseSchemaName = "payment_service";
                _.Serializer(CustomizeJsonSerializer());

                _.Schema.For<PolicyAccount>().Duplicate(t => t.PolicyAccountNumber, pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
            });
        }

        private static JsonNetSerializer CustomizeJsonSerializer()
        {
            var serializer = new JsonNetSerializer();

            serializer.Customize(_ =>
            {
                _.ContractResolver = new ProtectedSettersContractResolver();
            });

            return serializer;
        }
    }
}
