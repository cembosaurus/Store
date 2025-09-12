FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["API/Services.SYNC/Identity/Identity.csproj", "Services.SYNC/Identity/"]
COPY ["API/Business/Business.csproj", "Business/"]
RUN dotnet restore "Services.SYNC/Identity/Identity.csproj"
COPY . .
RUN dotnet build "API/Services.SYNC/Identity/Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/Services.SYNC/Identity/Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll", "--environment=Production"]
