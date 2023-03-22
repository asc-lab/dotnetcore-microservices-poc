FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY PricingService.Api/*.csproj $APP_HOME/PricingService.Api/
COPY PricingService/*.csproj $APP_HOME/PricingService/
RUN cd $APP_HOME/PricingService && dotnet restore

COPY PricingService.Api $APP_HOME/PricingService.Api/
COPY PricingService $APP_HOME/PricingService/
RUN cd $APP_HOME/PricingService && dotnet build

COPY PricingService.Test/*.csproj $APP_HOME/PricingService.Test/
RUN cd $APP_HOME/PricingService.Test && dotnet restore

COPY PricingService.Test $APP_HOME/PricingService.Test/
RUN cd $APP_HOME/PricingService.Test && dotnet build

FROM build AS test
WORKDIR $APP_HOME/PricingService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR $APP_HOME/PricingService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5040
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/PricingService/out ./
ENTRYPOINT ["dotnet", "PricingService.dll"]