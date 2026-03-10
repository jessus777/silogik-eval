using FluentValidation;
using SilogikEval.Application.Constants;
using SilogikEval.Application.Dtos;

namespace SilogikEval.Application.Validators
{
    public class UpdateContactValidator
        : AbstractValidator<UpdateContactRequestDto>
    {
        public UpdateContactValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithErrorCode(ErrorKeys.FirstNameRequired)
                .WithMessage("El nombre es obligatorio.")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithErrorCode(ErrorKeys.FirstNameAlphabetic)
                .WithMessage("El nombre solo permite caracteres alfabéticos.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithErrorCode(ErrorKeys.LastNameRequired)
                .WithMessage("El apellido paterno es obligatorio.")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithErrorCode(ErrorKeys.LastNameAlphabetic)
                .WithMessage("El apellido paterno solo permite caracteres alfabéticos.");

            RuleFor(x => x.SecondName)
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$")
                .When(x => !string.IsNullOrEmpty(x.SecondName))
                .WithErrorCode(ErrorKeys.SecondNameAlphabetic)
                .WithMessage("El segundo nombre solo permite caracteres alfabéticos.");

            RuleFor(x => x.SecondLastName)
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .When(x => !string.IsNullOrEmpty(x.SecondLastName))
                .WithErrorCode(ErrorKeys.SecondLastNameAlphabetic)
                .WithMessage("El apellido materno solo permite caracteres alfabéticos.");

            RuleFor(x => x.Comments)
                .NotEmpty()
                .WithErrorCode(ErrorKeys.CommentsRequired)
                .WithMessage("Los comentarios son obligatorios.")
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithErrorCode(ErrorKeys.CommentsAlphabetic)
                .WithMessage("Los comentarios solo permiten caracteres alfabéticos.");
        }
    }
}
