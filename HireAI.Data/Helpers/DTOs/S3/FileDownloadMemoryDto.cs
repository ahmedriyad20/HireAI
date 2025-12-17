using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.S3
{
    /// <summary>
    /// DTO for file download to memory response
    /// Used for in-memory processing without disk I/O
    /// </summary>
    public class FileDownloadMemoryDto
    {
        /// <summary>
        /// The complete file content as byte array
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// The MIME type of the file (e.g., "application/pdf")
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The original filename
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public int FileSize => FileContent?.Length ?? 0;
    }
}
