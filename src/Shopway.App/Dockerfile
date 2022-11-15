FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Shopway.App/Shopway.App.csproj", "src/Shopway.App/"]
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