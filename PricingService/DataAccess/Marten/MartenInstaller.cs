using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.DataAccess.Marten
{
    public static class MartenInstaller
    {
        public static void AddMarten(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IDocumentStore>(CreateDocumentStore(cnnString));

            //services.AddScoped<IDocumentSession>(sp => {
            //    return sp.GetService<IDocumentStore>().OpenSession();
            //});
            services.AddScoped<Domain.IDataStore, MartenDataStore>();
        }

        private static IDocumentStore CreateDocumentStore(string cn)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(cn);
                _.DatabaseSchemaName = "policy_service";
                _.Serializer(CustomizeJsonSerializer());

                _.Schema.For<Tariff>().Duplicate(t => t.Code,pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
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
