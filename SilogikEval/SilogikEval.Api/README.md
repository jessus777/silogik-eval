# SilogikEval.Api

Capa de presentación HTTP. Expone la funcionalidad del sistema a través de Minimal API endpoints.

## Responsabilidad

Recibir peticiones HTTP, delegar al servicio correspondiente y devolver respuestas con el formato adecuado. **No contiene lógica de negocio.**

## Estructura

```
SilogikEval.Api/
├── Endpoints/
│   ├── ContactEndpoints.cs          # CRUD de contactos (paginado, búsqueda)
│   └── TranslationEndpoints.cs      # Traducciones e idiomas
├── Extensions/
│   ├── CorsExtensions.cs            # Configuración de CORS
│   ├── EndpointExtensions.cs        # Registro centralizado de endpoints
│   └── MiddlewareExtensions.cs      # Registro de middlewares
├── Middleware/
│   └── ErrorLocalizationMiddleware.cs  # Captura excepciones y traduce mensajes
├── Models/
│   ├── CreateContactRequest.cs      # Modelo de entrada para crear contacto
│   └── UpdateContactRequest.cs      # Modelo de entrada para actualizar contacto
└── Program.cs                       # Punto de entrada y pipeline
```

## Qué hace cada parte

### Endpoints

Definen las rutas HTTP y reciben los parámetros de entrada. Transforman los modelos de la API a DTOs de la capa Application y devuelven la respuesta envuelta en `ApiResponse<T>`.

- Los endpoints de contactos aceptan `multipart/form-data` para soportar carga de archivos.
- El listado soporta paginación (`page`, `pageSize`) y búsqueda (`search`) por query string.

### Middleware

El `ErrorLocalizationMiddleware` intercepta las excepciones que lanza la capa de negocio (`AppValidationException`, `BusinessException`, `NotFoundException`) y las convierte en respuestas HTTP con el status code y mensaje traducido al idioma del header `Accept-Language`.

### Models

Son modelos específicos de la API para recibir los datos del request. Existen separados de los DTOs de Application para que la capa de negocio no dependa del framework HTTP.

## Dependencias

- `SilogikEval.Application` — para acceder a servicios e interfaces
- `SilogikEval.Persistence` — para registrar los servicios de infraestructura
- `Scalar.AspNetCore` — documentación interactiva de la API
