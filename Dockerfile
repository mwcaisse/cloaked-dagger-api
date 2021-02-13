FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# CA Certificats are outdated in base image, update before proceeding so we can restore nuget packages
RUN apt update -y && apt upgrade -y

ARG OWLTIN_SOURCE
ARG OWLTIN_USERNAME
ARG OWLTIN_PASSWORD

WORKDIR /build/src/

COPY . .

WORKDIR /build/src/CloakedDagger.Web

# Configure the package source, only clear text is supported on linux, but its build stage anyway
RUN dotnet nuget add source ${OWLTIN_SOURCE} --name owltin --username ${OWLTIN_USERNAME} --password ${OWLTIN_PASSWORD} --store-password-in-clear-text

RUN dotnet restore
RUN dotnet publish -c Release -o /build/out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app

COPY --from=build /build/out ./

# Run as a user other than root
RUN groupadd -g 999 appuser && useradd -r -u 999 -g appuser -m appuser
RUN chown appuser:appuser -R /app
USER 999

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "CloakedDagger.Web.dll"]