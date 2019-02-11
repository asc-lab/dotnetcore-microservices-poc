FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY PolicyService.Api/*.csproj ./PolicyService.Api/
COPY PaymentService.Api/*.csproj ./PaymentService.Api/
COPY PaymentService/*.csproj ./PaymentService/
RUN  cd PaymentService && dotnet restore

COPY PolicyService.Api ./PolicyService.Api/
COPY PaymentService.Api ./PaymentService.Api/
COPY PaymentService ./PaymentService/
RUN cd PaymentService && dotnet build

COPY PaymentService.Test/*.csproj ./PaymentService.Test/
RUN cd PaymentService.Test && dotnet restore

COPY PaymentService.Test ./PaymentService.Test/
RUN cd PaymentService.Test && dotnet build

FROM build AS test
WORKDIR /src/PaymentService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/PaymentService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
EXPOSE 53050
EXPOSE 44360
COPY --from=publish /src/PaymentService/out ./
ENTRYPOINT ["dotnet", "PaymentService.dll"]