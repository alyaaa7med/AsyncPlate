using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Services
{
    public interface IMediaService
    {

        Task<string> UploadImageAsync(IFormFile file, string folderName);
        void DeleteImage(string filePath);
    }

}
