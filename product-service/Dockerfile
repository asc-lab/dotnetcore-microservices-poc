FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY product-service/ProductService.Api/*.csproj ./product-service/ProductService.Api/
COPY product-service/ProductService/*.csproj ./product-service/ProductService/
RUN  cd product-service/ProductService && dotnet restore

COPY product-service/ProductService.Api ./product-service/ProductService.Api/
COPY product-service/ProductService ./product-service/ProductService/
RUN cd product-service/ProductService && dotnet build

COPY product-service/ProductService.Test/*.csproj ./product-service/ProductService.Test/
RUN cd product-service/ProductService.Test && dotnet restore

COPY product-service/ProductService.Test ./product-service/ProductService.Test/
RUN cd product-service/ProductService.Test && dotnet build

FROM build AS test
WORKDIR /src/product-service/ProductService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR /src/product-service/ProductService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5030
COPY --from=publish /src/product-service/ProductService/out ./
ENTRYPOINT ["dotnet", "ProductService.dll"]