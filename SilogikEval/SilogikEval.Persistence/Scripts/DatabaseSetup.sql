-- =============================================
-- SilogikEval - Database Setup Script
-- SQL Server | Dapper + Stored Procedures
-- =============================================

-- =============================================
-- 1. TABLES
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contacts')
BEGIN
    CREATE TABLE Contacts
    (
        Id                  UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY,
        Email               NVARCHAR(256)       NOT NULL,
        FirstName           NVARCHAR(100)       NOT NULL,
        SecondName          NVARCHAR(100)       NULL,
        LastName            NVARCHAR(100)       NOT NULL,
        SecondLastName      NVARCHAR(100)       NULL,
        Comments            NVARCHAR(MAX)       NOT NULL,
        FilePath            NVARCHAR(500)       NULL,
        CreatedDate         DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate    DATETIME2           NOT NULL DEFAULT GETUTCDATE()
    );

    CREATE UNIQUE INDEX UQ_Contacts_Email ON Contacts (Email);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Languages')
BEGIN
    CREATE TABLE Languages
    (
        Id                  INT IDENTITY(1,1)   NOT NULL PRIMARY KEY,
        Code                NVARCHAR(10)        NOT NULL,
        Name                NVARCHAR(100)       NOT NULL,
        IsActive            BIT                 NOT NULL DEFAULT 1,
        CreatedDate         DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT UQ_Languages_Code UNIQUE (Code)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Translations')
BEGIN
    CREATE TABLE Translations
    (
        Id                  INT IDENTITY(1,1)   NOT NULL PRIMARY KEY,
        LanguageCode        NVARCHAR(10)        NOT NULL,
        [Key]               NVARCHAR(256)       NOT NULL,
        Value               NVARCHAR(MAX)       NOT NULL,
        CreatedDate         DATETIME2           NOT NULL DEFAULT GETUTCDATE(),
        LastModifiedDate    DATETIME2           NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT FK_Translations_Languages
            FOREIGN KEY (LanguageCode) REFERENCES Languages(Code),

        CONSTRAINT UQ_Translations_Lang_Key
            UNIQUE (LanguageCode, [Key])
    );
END
GO

-- =============================================
-- 2. STORED PROCEDURES - Contacts
-- =============================================

CREATE OR ALTER PROCEDURE usp_Contact_Insert
    @Id                 UNIQUEIDENTIFIER,
    @Email              NVARCHAR(256),
    @FirstName          NVARCHAR(100),
    @SecondName         NVARCHAR(100)       = NULL,
    @LastName           NVARCHAR(100),
    @SecondLastName     NVARCHAR(100)       = NULL,
    @Comments           NVARCHAR(MAX),
    @FilePath           NVARCHAR(500)       = NULL,
    @CreatedDate        DATETIME2,
    @LastModifiedDate   DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Contacts
        (Id, Email, FirstName, SecondName, LastName, SecondLastName,
         Comments, FilePath, CreatedDate, LastModifiedDate)
    VALUES
        (@Id, @Email, @FirstName, @SecondName, @LastName, @SecondLastName,
         @Comments, @FilePath, @CreatedDate, @LastModifiedDate);

    SELECT @Id;
END
GO

CREATE OR ALTER PROCEDURE usp_Contact_GetById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  Id, Email, FirstName, SecondName, LastName, SecondLastName,
            Comments, FilePath, CreatedDate, LastModifiedDate
    FROM    Contacts
    WHERE   Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE usp_Contact_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  Id, Email, FirstName, SecondName, LastName, SecondLastName,
            Comments, FilePath, CreatedDate, LastModifiedDate
    FROM    Contacts
    ORDER BY CreatedDate DESC;
END
GO

CREATE OR ALTER PROCEDURE usp_Contact_EmailExists
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(1)
    FROM   Contacts
    WHERE  Email = @Email;
END
GO

-- =============================================
-- 3. STORED PROCEDURES - Translations
-- =============================================

CREATE OR ALTER PROCEDURE usp_Translation_GetByLanguage
    @LanguageCode NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  Id, LanguageCode, [Key], Value, CreatedDate, LastModifiedDate
    FROM    Translations
    WHERE   LanguageCode = @LanguageCode;
END
GO

CREATE OR ALTER PROCEDURE usp_Language_GetActive
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  Id, Code, Name, IsActive, CreatedDate, LastModifiedDate
    FROM    Languages
    WHERE   IsActive = 1
    ORDER BY Name;
END
GO

-- =============================================
-- 4. SEED DATA - Languages
-- =============================================

IF NOT EXISTS (SELECT 1 FROM Languages WHERE Code = 'es')
    INSERT INTO Languages (Code, Name, IsActive) VALUES ('es', N'Español', 1);

IF NOT EXISTS (SELECT 1 FROM Languages WHERE Code = 'en')
    INSERT INTO Languages (Code, Name, IsActive) VALUES ('en', N'English', 1);
GO

-- =============================================
-- 5. SEED DATA - Translations (es)
-- =============================================

-- Labels
INSERT INTO Translations (LanguageCode, [Key], Value) VALUES
('es', 'label.email',              N'Correo electrónico'),
('es', 'label.firstname',          N'Nombre'),
('es', 'label.secondname',         N'Segundo nombre'),
('es', 'label.lastname',           N'Apellido paterno'),
('es', 'label.secondlastname',     N'Apellido materno'),
('es', 'label.comments',           N'Comentarios'),
('es', 'label.attachment',         N'Adjunto'),
('es', 'label.contacts',           N'Contactos'),
('es', 'label.no_records',         N'No hay registros.'),
('es', 'label.add_contact',        N'Agregar Contacto'),
('es', 'label.actions',            N'Acciones'),
('es', 'label.contact_detail',     N'Detalle del Contacto'),
('es', 'label.created_date',       N'Fecha de creación'),

-- Buttons
('es', 'button.submit',            N'Enviar'),
('es', 'button.cancel',            N'Cancelar'),
('es', 'button.confirm',           N'Confirmar'),
('es', 'button.view',              N'Ver'),
('es', 'button.close',             N'Cerrar'),

-- Messages
('es', 'message.success',          N'El formulario se envió correctamente.'),
('es', 'message.error',            N'Ocurrió un error al procesar la solicitud.'),
('es', 'message.confirm_send',     N'¿Está seguro de enviar el formulario?'),

-- Validation
('es', 'validation.email.required',                 N'El correo electrónico es obligatorio.'),
('es', 'validation.email.invalid_format',            N'El formato del correo electrónico no es válido.'),
('es', 'validation.firstname.required',              N'El nombre es obligatorio.'),
('es', 'validation.firstname.alphabetic_only',       N'El nombre solo permite caracteres alfabéticos.'),
('es', 'validation.lastname.required',               N'El apellido paterno es obligatorio.'),
('es', 'validation.lastname.alphabetic_only',        N'El apellido paterno solo permite caracteres alfabéticos.'),
('es', 'validation.secondname.alphabetic_only',      N'El segundo nombre solo permite caracteres alfabéticos.'),
('es', 'validation.secondlastname.alphabetic_only',  N'El apellido materno solo permite caracteres alfabéticos.'),
('es', 'validation.comments.required',               N'Los comentarios son obligatorios.'),
('es', 'validation.comments.alphabetic_only',        N'Los comentarios solo permiten caracteres alfabéticos.'),

-- Business errors
('es', 'error.email.already_exists',        N'El email ya se encuentra registrado.'),
('es', 'error.file.extension_not_allowed',  N'Extensión de archivo no permitida.'),
('es', 'error.file.type_not_allowed',       N'Tipo de archivo no permitido.'),
('es', 'error.file.size_exceeded',          N'El archivo excede el tamaño permitido.'),
('es', 'error.entity.not_found',            N'El recurso solicitado no fue encontrado.'),
('es', 'error.invalid_request',             N'La solicitud no es válida.'),
('es', 'error.unexpected',                  N'Ocurrió un error inesperado.');
GO

-- =============================================
-- 6. SEED DATA - Translations (en)
-- =============================================

INSERT INTO Translations (LanguageCode, [Key], Value) VALUES
-- Labels
('en', 'label.email',              N'Email'),
('en', 'label.firstname',          N'First name'),
('en', 'label.secondname',         N'Middle name'),
('en', 'label.lastname',           N'Last name'),
('en', 'label.secondlastname',     N'Second last name'),
('en', 'label.comments',           N'Comments'),
('en', 'label.attachment',         N'Attachment'),
('en', 'label.contacts',           N'Contacts'),
('en', 'label.no_records',         N'No records found.'),
('en', 'label.add_contact',        N'Add Contact'),
('en', 'label.actions',            N'Actions'),
('en', 'label.contact_detail',     N'Contact Detail'),
('en', 'label.created_date',       N'Created date'),

-- Buttons
('en', 'button.submit',            N'Submit'),
('en', 'button.cancel',            N'Cancel'),
('en', 'button.confirm',           N'Confirm'),
('en', 'button.view',              N'View'),
('en', 'button.close',             N'Close'),

-- Messages
('en', 'message.success',          N'The form was submitted successfully.'),
('en', 'message.error',            N'An error occurred while processing the request.'),
('en', 'message.confirm_send',     N'Are you sure you want to submit the form?'),

-- Validation
('en', 'validation.email.required',                 N'Email is required.'),
('en', 'validation.email.invalid_format',            N'Email format is invalid.'),
('en', 'validation.firstname.required',              N'First name is required.'),
('en', 'validation.firstname.alphabetic_only',       N'First name only allows alphabetic characters.'),
('en', 'validation.lastname.required',               N'Last name is required.'),
('en', 'validation.lastname.alphabetic_only',        N'Last name only allows alphabetic characters.'),
('en', 'validation.secondname.alphabetic_only',      N'Middle name only allows alphabetic characters.'),
('en', 'validation.secondlastname.alphabetic_only',  N'Second last name only allows alphabetic characters.'),
('en', 'validation.comments.required',               N'Comments are required.'),
('en', 'validation.comments.alphabetic_only',        N'Comments only allow alphabetic characters.'),

-- Business errors
('en', 'error.email.already_exists',        N'Email is already registered.'),
('en', 'error.file.extension_not_allowed',  N'File extension is not allowed.'),
('en', 'error.file.type_not_allowed',       N'File type is not allowed.'),
('en', 'error.file.size_exceeded',          N'File exceeds the maximum allowed size.'),
('en', 'error.entity.not_found',            N'The requested resource was not found.'),
('en', 'error.invalid_request',             N'The request is invalid.'),
('en', 'error.unexpected',                  N'An unexpected error occurred.');
GO
