FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Umbracool.sln ./
COPY src/Umbracool.Web/Directory.Packages.props src/Umbracool.Web/
COPY src/Umbracool.Web/Umbracool.Web.csproj src/Umbracool.Web/
RUN dotnet restore src/Umbracool.Web/Umbracool.Web.csproj

COPY src/Umbracool.Web/ src/Umbracool.Web/
RUN dotnet publish src/Umbracool.Web/Umbracool.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

RUN mkdir -p /app/wwwroot/media

COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "Umbracool.Web.dll"]
