FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY StockTrackerAPI.csproj ./
RUN dotnet restore StockTrackerAPI.csproj

COPY . ./
RUN dotnet publish StockTrackerAPI.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish ./

USER app

ENTRYPOINT ["dotnet", "StockTrackerAPI.dll"]