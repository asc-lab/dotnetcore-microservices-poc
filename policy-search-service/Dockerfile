FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY policy-service/PolicyService.Api/*.csproj ./policy-service/PolicyService.Api/
COPY policy-search-service/PolicySearchService.Api/*.csproj ./policy-search-service/PolicySearchService.Api/
COPY policy-search-service/PolicySearchService/*.csproj ./policy-search-service/PolicySearchService/
RUN  cd policy-search-service/PolicySearchService && dotnet restore

COPY policy-service/PolicyService.Api ./policy-service/PolicyService.Api/
COPY policy-search-service/PolicySearchService.Api ./policy-search-service/PolicySearchService.Api/
COPY policy-search-service/PolicySearchService ./policy-search-service/PolicySearchService/
RUN cd policy-search-service/PolicySearchService && dotnet build

FROM build AS publish
WORKDIR /src/policy-search-service/PolicySearchService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
EXPOSE 63720
EXPOSE 44398
COPY --from=publish /src/policy-search-service/PolicySearchService/out ./
ENTRYPOINT ["dotnet", "PolicySearchService.dll"]