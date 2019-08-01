FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

COPY policy-service/PolicyService.Api/*.csproj ./policy-service/PolicyService.Api/
COPY chat-service/ChatService.Api/*.csproj ./chat-service/ChatService.Api/
COPY chat-service/ChatService/*.csproj ./chat-service/ChatService/
RUN  cd chat-service/ChatService && dotnet restore

COPY policy-service/PolicyService.Api ./policy-service/PolicyService.Api/
COPY chat-service/ChatService.Api ./chat-service/ChatService.Api/
COPY chat-service/ChatService ./chat-service/ChatService/
RUN cd chat-service/ChatService && dotnet build

FROM build AS publish
WORKDIR /src/chat-service/ChatService
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
EXPOSE 63720
EXPOSE 44398
COPY --from=publish /src/chat-service/ChatService/out ./
ENTRYPOINT ["dotnet", "ChatService.dll"]