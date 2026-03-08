using SilogikEval.Application.Commons;

namespace SilogikEval.Application.Entities
{
    public class Contact
        : BaseEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string? SecondName { get; set; }
        public string LastName { get; set; } = default!;
        public string? SecondLastName { get; set; }

        public string Comments { get; set; } = default!;

        public string? FilePath { get; set; }
    }
}
