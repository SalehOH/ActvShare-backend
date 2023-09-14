

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ActvShare.WebApi/ActvShare.WebApi.csproj", "src/ActvShare.WebApi/"]
COPY ["src/ActvShare.Application/ActvShare.Application.csproj", "src/ActvShare.Application/"]
COPY ["src/ActvShare.Domain/ActvShare.Domain.csproj", "src/ActvShare.Domain/"]
COPY ["src/ActvShare.Infrastructure/ActvShare.Infrastructure.csproj", "src/ActvShare.Infrastructure/"]
RUN dotnet restore "src/ActvShare.WebApi/ActvShare.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ActvShare.WebApi"
RUN dotnet build "ActvShare.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActvShare.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActvShare.WebApi.dll"]