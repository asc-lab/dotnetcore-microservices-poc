FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY PolicyService.Api/*.csproj $APP_HOME/PolicyService.Api/
COPY ChatService.Api/*.csproj $APP_HOME/ChatService.Api/
COPY ChatService/*.csproj $APP_HOME/ChatService/
RUN  cd $APP_HOME/ChatService && dotnet restore

COPY PolicyService.Api $APP_HOME/PolicyService.Api/
COPY ChatService.Api $APP_HOME/ChatService.Api/
COPY ChatService $APP_HOME/ChatService/
RUN cd $APP_HOME/ChatService && dotnet build

FROM build AS publish
WORKDIR $APP_HOME/ChatService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:4099
COPY --from=publish /opt/app/ChatService/out ./
ENTRYPOINT ["dotnet", "ChatService.dll"]