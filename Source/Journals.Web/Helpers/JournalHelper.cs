using System.Web;
using Medico.Model;

namespace Medico.Web.Helpers
{
    public static class JournalHelper
    {
        public static void PopulateFile(HttpPostedFileBase file, Journal journal, JournalIssue issue)
        {
            if (file != null && file.ContentLength > 0)
            {
                journal.FileName = System.IO.Path.GetFileName(file.FileName);
                issue.ContentType = file.ContentType;

                using (var reader = new System.IO.BinaryReader(file.InputStream))
                {
                    issue.Content = reader.ReadBytes(file.ContentLength);
                }
            }
        }
    }
}