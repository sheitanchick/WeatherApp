FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Weather.csproj", "."]
RUN dotnet restore "./Weather.csproj"
COPY . .
WORKDIR "/src/."

FROM build AS publish
RUN dotnet publish "Weather.csproj" -c Release -o /app/publish --runtime linux-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Weather.dll"]