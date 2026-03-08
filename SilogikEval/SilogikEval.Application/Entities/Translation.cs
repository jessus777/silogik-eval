using SilogikEval.Application.Commons;

namespace SilogikEval.Application.Entities
{
    public class Translation
        : BaseEntity
    {
        public int Id { get; set; }

        public string LanguageCode { get; set; } = default!;

        public string Key { get; set; } = default!;

        public string Value { get; set; } = default!;
    }
}
