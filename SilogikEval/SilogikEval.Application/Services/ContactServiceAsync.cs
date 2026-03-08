using FluentValidation;
using SilogikEval.Application.Constants;
using SilogikEval.Application.Dtos;
using SilogikEval.Application.Entities;
using SilogikEval.Application.Exceptions;
using SilogikEval.Application.Interfaces;

namespace SilogikEval.Application.Services
{
    public class ContactServiceAsync
        : IContactServiceAsync
    {
        private readonly IContactRepositoryAsync _contactRepository;
        private readonly IValidator<CreateContactRequestDto> _validator;
        private readonly IFileValidator _fileValidator;
        private readonly IFileStorageService _fileStorageService;

        public ContactServiceAsync(
            IContactRepositoryAsync contactRepository,
            IValidator<CreateContactRequestDto> validator,
            IFileValidator fileValidator,
            IFileStorageService fileStorageService)
        {
            _contactRepository = contactRepository;
            _validator = validator;
            _fileValidator = fileValidator;
            _fileStorageService = fileStorageService;
        }

        public async Task<Guid> CreateAsync(CreateContactRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new ValidationError
                {
                    PropertyName = e.PropertyName,
                    ErrorCode = e.ErrorCode,
                    ErrorMessage = e.ErrorMessage
                });

                throw new AppValidationException(errors);
            }

            if (await _contactRepository.EmailExistsAsync(request.Email))
                throw new BusinessException(ErrorKeys.EmailAlreadyExists, "El email ya se encuentra registrado.");

            string? filePath = null;

            if (request.FileStream is not null
                && request.FileName is not null
                && request.ContentType is not null)
            {
                _fileValidator.Validate(request.FileName, request.ContentType, request.FileSize ?? 0);
                filePath = await _fileStorageService.SaveAsync(request.FileStream, request.FileName);
            }

            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                LastName = request.LastName,
                SecondLastName = request.SecondLastName,
                Comments = request.Comments,
                FilePath = filePath,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            return await _contactRepository.CreateAsync(contact);
        }

        public async Task<ContactResponseDto?> GetByIdAsync(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            if (contact is null)
                return null;

            return MapToDto(contact);
        }

        public async Task<IEnumerable<ContactResponseDto>> GetAllAsync()
        {
            var contacts = await _contactRepository.GetAllAsync();

            return contacts.Select(MapToDto);
        }

        private static ContactResponseDto MapToDto(Contact contact)
        {
            return new ContactResponseDto
            {
                Id = contact.Id,
                Email = contact.Email,
                FirstName = contact.FirstName,
                SecondName = contact.SecondName,
                LastName = contact.LastName,
                SecondLastName = contact.SecondLastName,
                Comments = contact.Comments,
                FilePath = contact.FilePath,
                CreatedDate = contact.CreatedDate
            };
        }
    }
}
