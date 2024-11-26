# User API

Una API para la gestión de usuarios con autenticación JWT y operaciones CRUD. Este proyecto está diseñado en .NET 8 y utiliza SQLite como base de datos.

## Requisitos previos

Asegúrate de tener instalados los siguientes componentes:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional, para correr en Render)
- [Render CLI](https://render.com/docs/deployments) (para el despliegue en Render)

## Configuración local

1. Clona el repositorio:
```
git clone https://github.com/DavidZeballos/AdS_Tarea4 
cd UserApi
```

2. Configura la base de datos:
Asegúrate de que la cadena de conexión esté configurada en `appsettings.json`:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=userapi.db"
  },
  "Jwt": {
    "Key": "SuperSecureKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "http://localhost:5001",
    "Audience": "http://localhost:5001"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```


3. Restaura las dependencias:
```
dotnet restore
```


4. Aplica las migraciones de la base de datos:
```
dotnet ef migrations add InitialCreate --output-dir src/Data/Migrations
dotnet ef database update
```


5. Ejecuta la aplicación:
```
dotnet run
```


6. Accede a la API:
- Swagger: `http://localhost:5001/swagger`

## Colección de Postman

Incluye una colección de Postman en el directorio raíz del proyecto (`UserApi.postman_collection.json`) para probar los endpoints de la API.

## Endpoints principales

- **Autenticación:**
- `POST /api/auth/login` - Genera un token JWT.

- **Usuarios:**
- `POST /api/users` - Crea un nuevo usuario.
- `GET /api/users` - Lista usuarios (paginados).
- `GET /api/users/{id}` - Obtiene un usuario por ID.
- `PATCH /api/users/{id}` - Actualiza uno o más campos de un usuario.
- `DELETE /api/users/{id}` - Marca un usuario como eliminado (eliminación lógica).

## Notas adicionales

- **Semilla inicial:** El proyecto incluye un administrador con las credenciales:
- Email: `admin@example.com`
- Contraseña: `AdminPassword123`
- **Clave JWT:** Revisa y actualiza la clave en `appsettings.json` si es necesario.

