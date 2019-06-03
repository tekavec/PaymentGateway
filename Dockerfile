FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PaymentGateway/PaymentGateway.csproj", "PaymentGateway/"]
RUN dotnet restore "PaymentGateway/PaymentGateway.csproj"
COPY . .
WORKDIR "/src/PaymentGateway"
RUN dotnet build "PaymentGateway.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PaymentGateway.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]