call dotnet build ../ProductService.Api
call dotnet build ../PricingService.Api
call dotnet build ../PolicyService.Api
call dotnet build ../PaymentService.Api
call dotnet build ../PolicySearchService.Api

call dotnet build ../AgentPortalApiGateway
call dotnet build ../AuthService
call dotnet build ../ProductService
call dotnet build ../PricingService
call dotnet build ../PolicyService
call dotnet build ../PaymentService
call dotnet build ../PolicySearchService

call dotnet build ../ProductService.Test
call dotnet build ../PricingService.Test
call dotnet build ../PolicyService.Test
call dotnet build ../PaymentService.Test

call dotnet test ../ProductService.Test
call dotnet test ../PricingService.Test
call dotnet test ../PolicyService.Test
call dotnet test ../PaymentService.Test