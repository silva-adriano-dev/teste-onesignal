# 1. ESTÁGIO DE BUILD (SDK)
# Usamos o SDK do .NET 9 para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas o .csproj primeiro para aproveitar o cache das camadas do Docker
COPY ["TrackingRealtime.csproj", "./"]
RUN dotnet restore

# Agora copia o restante dos arquivos e compila
COPY . .
RUN dotnet build "TrackingRealtime.csproj" -c Release -o /app/build

# 2. ESTÁGIO DE PUBLISH
# Prepara os arquivos finais para execução
FROM build AS publish
RUN dotnet publish "TrackingRealtime.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 3. ESTÁGIO FINAL (RUNTIME)
# Usamos a imagem de runtime, que é muito menor que o SDK
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# No .NET 8/9, a porta padrão mudou de 80 para 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copia os arquivos compilados do estágio de publish
COPY --from=publish /app/publish .

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "TrackingRealtime.dll"]