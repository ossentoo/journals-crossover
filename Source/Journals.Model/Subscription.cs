using System.ComponentModel.DataAnnotations.Schema;
using Journals.Model;

namespace Medico.Model
{
    public class Subscription : BaseEntity
    {
        [ForeignKey("JournalId")]
        public Journal Journal { get; set; }

        public int JournalId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        public int UserId { get; set; }
    }
}