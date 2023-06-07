FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Inventory.Publisher/Inventory.Publisher.csproj", "Inventory.Publisher/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Inventory.Publisher/Inventory.Publisher.csproj"
COPY . .
RUN dotnet build "Inventory.Publisher/Inventory.Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Publisher/Inventory.Publisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Publisher.dll", "--environment=Production"]
