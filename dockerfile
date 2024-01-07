FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["ActvShare-backend/src/ActvShare.WebApi/ActvShare.WebApi.csproj", "src/ActvShare.WebApi/"]
COPY ["ActvShare-backend/src/ActvShare.Application/ActvShare.Application.csproj", "src/ActvShare.Application/"]
COPY ["ActvShare-backend/src/ActvShare.Domain/ActvShare.Domain.csproj", "src/ActvShare.Domain/"]
COPY ["ActvShare-backend/src/ActvShare.Infrastructure/ActvShare.Infrastructure.csproj", "src/ActvShare.Infrastructure/"]

RUN dotnet restore "src/ActvShare.WebApi/ActvShare.WebApi.csproj"

WORKDIR "/src/src/ActvShare.WebApi"

COPY . .
RUN dotnet build "ActvShare.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActvShare.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p wwwroot/images

COPY ["ActvShare-backend/src/ActvShare.WebApi/appsettings.json", "./"]
ENTRYPOINT ["dotnet", "ActvShare.WebApi.dll"]
