namespace SilogikEval.Application.Dtos
{
    public class UpdateContactRequestDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = default!;

        public string? SecondLastName { get; set; }

        public string Comments { get; set; } = default!;

        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public Stream? FileStream { get; set; }
    }
}
