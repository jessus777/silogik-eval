# SilogikEval

Sistema de gestión de contactos construido con .NET 10, Blazor WebAssembly y Minimal API.

## Tecnologías

| Capa | Tecnología |
|------|-----------|
| Frontend | Blazor WebAssembly, Bootstrap 5, Bootstrap Icons |
| Backend | .NET 10 Minimal API |
| Validación | FluentValidation |
| Base de datos | SQL Server (LocalDB), Dapper, Stored Procedures |
| Documentación API | OpenAPI + Scalar |

## Arquitectura

El proyecto sigue una arquitectura en N-capas con separación clara de responsabilidades:

```
┌──────────────────────────────────────────┐
│   SilogikEval.Web / Web.Client           │  UI - Blazor WebAssembly
├──────────────────────────────────────────┤
│   SilogikEval.Api                        │  Endpoints - Minimal API
├──────────────────────────────────────────┤
│   SilogikEval.Application                │  Lógica de negocio
├──────────────────────────────────────────┤
│   SilogikEval.Persistence                │  Acceso a datos
├──────────────────────────────────────────┤
│   SQL Server (LocalDB)                   │  Base de datos
└──────────────────────────────────────────┘
```

Cada capa tiene su propio `README.md` con los detalles de lo que contiene:

- [`SilogikEval.Api`](SilogikEval/SilogikEval.Api/README.md)
- [`SilogikEval.Application`](SilogikEval/SilogikEval.Application/README.md)
- [`SilogikEval.Persistence`](SilogikEval/SilogikEval.Persistence/README.md)
- [`SilogikEval.Web`](SilogikEval/SilogikEval.Web/SilogikEval.Web/README.md)
- [`SilogikEval.Web.Client`](SilogikEval/SilogikEval.Web/SilogikEval.Web.Client/README.md)

## Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (incluido con Visual Studio)
- Visual Studio 2022+ o VS Code
- Crear la carpeta `C:\uploads` (almacenamiento de archivos adjuntos)

## Configuración de la base de datos

### Opción A: SQL Server Management Studio (SSMS)

1. Conectarse a `(localdb)\MSSQLLocalDB`
2. Crear una base de datos con el nombre `SilogikEval`
3. Abrir el archivo `SilogikEval\SilogikEval.Persistence\Scripts\DatabaseSetup.sql`
4. Asegurarse de que la base de datos `SilogikEval` esté seleccionada en el dropdown
5. Ejecutar el script (`F5`)

### Opción B: Terminal con sqlcmd

1. Crear la base de datos:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "CREATE DATABASE SilogikEval;"
```

2. Ejecutar el script de setup (usar `-f 65001` para que los caracteres especiales se inserten correctamente):

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -d SilogikEval -f 65001 -i "SilogikEval\SilogikEval.Persistence\Scripts\DatabaseSetup.sql"
```

Ambas opciones crean las tablas, stored procedures y datos iniciales (idiomas + traducciones en español e inglés).

## Cómo ejecutar

El proyecto necesita **dos aplicaciones corriendo al mismo tiempo**: la API y el frontend.

### Opción 1: Visual Studio (recomendada)

1. Abrir `SilogikEval.sln`
2. Click derecho en la solución → **Configure Startup Projects**
3. Seleccionar **Multiple startup projects**
4. Poner `SilogikEval.Api` y `SilogikEval.Web` en **Start**
5. Presionar `F5`

### Opción 2: Terminal

```powershell
# Terminal 1 - API
cd SilogikEval/SilogikEval.Api
dotnet run --launch-profile https

# Terminal 2 - Web
cd SilogikEval/SilogikEval.Web/SilogikEval.Web
dotnet run --launch-profile https
```

### URLs

| Aplicación | URL |
|-----------|-----|
| Frontend (Blazor) | https://localhost:7063 |
| API | https://localhost:7103 |
| Documentación API (Scalar) | https://localhost:7103/scalar/v1 |

## Estructura de la solución

