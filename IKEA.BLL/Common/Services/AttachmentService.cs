using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IKEA.BLL.Common.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> AllowedExtentions = new() { ".jpg", ".jpeg", ".png" };
        private const int FileMaxSize = 2_097_152;//2MB
        public string UploadFile(IFormFile file, string FolderName)
        {
            #region Validations
            var fileextension=Path.GetExtension(file.FileName);
            if(!AllowedExtentions.Contains(fileextension))
            {
                throw new Exception("InValid File Extension Please Try Again");
            }
            if(file.Length > FileMaxSize)
            {
                throw new Exception("InVaild File Size");
            }
            #endregion
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", FolderName);
            if(!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            var FileName = $"{Guid.NewGuid()}{fileextension}"; 
            var FilePath=Path.Combine(FolderPath,FileName);
            using var FileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FileStream);
            return FileName;
        }
        public bool Delete(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }
            return false;
        }
    }
}
