using System.ComponentModel.DataAnnotations;

namespace Journals.Model
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string FileName { get; set; }

        public int UserId { get; set; }

        public bool IsSubscribed { get; set; }
    }
}