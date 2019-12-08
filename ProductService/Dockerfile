FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY ProductService.Api/*.csproj $APP_HOME/ProductService.Api/
COPY ProductService/*.csproj $APP_HOME/ProductService/
RUN  cd $APP_HOME/ProductService && dotnet restore

COPY ProductService.Api $APP_HOME/ProductService.Api/
COPY ProductService $APP_HOME/ProductService/
RUN cd $APP_HOME/ProductService && dotnet build

COPY ProductService.Test/*.csproj $APP_HOME/ProductService.Test/
RUN cd $APP_HOME/ProductService.Test && dotnet restore

COPY ProductService.Test $APP_HOME/ProductService.Test/
RUN cd $APP_HOME/ProductService.Test && dotnet build

FROM build AS test
WORKDIR $APP_HOME/ProductService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR $APP_HOME/ProductService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5030
COPY --from=publish /opt/app/ProductService/out ./
ENTRYPOINT ["dotnet", "ProductService.dll"]