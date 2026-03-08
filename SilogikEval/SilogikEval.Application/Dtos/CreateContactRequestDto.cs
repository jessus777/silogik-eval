namespace SilogikEval.Application.Dtos
{
    public class CreateContactRequestDto
    {
        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string? SecondName { get; set; }
        public string LastName { get; set; } = default!;
        public string? SecondLastName { get; set; }

        public string? Comments { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public Stream? FileStream { get; set; }
    }
}
