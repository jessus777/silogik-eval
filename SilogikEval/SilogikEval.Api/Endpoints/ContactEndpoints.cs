using Microsoft.AspNetCore.Mvc;
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
                .Produces<ApiResponse<PagedResult<ContactResponseDto>>>();

            group.MapGet("/{id:guid}", GetByIdAsync)
                .Produces<ApiResponse<ContactResponseDto>>()
                .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

            group.MapPut("/{id:guid}", UpdateAsync)
                .DisableAntiforgery()
                .Accepts<UpdateContactRequest>("multipart/form-data")
                .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
                .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
                .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteAsync)
                .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
                .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);

            return group;
        }

        private static async Task<IResult> CreateAsync(
            [FromForm] CreateContactRequest request,
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
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            [FromQuery] string? search,
            IContactServiceAsync contactService)
        {
            var currentPage = page is > 0 ? page.Value : 1;
            var currentPageSize = pageSize is > 0 and <= 50 ? pageSize.Value : 10;
            var searchTerm = string.IsNullOrWhiteSpace(search) ? null : search.Trim();

            var result = await contactService.GetAllAsync(currentPage, currentPageSize, searchTerm);

            return Results.Ok(ApiResponse<PagedResult<ContactResponseDto>>.Ok(result));
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

        private static async Task<IResult> UpdateAsync(
            Guid id,
            [FromForm] UpdateContactRequest request,
            IContactServiceAsync contactService)
        {
            var dto = new UpdateContactRequestDto
            {
                Id = id,
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

            await contactService.UpdateAsync(dto);

            return Results.Ok(ApiResponse<object>.Ok(null!, "Contacto actualizado exitosamente."));
        }

        private static async Task<IResult> DeleteAsync(
            Guid id,
            IContactServiceAsync contactService)
        {
            await contactService.DeleteAsync(id);

            return Results.Ok(ApiResponse<object>.Ok(null!, "Contacto eliminado exitosamente."));
        }
    }
}
