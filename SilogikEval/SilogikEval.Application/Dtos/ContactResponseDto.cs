namespace SilogikEval.Application.Dtos
{
    public class ContactResponseDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = default!;

        public string? SecondLastName { get; set; }

        public string Comments { get; set; } = default!;

        public string? FilePath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
