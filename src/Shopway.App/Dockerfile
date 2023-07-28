	#Build stage

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/Shopway.App/Shopway.App.csproj", "src/Shopway.App/"]
COPY ["src/Shopway.Application/Shopway.Application.csproj", "src/Shopway.Application/"]
COPY ["src/Shopway.Domain/Shopway.Domain.csproj", "src/Shopway.Domain/"]
COPY ["src/Shopway.Persistence/Shopway.Persistence.csproj", "src/Shopway.Persistence/"]
COPY ["src/Shopway.Infrastructure/Shopway.Infrastructure.csproj", "src/Shopway.Infrastructure/"]
COPY ["src/Shopway.Presentation/Shopway.Presentation.csproj", "src/Shopway.Presentation/"]
RUN dotnet restore "src/Shopway.Domain/Shopway.Domain.csproj"
RUN dotnet restore "src/Shopway.Persistence/Shopway.Persistence.csproj"
RUN dotnet restore "src/Shopway.Infrastructure/Shopway.Infrastructure.csproj"
RUN dotnet restore "src/Shopway.Presentation/Shopway.Presentation.csproj"
RUN dotnet restore "src/Shopway.App/Shopway.App.csproj"
COPY . .
WORKDIR "/src/src/Shopway.App"
RUN dotnet build "Shopway.App.csproj" -c Release -o /app/build

	#Publish stage

FROM build AS publish
RUN dotnet publish "Shopway.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

	#Final stage

FROM base AS final

#Set the name of the image owner
LABEL Name="dr-marek-jaskula"

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopway.App.dll"]