# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../../Backend-Bar.sln ./
COPY ../BarGunter.Domain/ ./BarGunter.Domain/
COPY ../BarGunter.Application/ ./BarGunter.Application/
COPY ../BarGunter.Infrastructure/ ./BarGunter.Infrastructure/
COPY . ./BarGunter.API/
RUN dotnet restore BarGunter.API/BarGunter.API.csproj
RUN dotnet publish BarGunter.API/BarGunter.API.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "BarGunter.API.dll"]
