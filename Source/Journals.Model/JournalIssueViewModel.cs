using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medico.Model
{
    public class JournalIssueViewModel
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                DisplayTitle = $"Issue # {_id}";
            }
        }

        [Required]
        public string Title { get; set; }

        public string DisplayTitle { get; private set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        [Required, ValidateFile]
        public HttpPostedFileBase File { get; set; }
        public int JournalId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

    }

    public class JournalIssueViewList : List<JournalIssueViewModel>
    {
        public JournalIssueViewList(List<JournalIssueViewModel> items)
        {
            if (items != null && items.Any())
            {
                var firstItem = items.First();
                Title = firstItem.Title;
                JournalId = firstItem.JournalId;

                AddRange(items);
            }
        }

        public string Title { get; private set; }
        public int JournalId { get; private set; }
    }
}