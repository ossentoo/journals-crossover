using System.ComponentModel.DataAnnotations.Schema;

namespace Journals.Model
{
    public class Subscription
    {
        public int Id { get; set; }

        [ForeignKey("JournalId")]
        public Journal Journal { get; set; }

        public int JournalId { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        public int UserId { get; set; }
    }
}