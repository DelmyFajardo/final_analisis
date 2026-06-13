FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FinalAnalisiss.csproj ./
RUN dotnet restore FinalAnalisiss.csproj

COPY . ./
RUN dotnet publish FinalAnalisiss.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "FinalAnalisiss.dll"]