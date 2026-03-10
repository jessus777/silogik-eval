# SilogikEval.Web.Client

Proyecto Blazor WebAssembly que contiene toda la interfaz de usuario. Se ejecuta en el navegador del cliente.

## Responsabilidad

Renderizar la interfaz, capturar las interacciones del usuario, consumir la API REST y gestionar el estado de la aplicación del lado del cliente.

## Estructura

```
SilogikEval.Web.Client/
├── Components/
│   ├── ContactList.razor              # Tabla de contactos con paginación y búsqueda
│   ├── ContactFormModal.razor         # Modal para crear un contacto
│   ├── ContactEditModal.razor         # Modal para editar un contacto
│   ├── ContactDetailModal.razor       # Modal para ver detalle de un contacto
│   └── LanguageSelector.razor         # Selector de idioma (es/en)
├── Extensions/
│   └── ServiceExtensionClient.cs      # Registro de servicios del cliente
├── Models/
│   ├── ContactModel.cs               # Modelo de contacto
│   ├── CreateContactModel.cs         # Modelo para crear contacto
│   ├── UpdateContactModel.cs         # Modelo para actualizar contacto
│   ├── PagedResultModel.cs           # Modelo de respuesta paginada
│   ├── ApiResponseModel.cs           # Modelo de respuesta de la API
│   └── LanguageModel.cs              # Modelo de idioma
├── Pages/
│   └── Contact.razor                  # Página principal de contactos
├── Services/
│   ├── IApiContactService.cs          # Interfaz del servicio de contactos
│   ├── ApiContactService.cs           # Consumo de API de contactos
│   ├── IApiTranslationService.cs      # Interfaz del servicio de traducciones
│   ├── ApiTranslationService.cs       # Consumo de API de traducciones
│   ├── ILanguageStateService.cs       # Interfaz del estado de idioma
│   ├── LanguageStateService.cs        # Gestión del idioma seleccionado
│   └── SweetAlertService.cs           # Interop con SweetAlert2
├── Program.cs                         # Punto de entrada WASM
└── _Imports.razor                     # Usings globales
```

## Qué hace cada parte

### Components

Componentes reutilizables de Blazor. `ContactList` incluye la tabla con paginación Bootstrap (primera, anterior, numéricas, siguiente, última) y un buscador por email o nombre.

Los modales (`ContactFormModal`, `ContactEditModal`, `ContactDetailModal`) se comunican con la página padre a través de `EventCallback`.

### Services

Servicios que consumen la API REST usando `HttpClient`. Manejan errores de conexión y respuestas no exitosas sin lanzar excepciones, devolviendo modelos vacíos o con el mensaje de error.

`LanguageStateService` mantiene el idioma seleccionado y notifica a los componentes cuando cambia para que se re-renderizen con las traducciones actualizadas.

### Models

Modelos específicos del cliente que se usan para serializar/deserializar las respuestas de la API. Son independientes de los DTOs del backend.

## Dependencias

- `Microsoft.AspNetCore.Components.WebAssembly` — runtime de Blazor WASM
- **No referencia a Application ni Persistence** — solo se comunica con el backend por HTTP
