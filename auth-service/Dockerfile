FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY auth-service/AuthService/*.csproj ./auth-service/AuthService/
RUN  cd auth-service/AuthService && dotnet restore

COPY auth-service/AuthService ./auth-service/AuthService/
RUN cd auth-service/AuthService && dotnet build

FROM build AS publish
WORKDIR /src/auth-service/AuthService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=publish /src/auth-service/AuthService/out ./
ENV ASPNETCORE_URLS=http://+:6060
ENTRYPOINT ["dotnet", "AuthService.dll"]