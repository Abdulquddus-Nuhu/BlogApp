using System;
namespace Core.Interfaces
{
    public interface IFileUploadService
    {
        public string UploadFile(Stream file);
    }
}

