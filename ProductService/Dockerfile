FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src

COPY ProductService.Api/*.csproj ./ProductService.Api/
COPY ProductService/*.csproj ./ProductService/
RUN  cd ProductService && dotnet restore

COPY ProductService.Api ./ProductService.Api/
COPY ProductService ./ProductService/
RUN cd ProductService && dotnet build

COPY ProductService.Test/*.csproj ./ProductService.Test/
RUN cd ProductService.Test && dotnet restore

COPY ProductService.Test ./ProductService.Test/
RUN cd ProductService.Test && dotnet build

FROM build AS test
WORKDIR /src/ProductService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/ProductService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
WORKDIR /app
EXPOSE 63720
EXPOSE 44398
COPY --from=publish /src/ProductService/out ./
ENTRYPOINT ["dotnet", "ProductService.dll"]