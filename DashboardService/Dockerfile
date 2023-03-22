FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY DashboardService.Api/*.csproj $APP_HOME/DashboardService.Api/
COPY PolicyService.Api/*.csproj $APP_HOME/PolicyService.Api/
COPY DashboardService/*.csproj $APP_HOME/DashboardService/
RUN  cd $APP_HOME/DashboardService && dotnet restore

COPY DashboardService.Api $APP_HOME/DashboardService.Api/
COPY PolicyService.Api $APP_HOME/PolicyService.Api/
COPY DashboardService $APP_HOME/DashboardService/
RUN cd $APP_HOME/DashboardService && dotnet build

FROM build AS publish
WORKDIR $APP_HOME/DashboardService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5035
ENV ASPNETCORE_ENVIRONMENT=docker
COPY --from=publish /opt/app/DashboardService/out ./
ENTRYPOINT ["dotnet", "DashboardService.dll"]