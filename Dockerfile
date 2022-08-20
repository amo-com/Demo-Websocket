# syntax=docker/dockerfile:experimental
FROM docker/dockerfile:experimental
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
WORKDIR /app/Service.Api
RUN --mount=type=cache,target=/root/.nuget,id=nuget-cache,rw dotnet restore
RUN --mount=type=cache,target=/root/.nuget,id=nuget-cache,rw dotnet publish -c Release -o out

# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
WORKDIR /app/service
EXPOSE 8080

# env config
RUN mkdir /app/config
RUN mkdir /app/log
RUN mkdir /app/data

# ENV ASPNETCORE_PATH /app/config

COPY --from=build /app/Service.Api/out ./
ENTRYPOINT ["dotnet", "Service.Api.dll"]