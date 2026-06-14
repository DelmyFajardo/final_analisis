FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["FinalAnalisiss.csproj", "./"]
RUN dotnet restore "FinalAnalisiss.csproj"

COPY . .

RUN dotnet build "FinalAnalisiss.csproj" -c Release -o /app/build
RUN dotnet publish "FinalAnalisiss.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "FinalAnalisiss.dll"]