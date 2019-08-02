FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY policy-service/PolicyService.Api/*.csproj ./policy-service/PolicyService.Api/
COPY payment-service/PaymentService.Api/*.csproj ./payment-service/PaymentService.Api/
COPY payment-service/PaymentService/*.csproj ./payment-service/PaymentService/
RUN  cd payment-service/PaymentService && dotnet restore

COPY policy-service/PolicyService.Api ./policy-service/PolicyService.Api/
COPY payment-service/PaymentService.Api ./payment-service/PaymentService.Api/
COPY payment-service/PaymentService ./payment-service/PaymentService/
RUN cd payment-service/PaymentService && dotnet build

COPY payment-service/PaymentService.Test/*.csproj ./payment-service/PaymentService.Test/
RUN cd payment-service/PaymentService.Test && dotnet restore

COPY payment-service/PaymentService.Test ./payment-service/PaymentService.Test/
RUN cd payment-service/PaymentService.Test && dotnet build

FROM build AS test
WORKDIR /src/payment-service/PaymentService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/payment-service/PaymentService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5070
COPY --from=publish /src/payment-service/PaymentService/out ./
ENTRYPOINT ["dotnet", "PaymentService.dll"]