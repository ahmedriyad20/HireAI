using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.S3
{
    /// <summary>
    /// DTO for file download response (stream-based)
    /// Used for direct download to client/device
    /// </summary>
    public class FileDownloadDto
    {
        /// <summary>
        /// The file stream from S3
        /// </summary>
        public Stream FileStream { get; set; }

        /// <summary>
        /// The MIME type of the file (e.g., "application/pdf")
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The original filename
        /// </summary>
        public string FileName { get; set; }
    }
}