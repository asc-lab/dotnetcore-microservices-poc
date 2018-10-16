FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53050
EXPOSE 44360

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["PaymentService/PaymentService.csproj", "PaymentService/"]
RUN dotnet restore "PaymentService/PaymentService.csproj"
COPY . .
WORKDIR "/src/PaymentService"
RUN dotnet build "PaymentService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PaymentService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentService.dll"]