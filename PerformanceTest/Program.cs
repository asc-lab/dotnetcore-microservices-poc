using System;
using NBomber.CSharp;

namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running performance test");

            NBomberRunner
                .RegisterScenarios
                (
                    new PricingTravelTest().CreateScenario(), 
                    new PricingHouseTest().CreateScenario(),
                    new PricingFarmTest().CreateScenario(),
                    new PolicyTravelTest().CreateScenario()
                )
                .Run();
            
        }
    }
}