FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

COPY agent-portal-gateway/AgentPortalApiGateway/*.csproj ./agent-portal-gateway/AgentPortalApiGateway/
RUN  cd agent-portal-gateway/AgentPortalApiGateway && dotnet restore

COPY agent-portal-gateway/AgentPortalApiGateway ./agent-portal-gateway/AgentPortalApiGateway/
RUN cd agent-portal-gateway/AgentPortalApiGateway && dotnet build

FROM build AS publish
WORKDIR /src/agent-portal-gateway/AgentPortalApiGateway
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8081
COPY --from=publish /src/agent-portal-gateway/AgentPortalApiGateway/out ./
ENTRYPOINT ["dotnet", "AgentPortalApiGateway.dll"]