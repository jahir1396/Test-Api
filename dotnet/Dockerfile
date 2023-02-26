
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
#Puerto disponible 8181

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Test.sln ./
COPY Test.Core/*.csproj ./Test.Core/
COPY Test.EntityFramework/*.csproj ./Test.EntityFramework/
COPY Test.Web/*.csproj ./Test.Web/

RUN dotnet restore
COPY . .
WORKDIR /src/Test.Core
RUN dotnet build -c Release -o /app

WORKDIR /src/Test.EntityFramework
RUN dotnet build -c Release -o /app

WORKDIR /src/Test.Web
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Test.Web.dll"]

