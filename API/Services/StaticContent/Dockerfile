FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/StaticContent/StaticContent.csproj", "Services/StaticContent/"]
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Services/StaticContent/StaticContent.csproj"
COPY . .
RUN dotnet build "Services/StaticContent/StaticContent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/StaticContent/StaticContent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StaticContent.dll", "--environment=Production"]