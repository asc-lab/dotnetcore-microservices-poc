FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME

COPY PolicyService.Api/*.csproj $APP_HOME/PolicyService.Api/
COPY PaymentService.Api/*.csproj $APP_HOME/PaymentService.Api/
COPY PaymentService/*.csproj $APP_HOME/PaymentService/
RUN  cd $APP_HOME/PaymentService && dotnet restore

COPY PolicyService.Api $APP_HOME/PolicyService.Api/
COPY PaymentService.Api $APP_HOME/PaymentService.Api/
COPY PaymentService $APP_HOME/PaymentService/
RUN cd $APP_HOME/PaymentService && dotnet build

COPY PaymentService.Test/*.csproj $APP_HOME/PaymentService.Test/
RUN cd $APP_HOME/PaymentService.Test && dotnet restore

COPY PaymentService.Test .$APP_HOME/PaymentService.Test/
RUN cd $APP_HOME/PaymentService.Test && dotnet build

FROM build AS test
WORKDIR $APP_HOME/PaymentService.Test
RUN dotnet test --verbosity:normal

FROM build AS publish
WORKDIR $APP_HOME/PaymentService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5070
COPY --from=publish /opt/app/PaymentService/out ./
ENTRYPOINT ["dotnet", "PaymentService.dll"]