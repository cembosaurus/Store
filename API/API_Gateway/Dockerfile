FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API_Gateway/API_Gateway.csproj", "API_Gateway/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "API_Gateway/API_Gateway.csproj"
COPY . .
RUN dotnet build "API_Gateway/API_Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_Gateway/API_Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Gateway.dll", "--environment=Production"]
