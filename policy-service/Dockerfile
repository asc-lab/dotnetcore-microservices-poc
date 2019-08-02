FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY pricing-service/PricingService.Api/*.csproj ./pricing-service/PricingService.Api/
COPY policy-service/PolicyService.Api/*.csproj ./policy-service/PolicyService.Api/
COPY policy-service/PolicyService/*.csproj ./policy-service/PolicyService/
RUN  cd policy-service/PolicyService && dotnet restore

COPY pricing-service/PricingService.Api ./pricing-service/PricingService.Api/
COPY policy-service/PolicyService.Api ./policy-service/PolicyService.Api/
COPY policy-service/PolicyService ./policy-service/PolicyService/
RUN cd policy-service/PolicyService && dotnet build

COPY policy-service/PolicyService.Test/*.csproj ./policy-service/PolicyService.Test/
RUN cd policy-service/PolicyService.Test && dotnet restore

COPY policy-service/PolicyService.Test ./policy-service/PolicyService.Test/
RUN cd policy-service/PolicyService.Test && dotnet build

FROM build AS test
WORKDIR /src/policy-service/PolicyService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/policy-service/PolicyService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5050
COPY --from=publish /src/policy-service/PolicyService/out ./
ENTRYPOINT ["dotnet", "PolicyService.dll"]