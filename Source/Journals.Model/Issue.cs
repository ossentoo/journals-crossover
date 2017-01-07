using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medico.Model
{
    public class Issue: BaseEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public DateTime ModifiedDate { get; set; }
        [ForeignKey("JournalId")]
        public Journal Journal { get; set; }
        public int JournalId { get; set; }
    }
}