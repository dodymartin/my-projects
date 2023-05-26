#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinimalApi/MinimalApi/MinimalApi.csproj", "MinimalApi/"]
COPY ./nuget.config /root/.nuget/NuGet/
RUN dotnet restore "MinimalApi/MinimalApi.csproj" --configfile /root/.nuget/NuGet/nuget.config
COPY . .
WORKDIR "/src/MinimalApi"
RUN dotnet build "MinimalApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN del -f /root/.nuget/NuGet/nuget.config
ENTRYPOINT ["dotnet", "MinimalApi.dll"]