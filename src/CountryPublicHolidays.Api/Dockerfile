#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CountryPublicHolidays.Api/CountryPublicHolidays.Api.csproj", "src/CountryPublicHolidays.Api/"]
COPY ["src/CountryPublicHolidays.ServiceLibrary/CountryPublicHolidays.ServiceLibrary.csproj", "src/CountryPublicHolidays.ServiceLibrary/"]
RUN dotnet restore "src/CountryPublicHolidays.Api/CountryPublicHolidays.Api.csproj"
COPY . .
WORKDIR "/src/src/CountryPublicHolidays.Api"
RUN dotnet build "CountryPublicHolidays.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CountryPublicHolidays.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CountryPublicHolidays.Api.dll"]