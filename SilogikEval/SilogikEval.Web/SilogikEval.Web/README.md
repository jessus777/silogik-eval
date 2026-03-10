# SilogikEval.Web

Proyecto servidor que hospeda la aplicación Blazor WebAssembly.

## Responsabilidad

Servir los archivos estáticos de Blazor WebAssembly, manejar el prerendering del lado del servidor y configurar el layout general de la aplicación.

## Estructura

```
SilogikEval.Web/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor          # Layout principal (navbar, contenido)
│   │   └── NavMenu.razor             # Menú de navegación
│   ├── Pages/
│   │   ├── Home.razor                # Página de inicio
│   │   ├── Error.razor               # Página de error
│   │   └── NotFound.razor            # Página 404
│   ├── App.razor                     # Componente raíz de la aplicación
│   ├── Routes.razor                  # Configuración de rutas
│   └── _Imports.razor                # Usings globales
├── Properties/
│   └── launchSettings.json           # Perfil de ejecución (puerto 7063)
├── Program.cs                        # Punto de entrada y pipeline
└── appsettings.json                  # Configuración (URL de la API)
```

## Configuración

En `appsettings.json` se define la URL base de la API que consumirá el cliente:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7103"
  }
}
```

## Dependencias

- `SilogikEval.Web.Client` — componentes Blazor WebAssembly
- `Microsoft.AspNetCore.Components.WebAssembly.Server` — hosting de WASM
