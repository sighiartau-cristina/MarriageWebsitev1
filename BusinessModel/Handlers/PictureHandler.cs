using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace BusinessModel.Handlers
{
    class PictureHandler
    {

        public void AddFile()
        {
            /*foreach (string upload in Request.Files)
            {
                if (!Request.Files[upload].HasFile()) continue;

                string mimeType = Request.Files[upload].ContentType;
                Stream fileStream = Request.Files[upload].InputStream;
                string fileName = Path.GetFileName(Request.Files[upload].FileName);
                int fileLength = Request.Files[upload].ContentLength;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);

                const string connect = @"Server=.\SQLExpress;Database=FileTest;Trusted_Connection=True;";
                using (var conn = new SqlConnection(connect))
                {
                    var qry = "INSERT INTO FileStore (FileContent, MimeType, FileName) VALUES (@FileContent, @MimeType, @FileName)";
                    var cmd = new SqlCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@FileContent", fileData);
                    cmd.Parameters.AddWithValue("@MimeType", mimeType);
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }*/
        }

        public byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            using (Stream inputStream = file.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                return memoryStream.ToArray();
            }
        }

    }
}
