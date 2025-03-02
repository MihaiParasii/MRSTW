FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MRSTW.Api/MRSTW.Api.csproj", "MRSTW.Api/"]
COPY ["MRSTW.BusinessLogicLayer/MRSTW.BusinessLogicLayer.csproj", "MRSTW.BusinessLogicLayer/"]
COPY ["MRSTW.DataAccessLayer/MRSTW.DataAccessLayer.csproj", "MRSTW.DataAccessLayer/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "MRSTW.Api/MRSTW.Api.csproj"
COPY . .
WORKDIR "/src/MRSTW.Api"
RUN dotnet build "MRSTW.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MRSTW.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MRSTW.Api.dll"]
