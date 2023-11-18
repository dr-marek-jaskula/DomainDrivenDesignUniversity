	#Build stage

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Shopway.Domain/Shopway.Domain.csproj", "src/Shopway.Domain/"]
COPY ["src/Shopway.Application/Shopway.Application.csproj", "src/Shopway.Application/"]
COPY ["src/Shopway.Infrastructure/Shopway.Infrastructure.csproj", "src/Shopway.Infrastructure/"]
COPY ["src/Shopway.Presentation/Shopway.Presentation.csproj", "src/Shopway.Presentation/"]
COPY ["src/Shopway.Persistence/Shopway.Persistence.csproj", "src/Shopway.Persistence/"]
COPY ["src/Shopway.App/Shopway.App.csproj", "src/Shopway.App/"]
RUN dotnet restore "src/Shopway.Domain/Shopway.Domain.csproj"
RUN dotnet restore "src/Shopway.Application/Shopway.Application.csproj"
RUN dotnet restore "src/Shopway.Infrastructure/Shopway.Infrastructure.csproj"
RUN dotnet restore "src/Shopway.Presentation/Shopway.Presentation.csproj"
RUN dotnet restore "src/Shopway.Persistence/Shopway.Persistence.csproj"
RUN dotnet restore "src/Shopway.App/Shopway.App.csproj"
COPY . .
WORKDIR "/src/src/Shopway.App"
RUN dotnet build "Shopway.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

	#Publish stage

FROM build AS publish
ARG PUBLISH_CONFIGURATION=Release
RUN dotnet publish "Shopway.App.csproj" -c $PUBLISH_CONFIGURATION -o /app/publish /p:UseAppHost=false

	#Final stage

FROM base AS final

#Set the name of the image owner
LABEL Name="dr-marek-jaskula"

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopway.App.dll"]
