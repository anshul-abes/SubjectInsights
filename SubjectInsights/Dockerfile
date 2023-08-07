#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SubjectInsights/SubjectInsights.API.csproj", "SubjectInsights/"]
COPY ["SubjectInsights.Engine/SubjectInsights.Infrastructure.csproj", "SubjectInsights.Engine/"]
COPY ["SubjectInsights.Repository/SubjectInsights.Application.csproj", "SubjectInsights.Repository/"]
COPY ["SubjectInsights.Logger/SubjectInsights.Logger.csproj", "SubjectInsights.Logger/"]
COPY ["SubjectInsights.Common/SubjectInsights.Common.csproj", "SubjectInsights.Common/"]
COPY ["SubjectInsights.Service/SubjectInsights.Service.csproj", "SubjectInsights.Service/"]
RUN dotnet restore "SubjectInsights/SubjectInsights.API.csproj"
COPY . .
WORKDIR "/src/SubjectInsights"
RUN dotnet build "SubjectInsights.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SubjectInsights.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubjectInsights.API.dll"]