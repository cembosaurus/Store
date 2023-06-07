FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Inventory/Inventory.csproj", "Services/Inventory/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Services/Inventory/Inventory.csproj"
COPY . .
RUN dotnet build "Services/Inventory/Inventory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/Inventory/Inventory.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.dll", "--environment=Production"]
