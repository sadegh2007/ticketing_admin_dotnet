FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERP.Ticketing.HttpApi/ERP.Ticketing.HttpApi.csproj", "ERP.Ticketing.HttpApi/"]
RUN dotnet restore "ERP.Ticketing.HttpApi/ERP.Ticketing.HttpApi.csproj"
COPY . .
WORKDIR "/src/ERP.Ticketing.HttpApi"
RUN dotnet build "ERP.Ticketing.HttpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Ticketing.HttpApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Ticketing.HttpApi.dll"]
