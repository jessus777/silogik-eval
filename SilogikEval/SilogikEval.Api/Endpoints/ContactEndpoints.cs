using SilogikEval.Api.Models;
using SilogikEval.Application.Dtos;
using SilogikEval.Application.Interfaces;
using SilogikEval.Application.Responses;

namespace SilogikEval.Api.Endpoints
{
    public static class ContactEndpoints
    {
        public static RouteGroupBuilder MapContactEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/contacts")
                .WithTags("Contacts");

            group.MapPost("/", CreateAsync)
                .DisableAntiforgery()
                .Accepts<CreateContactRequest>("multipart/form-data")
                .Produces<ApiResponse<Guid>>(StatusCodes.Status201Created)
                .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
                .Produces<ApiResponse<object>>(StatusCodes.Status409Conflict);

            group.MapGet("/", GetAllAsync)
                .Produces<ApiResponse<IEnumerable<ContactResponseDto>>>();

            group.MapGet("/{id:guid}", GetByIdAsync)
                .Produces<ApiResponse<ContactResponseDto>>()
                .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

            return group;
        }

        private static async Task<IResult> CreateAsync(
            [AsParameters] CreateContactRequest request,
            IContactServiceAsync contactService)
        {
            var dto = new CreateContactRequestDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                LastName = request.LastName,
                SecondLastName = request.SecondLastName,
                Comments = request.Comments
            };

            if (request.Attachment is not null)
            {
                dto.FileName = request.Attachment.FileName;
                dto.ContentType = request.Attachment.ContentType;
                dto.FileSize = request.Attachment.Length;
                dto.FileStream = request.Attachment.OpenReadStream();
            }

            var id = await contactService.CreateAsync(dto);

            var response = ApiResponse<Guid>.Ok(id, "Contacto creado exitosamente.");

            return Results.Created($"/api/contacts/{id}", response);
        }

        private static async Task<IResult> GetAllAsync(
            IContactServiceAsync contactService)
        {
            var contacts = await contactService.GetAllAsync();

            return Results.Ok(ApiResponse<IEnumerable<ContactResponseDto>>.Ok(contacts));
        }

        private static async Task<IResult> GetByIdAsync(
            Guid id,
            IContactServiceAsync contactService)
        {
            var contact = await contactService.GetByIdAsync(id);

            if (contact is null)
                return Results.NotFound(ApiResponse<object>.Fail("Contacto no encontrado."));

            return Results.Ok(ApiResponse<ContactResponseDto>.Ok(contact));
        }
    }
}
