# Use an Ubuntu base image
FROM ubuntu:latest AS build

RUN export DEBIAN_FRONTEND=noninteractive; \
    apt-get update \
    # Install prerequisites
    && apt-get install -y --no-install-recommends \
       wget \
       ca-certificates \
    \
    # Install Microsoft package feed
    && wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    \
    # Install .NET
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-sdk-8.0

WORKDIR /app
COPY . .

RUN dotnet build -c Release
RUN dotnet publish -c Release -o out

FROM ubuntu:latest AS runtime

RUN export DEBIAN_FRONTEND=noninteractive; \
    apt-get update \
    # Install prerequisites
    && apt-get install -y --no-install-recommends \
       wget \
       ca-certificates \
    \
    # Install Microsoft package feed
    && wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    \
    # Install .NET
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        aspnetcore-runtime-8.0

WORKDIR /app

COPY --from=build /app .

EXPOSE 4040
ENV ASPNETCORE_URLS=http://*:4040

CMD ["dotnet", "/app/out/RNGService.dll"]
