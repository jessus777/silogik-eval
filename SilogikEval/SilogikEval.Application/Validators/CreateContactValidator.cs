using FluentValidation;
using SilogikEval.Application.Dtos;

namespace SilogikEval.Application.Validators
{
    public class CreateContactValidator
        : AbstractValidator<CreateContactRequestDto>
    {
        public CreateContactValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El nombre solo permite caracteres alfabéticos.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("El apellido paterno solo permite caracteres alfabéticos.");

            RuleFor(x => x.SecondName)
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$")
                .When(x => !string.IsNullOrEmpty(x.SecondName))
                .WithMessage("El segundo nombre solo permite caracteres alfabéticos.");

            RuleFor(x => x.SecondLastName)
                .NotEmpty()
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$")
                .When(x => !string.IsNullOrEmpty(x.SecondLastName))
                .WithMessage("El apellido materno solo permite caracteres alfabéticos.");

            RuleFor(x => x.Comments)
                .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$")
                .When(x => !string.IsNullOrEmpty(x.Comments))
                .WithMessage("Los comentarios solo permiten caracteres alfabéticos.");
        }

    }
}
