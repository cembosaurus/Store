FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["API/Inventory.Consumer/Inventory.Consumer.csproj", "Inventory.Consumer/"]
COPY ["API/Business/Business.csproj", "Business/"]
RUN dotnet restore "Inventory.Consumer/Inventory.Consumer.csproj"
COPY . .
RUN dotnet build "API/Inventory.Consumer/Inventory.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/Inventory.Consumer/Inventory.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Consumer.dll", "--environment=Production"]
