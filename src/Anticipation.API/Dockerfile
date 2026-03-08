FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Anticipation.API/Anticipation.API.csproj", "src/Anticipation.API/"]
COPY ["src/Anticipation.Application/Anticipation.Application.csproj", "src/Anticipation.Application/"]
COPY ["src/Anticipation.Domain/Anticipation.Domain.csproj", "src/Anticipation.Domain/"]
COPY ["src/Anticipation.Infrastructure/Anticipation.Infrastructure.csproj", "src/Anticipation.Infrastructure/"]
RUN dotnet restore "src/Anticipation.API/Anticipation.API.csproj"

COPY . .
RUN dotnet publish "src/Anticipation.API/Anticipation.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update \
	&& apt-get install -y --no-install-recommends curl \
	&& rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Anticipation.API.dll"]