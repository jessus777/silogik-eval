namespace SilogikEval.Application.Constants
{
    public static class ErrorKeys
    {
        // Contact - Validation
        public const string EmailRequired = "validation.email.required";
        public const string EmailInvalidFormat = "validation.email.invalid_format";
        public const string FirstNameRequired = "validation.firstname.required";
        public const string FirstNameAlphabetic = "validation.firstname.alphabetic_only";
        public const string LastNameRequired = "validation.lastname.required";
        public const string LastNameAlphabetic = "validation.lastname.alphabetic_only";
        public const string SecondNameAlphabetic = "validation.secondname.alphabetic_only";
        public const string SecondLastNameAlphabetic = "validation.secondlastname.alphabetic_only";
        public const string CommentsRequired = "validation.comments.required";
        public const string CommentsAlphabetic = "validation.comments.alphabetic_only";

        // Contact - Business rules
        public const string EmailAlreadyExists = "error.email.already_exists";

        // File
        public const string FileExtensionNotAllowed = "error.file.extension_not_allowed";
        public const string FileTypeNotAllowed = "error.file.type_not_allowed";
        public const string FileSizeExceeded = "error.file.size_exceeded";

        // Generic
        public const string EntityNotFound = "error.entity.not_found";
        public const string InvalidRequest = "error.invalid_request";
        public const string UnexpectedError = "error.unexpected";
    }
}
