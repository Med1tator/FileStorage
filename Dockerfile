#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/FileStorage.WebApi/FileStorage.WebApi.csproj", "FileStorage.WebApi/"]
RUN dotnet restore "FileStorage.WebApi/FileStorage.WebApi.csproj"
COPY src/FileStorage.WebApi/ FileStorage.WebApi/
WORKDIR "/src/FileStorage.WebApi"
RUN dotnet build "FileStorage.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileStorage.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FileStorage.WebApi.dll"]