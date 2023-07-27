#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Shopway.App/Shopway.App.csproj", "src/Shopway.App/"]
COPY ["src/Shopway.Application/Shopway.Application.csproj", "src/Shopway.Application/"]
COPY ["src/Shopway.Domain/Shopway.Domain.csproj", "src/Shopway.Domain/"]
COPY ["src/Shopway.Persistence/Shopway.Persistence.csproj", "src/Shopway.Persistence/"]
COPY ["src/Shopway.Infrastructure/Shopway.Infrastructure.csproj", "src/Shopway.Infrastructure/"]
COPY ["src/Shopway.Presentation/Shopway.Presentation.csproj", "src/Shopway.Presentation/"]
RUN dotnet restore "src/Shopway.App/Shopway.App.csproj"
COPY . .
WORKDIR "/src/src/Shopway.App"
RUN dotnet build "Shopway.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shopway.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopway.App.dll"]