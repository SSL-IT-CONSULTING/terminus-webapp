# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY UI/*.csproj ./UI/
COPY Shared/terminus.shared.models.csproj ./Shared/

RUN dotnet restore

# copy everything else and build app
COPY . .

WORKDIR /source/UI
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV ASPNETCORE_URLS http://0.0.0.0:5000
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "terminus-webapp.dll"]