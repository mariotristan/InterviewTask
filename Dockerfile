# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

COPY . ./

WORKDIR /app/RNGService

RUN dotnet restore

RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o out --no-restore

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 4040
ENV ASPNETCORE_URLS=http://*:4040

CMD ["dotnet", "/app/RNGService/out/RNGService.dll"]

