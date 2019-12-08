FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY AuthService/*.csproj $APP_HOME/
RUN cd $APP_HOME && dotnet restore

COPY AuthService/ $APP_HOME/
RUN cd $APP_HOME && dotnet build

FROM build AS publish
WORKDIR $APP_HOME
RUN cd $APP_HOME && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:6060
COPY --from=publish /opt/app/out ./
ENTRYPOINT ["dotnet", "AuthService.dll"]