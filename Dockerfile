# Etapa base para compilar y publicar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo .csproj y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia el resto del código fuente y publica la aplicación
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa base para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia los archivos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Exponer el puerto dinámico proporcionado por Render
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "UserApi.dll"]
