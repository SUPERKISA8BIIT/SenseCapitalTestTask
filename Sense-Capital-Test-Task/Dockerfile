#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sense-Capital-Test-Task/Sense-Capital-Test-Task.csproj", "Sense-Capital-Test-Task/"]
RUN dotnet restore "Sense-Capital-Test-Task/Sense-Capital-Test-Task.csproj"
COPY . .
WORKDIR "/src/Sense-Capital-Test-Task"
RUN dotnet build "Sense-Capital-Test-Task.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sense-Capital-Test-Task.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sense-Capital-Test-Task.dll"]