FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OpenCodeChems.Service/OpenCodeChems.Service.csproj", "OpenCodeChems.Service/"]
RUN dotnet restore "OpenCodeChems.Service/OpenCodeChems.Service.csproj"
COPY . .
WORKDIR "/src/OpenCodeChems.Service"
RUN dotnet build "OpenCodeChems.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenCodeChems.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenCodeChems.Service.exe"]
