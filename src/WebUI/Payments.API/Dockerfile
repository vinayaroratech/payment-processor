#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/WebUI/Payments.API/Payments.API.csproj", "src/WebUI/Payments.API/"]
COPY ["src/Infrastructure/Payments.Infrastructure/Payments.Infrastructure.csproj", "src/Infrastructure/Payments.Infrastructure/"]
COPY ["src/Application/Payments.Application/Payments.Application.csproj", "src/Application/Payments.Application/"]
COPY ["src/Domain/Payments.Domain/Payments.Domain.csproj", "src/Domain/Payments.Domain/"]
RUN dotnet restore "src/WebUI/Payments.API/Payments.API.csproj"
COPY . .
WORKDIR "/src/src/WebUI/Payments.API"
RUN dotnet build "Payments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payments.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payments.API.dll"]