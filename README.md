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

## Probar el Programa Desplegado

El programa ya está desplegado en Render y puedes probarlo utilizando **Postman** o cualquier cliente HTTP. La URL base para los endpoints de la API es:
```
https://ads-tarea4.onrender.com
```

### Endpoints Disponibles

1. **Autenticación:**
   - **Endpoint:** `POST /api/auth/login`
   - **Descripción:** Genera un token JWT para autenticarse.
   - **Cuerpo de Ejemplo (JSON):**
     ```json
     {
       "email": "admin@example.com",
       "password": "AdminPassword123"
     }
     ```
   - **Encabezado de Respuesta:**
     - `Authorization: Bearer <TOKEN>`
   - Guarda este token para usarlo en los endpoints protegidos.

2. **CRUD de Usuarios:**
   - **Crear Usuario:**
     - **Endpoint:** `POST /api/users`
     - **Cuerpo de Ejemplo (JSON):**
       ```json
       {
         "firstName": "John",
         "lastName": "Doe",
         "email": "john.doe@example.com",
         "password": "password123"
       }
       ```
   - **Obtener Usuarios Paginados:**
     - **Endpoint:** `GET /api/users?page=1&limit=10`
   - **Obtener Usuario por ID:**
     - **Endpoint:** `GET /api/users/{id}`
   - **Actualizar Usuario:**
     - **Endpoint:** `PATCH /api/users/{id}`
     - **Cuerpo de Ejemplo (JSON):**
       ```json
       {
         "firstName": "Jane",
         "lastName": "Smith"
       }
       ```
   - **Eliminar Usuario (Lógica):**
     - **Endpoint:** `DELETE /api/users/{id}`

### Probar Autenticación y Seguridad

1. **Autentícate con el endpoint `POST /api/auth/login`** usando las credenciales:
   - **Email:** `admin@example.com`
   - **Contraseña:** `AdminPassword123`

2. **Incluye el Token JWT en las Solicitudes Protegidas:**
   - Después de autenticarse, usa el token recibido en el encabezado de las solicitudes a los endpoints protegidos:
     ```
     Authorization: Bearer <TOKEN>
     ```

