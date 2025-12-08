FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ["src/Logistics.Logs.Web.Host/Logistics.Logs.Web.Host.csproj", "src/Logistics.Logs.Web.Host/"]
COPY ["src/Logistics.Logs.Web.Core/Logistics.Logs.Web.Core.csproj", "src/Logistics.Logs.Web.Core/"]
COPY ["src/Logistics.Logs.Application/Logistics.Logs.Application.csproj", "src/Logistics.Logs.Application/"]
COPY ["src/Logistics.Logs.Core/Logistics.Logs.Core.csproj", "src/Logistics.Logs.Core/"]
COPY ["src/Logistics.Logs.EntityFrameworkCore/Logistics.Logs.EntityFrameworkCore.csproj", "src/Logistics.Logs.EntityFrameworkCore/"]
WORKDIR "/src/src/Logistics.Logs.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Logistics.Logs.Web.Host", "src/Logistics.Logs.Web.Host"]
COPY ["src/Logistics.Logs.Web.Core", "src/Logistics.Logs.Web.Core"]
COPY ["src/Logistics.Logs.Application", "src/Logistics.Logs.Application"]
COPY ["src/Logistics.Logs.Core", "src/Logistics.Logs.Core"]
COPY ["src/Logistics.Logs.EntityFrameworkCore", "src/Logistics.Logs.EntityFrameworkCore"]
WORKDIR "/src/src/Logistics.Logs.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Logistics.Logs.Web.Host.dll"]
