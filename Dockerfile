# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app





COPY . ./
RUN ls 

WORKDIR /app/RNGService

RUN dotnet restore

RUN dotnet build -c Release --no-restore

# Run tests
RUN dotnet test --no-restore --verbosity normal

# Publish the application
RUN dotnet publish -c Release -o /out --no-restore

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

EXPOSE 4040
ENV ASPNETCORE_URLS=http://*:4040

CMD ["dotnet", "/app/out/RNGService.dll"]

