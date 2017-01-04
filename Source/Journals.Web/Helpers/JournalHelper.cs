using System.Web;
using Journals.Model;

namespace Medico.Web.Helpers
{
    public static class JournalHelper
    {
        public static void PopulateFile(HttpPostedFileBase file, Journal journal)
        {
            if (file != null && file.ContentLength > 0)
            {
                journal.FileName = System.IO.Path.GetFileName(file.FileName);
                journal.ContentType = file.ContentType;

                using (var reader = new System.IO.BinaryReader(file.InputStream))
                {
                    journal.Content = reader.ReadBytes(file.ContentLength);
                }
            }
        }
    }
}