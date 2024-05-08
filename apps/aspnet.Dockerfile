FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PROJECT_NAME
WORKDIR /src
COPY . .
RUN dotnet publish "${PROJECT_NAME}.csproj" -c Release -o /app/publish --no-self-contained
RUN mv "/app/publish/${PROJECT_NAME}.dll" "/app/publish/app.dll"
RUN mv "/app/publish/${PROJECT_NAME}.runtimeconfig.json" "/app/publish/app.runtimeconfig.json"

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
USER 1000
EXPOSE 8080 
ENTRYPOINT ["dotnet", "app.dll"]
