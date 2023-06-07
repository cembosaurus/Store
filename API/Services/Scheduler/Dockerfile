FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Scheduler/Scheduler.csproj", "Services/Scheduler/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Services/Scheduler/Scheduler.csproj"
COPY . .
RUN dotnet build "Services/Scheduler/Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/Scheduler/Scheduler.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scheduler.dll", "--environment=Production"]