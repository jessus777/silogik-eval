namespace SilogikEval.Api.Models
{
    public class UpdateContactRequest
    {
        public string FirstName { get; set; } = default!;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = default!;

        public string? SecondLastName { get; set; }

        public string Comments { get; set; } = default!;

        public IFormFile? Attachment { get; set; }
    }
}
