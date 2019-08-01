dotnet build ../product-service/ProductService.Api
dotnet build ../pricing-service/PricingService.Api
dotnet build ../policy-service/PolicyService.Api
dotnet build ../payment-service/PaymentService.Api
dotnet build ../policy-search-service/PolicySearchService.Api
dotnet build ../chat-service/ChatService.Api

dotnet build ../agent-portal-gateway/AgentPortalApiGateway
dotnet build ../auth-service/AuthService
dotnet build ../product-service/ProductService
dotnet build ../pricing-service/PricingService
dotnet build ../policy-service/PolicyService
dotnet build ../payment-service/PaymentService
dotnet build ../policy-search-service/PolicySearchService
dotnet build ../chat-service/ChatService

dotnet build ../product-service/ProductService.Test
dotnet build ../pricing-service/PricingService.Test
dotnet build ../policy-service/PolicyService.Test
dotnet build ../payment-service/PaymentService.Test

dotnet test ../product-service/ProductService.Test
dotnet test ../pricing-service/PricingService.Test
dotnet test ../policy-service/PolicyService.Test
dotnet test ../payment-service/PaymentService.Test