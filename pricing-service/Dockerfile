FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY pricing-service/PricingService.Api/*.csproj ./pricing-service/PricingService.Api/
COPY pricing-service/PricingService/*.csproj ./pricing-service/PricingService/
RUN  cd pricing-service/PricingService && dotnet restore

COPY pricing-service/PricingService.Api ./pricing-service/PricingService.Api/
COPY pricing-service/PricingService ./pricing-service/PricingService/
RUN cd pricing-service/PricingService && dotnet build

COPY pricing-service/PricingService.Test/*.csproj ./pricing-service/PricingService.Test/
RUN cd pricing-service/PricingService.Test && dotnet restore

COPY pricing-service/PricingService.Test ./pricing-service/PricingService.Test/
RUN cd pricing-service/PricingService.Test && dotnet build

FROM build AS test
WORKDIR /src/pricing-service/PricingService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/pricing-service/PricingService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
EXPOSE 55269
EXPOSE 44392
COPY --from=publish /src/pricing-service/PricingService/out ./
ENTRYPOINT ["dotnet", "PricingService.dll"]