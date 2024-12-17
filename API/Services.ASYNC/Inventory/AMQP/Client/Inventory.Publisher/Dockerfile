FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["API/Inventory.Publisher/Inventory.Publisher.csproj", "Inventory.Publisher/"]
COPY ["API/Business/Business.csproj", "Business/"]
RUN dotnet restore "Inventory.Publisher/Inventory.Publisher.csproj"
COPY . .
RUN dotnet build "API/Inventory.Publisher/Inventory.Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/Inventory.Publisher/Inventory.Publisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Publisher.dll", "--environment=Production"]
