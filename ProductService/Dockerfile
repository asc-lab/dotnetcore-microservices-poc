FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 63720
EXPOSE 44398

FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY ProductService/ProductService/ProductService.csproj ProductService/ProductService/
RUN dotnet restore ProductService/ProductService/ProductService.csproj
COPY . .
WORKDIR /src/ProductService/ProductService
RUN dotnet build ProductService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ProductService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductService.dll"]
