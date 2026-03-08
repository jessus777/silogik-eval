using SilogikEval.Application.Commons;

namespace SilogikEval.Application.Entities
{
    public class Language
        : BaseEntity
    {
        public int Id { get; set; }

        public string Code { get; set; } = default!;

        public string Name { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
