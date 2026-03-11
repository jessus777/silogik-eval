using FluentValidation;
using SilogikEval.Application.Constants;
using SilogikEval.Application.Dtos;
using SilogikEval.Application.Entities;
using SilogikEval.Application.Exceptions;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Responses;

namespace SilogikEval.Application.Services
{
    public class ContactServiceAsync
        : IContactServiceAsync
    {
        private readonly IContactRepositoryAsync _contactRepository;
        private readonly IValidator<CreateContactRequestDto> _createValidator;
        private readonly IValidator<UpdateContactRequestDto> _updateValidator;
        private readonly IFileValidator _fileValidator;
        private readonly IFileStorageService _fileStorageService;

        public ContactServiceAsync(
            IContactRepositoryAsync contactRepository,
            IValidator<CreateContactRequestDto> createValidator,
            IValidator<UpdateContactRequestDto> updateValidator,
            IFileValidator fileValidator,
            IFileStorageService fileStorageService)
        {
            _contactRepository = contactRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _fileValidator = fileValidator;
            _fileStorageService = fileStorageService;
        }

        public async Task<Guid> CreateAsync(CreateContactRequestDto request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);

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

        public async Task UpdateAsync(UpdateContactRequestDto request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);

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

            var existing = await _contactRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Contact), request.Id);

            existing.FirstName = request.FirstName;
            existing.SecondName = request.SecondName;
            existing.LastName = request.LastName;
            existing.SecondLastName = request.SecondLastName;
            existing.Comments = request.Comments;
            existing.LastModifiedDate = DateTime.UtcNow;

            if (request.FileStream is not null
                && request.FileName is not null
                && request.ContentType is not null)
            {
                _fileValidator.Validate(request.FileName, request.ContentType, request.FileSize ?? 0);

                if (!string.IsNullOrEmpty(existing.FilePath))
                    await _fileStorageService.DeleteAsync(existing.FilePath);

                existing.FilePath = await _fileStorageService.SaveAsync(request.FileStream, request.FileName);
            }

            await _contactRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _contactRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Contact), id);

            if (!string.IsNullOrEmpty(existing.FilePath))
                await _fileStorageService.DeleteAsync(existing.FilePath);

            await _contactRepository.DeleteAsync(id);
        }

        public async Task<ContactResponseDto?> GetByIdAsync(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            if (contact is null)
                return null;

            return MapToDto(contact);
        }

        public async Task<PagedResult<ContactResponseDto>> GetAllAsync(int pageNumber, int pageSize, string? search = null)
        {
            var pagedContacts = await _contactRepository.GetAllAsync(pageNumber, pageSize, search);

            var items = pagedContacts.Items.Select(MapToDto);

            return PagedResult<ContactResponseDto>.Create(items, pagedContacts.PageNumber, pagedContacts.PageSize, pagedContacts.TotalCount);
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
