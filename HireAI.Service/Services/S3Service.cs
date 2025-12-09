using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HireAI.Data.Helpers.DTOs.S3;
using HireAI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucketName = "hireaibucket"; 
        private  readonly string _region = "us-east-1";
        public S3Service(IAmazonS3 s3) {
            _s3 = s3;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty", nameof(file));
            }

            var  key = $"cv/{Guid.NewGuid()}_{file.FileName}";

           using var stream = file.OpenReadStream();    
           
           var transferUtility = new TransferUtility(_s3);

           var transferRequest = new TransferUtilityUploadRequest
           {
               InputStream = stream,
               Key = key,
               BucketName = _bucketName,
               ContentType = file.ContentType
           };

          await transferUtility.UploadAsync(transferRequest);

          return key;
        }

        public async Task<FileDownloadDto> DownloadFileAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }

            var getObjectRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            var response = await _s3.GetObjectAsync(getObjectRequest);

            var fileName = Path.GetFileName(key);

            return new FileDownloadDto
            {
                FileStream = response.ResponseStream,
                ContentType = response.Headers.ContentType,
                FileName = fileName
            };
        }
    }
}