```
SilogikEval/
├── SilogikEval.Api/                  # Minimal API - Endpoints y middleware
├── SilogikEval.Application/          # Lógica de negocio, DTOs, validaciones
├── SilogikEval.Persistence/          # Repositorios, Dapper, scripts SQL
└── SilogikEval.Web/
    ├── SilogikEval.Web/              # Host del servidor Blazor
    └── SilogikEval.Web.Client/       # Componentes Blazor WebAssembly
```

## Características

- **CRUD de contactos** con formularios validados
- **Paginación del lado del servidor** con búsqueda por email o nombre
- **Carga de archivos** adjuntos con validación de tipo y tamaño
- **Internacionalización** (Español / Inglés) con traducciones almacenadas en BD
- **Localización de errores** — las validaciones y mensajes de error se traducen al idioma seleccionado
- **Documentación interactiva** de la API con Scalar

## Endpoints de la API

### Contactos

| Método | Ruta | Descripción |
|--------|------|-------------|
| `POST` | `/api/contacts` | Crear un contacto |
| `GET` | `/api/contacts?page=1&pageSize=10&search=` | Listar contactos (paginado) |
| `GET` | `/api/contacts/{id}` | Obtener un contacto por ID |
| `PUT` | `/api/contacts/{id}` | Actualizar un contacto |
| `DELETE` | `/api/contacts/{id}` | Eliminar un contacto |

### Traducciones

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/translations/{languageCode}` | Obtener traducciones por idioma |
| `GET` | `/api/translations/languages` | Obtener idiomas activos |

## Sobre el proyecto

Este proyecto fue desarrollado como parte de una evaluación técnica. El objetivo fue construir un sistema de gestión de contactos que demuestre buenas prácticas de desarrollo en .NET.

Requerimiento: 
    Como usuario, necesito una aplicación que me permita enviar un formulario de contacto, con las siguientes características. 
        • La información debe almacenarse en base de datos para su consulta y utilización futura.  
        • Los datos para ingresar serán: email, nombres, apellidos, comentarios y permitir adjuntar una imagen o pdf. 
        • Se debe validar que los comentarios, nombres y apellidos sólo permitan caracteres alfabéticos del español, que el email tenga formato válido y que el tipo de archivo seleccionado sea una imagen o un pdf.  
        • Todos los campos son obligatorios excepto el campo de adjunto.  
        • Pedir confirmación antes de enviar el formulario.  
        • Mostrar un mensaje de envío correcto del formulario. 
        • El formulario debe ser multi-idioma: 
            o Debo poder seleccionar el idioma en que se muestran las etiquetas de los campos, botones, mensajes, etc. 
            o Los textos de cada idioma deberán configurarse y obtenerse de la base de datos. 
            o Esta funcionalidad se debe desarrollar sin utilizar librerías externas y sin copiar código de terceros. 
              El objetivo es conocer la capacidad de solución, el estilo de programación y uso de buenas prácticas. 

### Decisiones técnicas

- **Arquitectura N-capas** para mantener cada responsabilidad aislada. La capa de negocio no conoce los detalles de la base de datos ni del framework web.
- **Interfaces en todas las capas** para facilitar el testing y respetar el principio de inversión de dependencias.
- **Minimal API** en lugar de controllers porque el alcance del proyecto no requiere la complejidad adicional que traen los controllers.
- **Dapper + Stored Procedures** como estrategia de acceso a datos, priorizando el control sobre las consultas SQL y el rendimiento.
- **FluentValidation** para centralizar las reglas de validación fuera de los modelos.
- **Traducciones desde BD** en lugar de archivos de recursos, permitiendo agregar idiomas sin recompilar.
- **Middleware de localización de errores** que intercepta las excepciones del negocio y traduce los mensajes antes de devolverlos al cliente.

### Principios aplicados

- **SOLID** — Cada clase tiene una responsabilidad, las dependencias se inyectan por interfaz y las capas se comunican a través de abstracciones.
- **Clean Code** — Nombres descriptivos, métodos cortos, sin comentarios innecesarios. El código se explica solo.
- **Separation of Concerns** — La UI no conoce SQL, la API no conoce reglas de negocio, el negocio no conoce el framework HTTP.
