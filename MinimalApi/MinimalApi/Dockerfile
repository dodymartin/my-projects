#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Setup Ports
FROM		mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR		/app
EXPOSE		80
EXPOSE		443

#Setup Environment
ENV			CONFIG_PATH=/app/config
ENV			CONFIG_SITE_PATH=/app/configbysite
ENV			DOCKER_RUNNING=true
ENV			ASPNETCORE_ENVIRONMENT=development

#Pull nuget packages
FROM		mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR		/src
COPY		["MinimalApi/MinimalApi.Api.csproj", "MinimalApi/"]
RUN			dotnet restore "MinimalApi/MinimalApi.Api.csproj"

#Build app
COPY		. .
WORKDIR		/src/MinimalApi
RUN			dotnet build "MinimalApi.Api.csproj" -c Release -o /app/build

#Publish app
FROM		build AS publish
RUN			dotnet publish "MinimalApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM		base AS final
WORKDIR		/app

#Copy config settings exclusive by app to sub-directory
COPY		MinimalApi/NLog.json config/nlog.json
COPY		MinimalApi/appsettings.json config/
COPY		MinimalApi/appsettings.Development.json config/appsettings.development.json

COPY		--from=publish /app/publish .

#Remove config settings files from app directory
RUN			rm -f /app/NLog.json
RUN			rm -f /app/appsettings.json
RUN			rm -f /app/appsettings.Development.json

ENTRYPOINT	["dotnet", "MinimalApi.Api.dll"]