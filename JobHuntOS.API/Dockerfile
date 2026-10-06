FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["JobHuntOS.API/JobHuntOS.API.csproj", "JobHuntOS.API/"]
COPY ["JobHuntOS.Application/JobHuntOS.Application.csproj", "JobHuntOS.Application/"]
COPY ["JobHuntOS.Domain/JobHuntOS.Domain.csproj", "JobHuntOS.Domain/"]
COPY ["JobHuntOS.Infrastructure/JobHuntOS.Infrastructure.csproj", "JobHuntOS.Infrastructure/"]

RUN dotnet restore "JobHuntOS.API/JobHuntOS.API.csproj"

COPY . .

RUN dotnet publish "JobHuntOS.API/JobHuntOS.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobHuntOS.API.dll"]