# SilogikEval.Application

Capa de lógica de negocio. Aquí viven las reglas, validaciones y la definición de cómo se comporta el sistema.

## Responsabilidad

Contener toda la lógica de negocio, definir las interfaces que las capas externas deben implementar, y exponer los servicios que la API consume. **No depende de ningún framework web ni de ninguna base de datos.**

## Estructura

```
SilogikEval.Application/
├── Commons/
│   └── BaseEntity.cs                 # Clase base con CreatedDate/LastModifiedDate
├── Constants/
│   └── ErrorKeys.cs                  # Claves de error para traducciones
├── Dtos/
│   ├── CreateContactRequestDto.cs    # DTO para creación de contacto
│   ├── UpdateContactRequestDto.cs    # DTO para actualización de contacto
│   ├── ContactResponseDto.cs         # DTO de respuesta de contacto
│   └── LanguageDto.cs                # DTO de idioma
├── Entities/
│   ├── Contact.cs                    # Entidad de contacto
│   ├── Language.cs                   # Entidad de idioma
│   └── Translation.cs               # Entidad de traducción
├── Exceptions/
│   ├── AppValidationException.cs     # Error de validación (400)
│   ├── BusinessException.cs          # Error de regla de negocio (409)
│   └── NotFoundException.cs          # Recurso no encontrado (404)
├── Interfaces/
│   ├── IContactRepositoryAsync.cs    # Contrato del repositorio de contactos
│   ├── IContactServiceAsync.cs       # Contrato del servicio de contactos
│   ├── ITranslationRepositoryAsync.cs
│   ├── ITranslationServiceAsync.cs
│   ├── IFileStorageService.cs        # Contrato para almacenamiento de archivos
│   └── IFileValidator.cs             # Contrato para validación de archivos
├── Responses/
│   ├── ApiResponse.cs                # Wrapper genérico de respuesta
│   └── PagedResult.cs                # Wrapper genérico de respuesta paginada
├── Services/
│   ├── ContactServiceAsync.cs        # Lógica de negocio de contactos
│   └── TranslationServiceAsync.cs    # Lógica de traducciones
├── Validators/
│   ├── CreateContactValidator.cs     # Reglas de validación para crear
│   └── UpdateContactValidator.cs     # Reglas de validación para actualizar
└── ServiceExtensionApplication.cs    # Registro de servicios en DI
```

## Qué hace cada parte

### Services

Contienen la lógica de negocio. `ContactServiceAsync` valida los datos de entrada, verifica reglas de negocio (como email duplicado), gestiona archivos adjuntos y delega la persistencia al repositorio.

### Interfaces

Definen los contratos que la capa de Persistence implementa. Gracias a esto, la lógica de negocio no sabe si los datos vienen de SQL Server, un API externo o un archivo. Solo conoce la interfaz.

### Validators

Usan FluentValidation para definir las reglas de validación. Los códigos de error (`ErrorKeys`) coinciden con las claves de traducción en la base de datos, permitiendo devolver mensajes localizados.

### Exceptions

Excepciones de dominio tipadas que el middleware de la API intercepta y convierte en respuestas HTTP con el status code correcto.

### DTOs

Objetos que viajan entre capas. Los DTOs de entrada reciben los datos desde la API, los DTOs de salida devuelven datos al cliente. Las entidades nunca se exponen directamente.

## Dependencias

- `FluentValidation` — validación de reglas de negocio
- **Ninguna dependencia a frameworks web o base de datos**
