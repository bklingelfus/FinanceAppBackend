FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FinanceAppBackend/FinanceAppBackend.csproj", "FinanceAppBackend/"]
RUN dotnet restore "FinanceAppBackend/FinanceAppBackend.csproj"
COPY . .
WORKDIR "/src/FinanceAppBackend"
RUN dotnet build "FinanceAppBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceAppBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet FinanceAppBackend.dll