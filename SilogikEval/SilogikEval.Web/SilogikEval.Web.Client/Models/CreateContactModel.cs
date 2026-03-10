using Microsoft.AspNetCore.Components.Forms;

namespace SilogikEval.Web.Client.Models
{
    public class CreateContactModel
    {
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string? SecondLastName { get; set; }

        public string Comments { get; set; } = string.Empty;

        public IBrowserFile? Attachment { get; set; }
    }
}
