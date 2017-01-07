using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Journals.Model;

namespace Medico.Model
{

    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }

    public class Journal: BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string FileName { get; set; }

        public DateTime ModifiedDate { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }
        public virtual Collection<Issue> Issues { get; set; }
        public int UserId { get; set; }
    }
}