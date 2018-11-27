using Funta.Core.DTO.Constants;
using Funta.Core.Helper.Enums;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Funta.Core.Helper.FileUploader
{
    public interface IImageUploaderService
    {
        FileUploadResult Upload(IFormFile file, string entityImage, Constants.EntityImageSizeFolders imageSizeFolder, int width, int height);
        FileUploadResult UploadByteArray(string base64String, string entityImage, string imageSizeFolder, int width, int height, ref string fileName);
        DeleteFileResult Delete(string entityImage, string fileName);
        string GetImageExtension(IFormFile file);
        string GetImageFileName(IFormFile file);
        int GetImageSize(IFormFile file);
        string GetFullPath(string entityImage, string fileName, string fileExtension);
        Image ScaleImage(Image image, int maxWidth, int maxHeight);
    }
}
