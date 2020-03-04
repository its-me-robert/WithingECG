#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WithingsECG.API/WithingsECG.API.csproj", "WithingsECG.API/"]
RUN dotnet restore "WithingsECG.API/WithingsECG.API.csproj"
COPY . .
WORKDIR "/src/WithingsECG.API"
RUN dotnet build "WithingsECG.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WithingsECG.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WithingsECG.API.dll"]