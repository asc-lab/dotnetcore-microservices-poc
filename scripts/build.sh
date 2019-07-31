dotnet build ../ProductService.Api
dotnet build ../PricingService.Api
dotnet build ../PolicyService.Api
dotnet build ../PaymentService.Api
dotnet build ../PolicySearchService.Api

dotnet build ../AgentPortalApiGateway
dotnet build ../AuthService
dotnet build ../ProductService
dotnet build ../PricingService
dotnet build ../PolicyService
dotnet build ../PaymentService
dotnet build ../PolicySearchService

dotnet build ../ProductService.Test
dotnet build ../PricingService.Test
dotnet build ../PolicyService.Test
dotnet build ../PaymentService.Test

dotnet test ../ProductService.Test
dotnet test ../PricingService.Test
dotnet test ../PolicyService.Test
dotnet test ../PaymentService.Test

