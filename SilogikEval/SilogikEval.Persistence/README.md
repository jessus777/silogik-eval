# SilogikEval.Persistence

Capa de acceso a datos. Implementa las interfaces definidas en Application y se comunica con SQL Server usando Dapper.

## Responsabilidad

Ejecutar las consultas a la base de datos, gestionar el almacenamiento de archivos en disco y proveer las implementaciones concretas de los repositorios. **No contiene lógica de negocio.**

## Estructura

```
SilogikEval.Persistence/
├── Context/
│   ├── IDbConnectionFactory.cs       # Interfaz para crear conexiones
│   └── SqlConnectionFactory.cs       # Implementación con SqlConnection
├── Repositories/
│   ├── ContactRepositoryAsync.cs     # Acceso a datos de contactos
│   └── TranslationRepositoryAsync.cs # Acceso a datos de traducciones
├── Scripts/
│   └── DatabaseSetup.sql             # Tablas, SPs y seed data
├── Services/
│   ├── LocalFileStorageService.cs    # Almacenamiento de archivos en disco
│   └── FileValidator.cs             # Validación de tipo y tamaño de archivo
└── ServiceExtensionPersistence.cs    # Registro de servicios en DI
```

## Qué hace cada parte

### Repositories

Implementan las interfaces de `IContactRepositoryAsync` e `ITranslationRepositoryAsync`. Usan Dapper para ejecutar stored procedures y mapear los resultados a entidades.

- `ContactRepositoryAsync` soporta paginación del lado del servidor usando `QueryMultipleAsync` para leer el total de registros y los datos en una sola llamada.

### Context

`SqlConnectionFactory` crea instancias de `SqlConnection` a partir del connection string. Los repositorios la usan en lugar de crear conexiones directamente, facilitando el testing.

### Services

- `LocalFileStorageService` guarda archivos en una carpeta del disco (`C:\uploads` por defecto).
- `FileValidator` valida la extensión, el content type y el tamaño del archivo antes de guardarlo.

### Scripts

`DatabaseSetup.sql` contiene todo lo necesario para crear la base de datos desde cero:

- Tablas: `Contacts`, `Languages`, `Translations`
- Stored Procedures: `usp_Contact_Insert`, `usp_Contact_GetAll` (con paginación), `usp_Contact_GetById`, `usp_Contact_Update`, `usp_Contact_EmailExists`, `usp_Translation_GetByLanguage`, `usp_Language_GetActive`
- Seed data: 2 idiomas (es, en) y traducciones para labels, botones, mensajes, validaciones y errores

## Dependencias

- `SilogikEval.Application` — para implementar las interfaces
- `Dapper` — micro ORM para ejecutar stored procedures
- `Microsoft.Data.SqlClient` — driver de SQL Server
