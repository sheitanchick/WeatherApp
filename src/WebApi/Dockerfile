FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY *.sln .
COPY src/WebApi/*.csproj ./src/WebApi/
COPY src/Weather.Domain/*.csproj ./src/Weather.Domain/
COPY src/Weather.Infrastructure/*.csproj ./src/Weather.Infrastructure/
COPY src/Weather.Application/*.csproj ./src/Weather.Application/
COPY tests/Weather.Tests/*.csproj ./tests/Weather.Tests/

RUN dotnet restore
COPY . .
WORKDIR "/src/src/WebApi"

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish --runtime linux-x64

FROM base AS final
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]