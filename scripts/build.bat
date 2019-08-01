call dotnet build ../product-service/ProductService.Api
call dotnet build ../pricing-service/PricingService.Api
call dotnet build ../policy-service/PolicyService.Api
call dotnet build ../payment-service/PaymentService.Api
call dotnet build ../policy-search-service/PolicySearchService.Api
call dotnet build ../chat-service/ChatService.Api

call dotnet build ../agent-portal-gateway/AgentPortalApiGateway
call dotnet build ../auth-service/AuthService
call dotnet build ../product-service/ProductService
call dotnet build ../pricing-service/PricingService
call dotnet build ../policy-service/PolicyService
call dotnet build ../payment-service/PaymentService
call dotnet build ../policy-search-service/PolicySearchService
call dotnet build ../chat-service/ChatService

call dotnet build ../product-service/ProductService.Test
call dotnet build ../pricing-service/PricingService.Test
call dotnet build ../policy-service/PolicyService.Test
call dotnet build ../payment-service/PaymentService.Test

call dotnet test ../product-service/ProductService.Test
call dotnet test ../pricing-service/PricingService.Test
call dotnet test ../policy-service/PolicyService.Test
call dotnet test ../payment-service/PaymentService.Test