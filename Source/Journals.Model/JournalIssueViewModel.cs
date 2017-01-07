using System.ComponentModel.DataAnnotations;

namespace Medico.Model
{
    public class JournalIssueViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}