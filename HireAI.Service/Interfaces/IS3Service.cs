using Microsoft.AspNetCore.Http;
using HireAI.Data.Helpers.DTOs.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
                                    
namespace HireAI.Service.Interfaces
{
    public interface IS3Service
    {
        /// <summary>
        /// Upload a file to S3 bucket
        /// Returns the S3 key (not the full URL)
        /// </summary>
        public Task<string> UploadFileAsync(IFormFile file);
    
        /// <summary>
        /// Download a file from S3 as a stream
        /// Used for direct download to device/client
        /// Returns the response stream directly from S3
        /// </summary>
        public Task<FileDownloadDto> DownloadFileAsync(string key);

        /// <summary>
        /// Download a file from S3 into memory as byte array
        /// Useful for processing files without writing to disk
        /// </summary>
        public Task<FileDownloadMemoryDto> DownloadFileToMemoryAsync(string key);
    }
}
