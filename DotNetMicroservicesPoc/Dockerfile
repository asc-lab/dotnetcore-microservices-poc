FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 40083
EXPOSE 44328

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DotNetMicroservicesPoc/DotNetMicroservicesPoc.csproj", "DotNetMicroservicesPoc/"]
RUN dotnet restore "DotNetMicroservicesPoc/DotNetMicroservicesPoc.csproj"
COPY . .
WORKDIR "/src/DotNetMicroservicesPoc"
RUN dotnet build "DotNetMicroservicesPoc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DotNetMicroservicesPoc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DotNetMicroservicesPoc.dll"]