using Funta.Core.DTO.Constants;
using Funta.Core.Helper.Enums;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Funta.Core.Helper.FileUploader
{
    public class ImageUploaderService : IImageUploaderService
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly string[] _allowedMimeTypes;
        private readonly int _maximumAllowedSizeInKb;
        private string _fileName;
        private string _extension;
        private string _fullPathRoot;
        private string _fullPathRelative;
        private int _fileSize;

        public ImageUploaderService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _allowedMimeTypes = new string[] { "jpg", "jpeg", "png" };
            _maximumAllowedSizeInKb = 2000000;
        }

        public FileUploadResult Upload(IFormFile file, string entityImage, Constants.EntityImageSizeFolders imageSize, int width, int height)
        {
            _fileSize = GetImageSize(file);
            _extension = GetImageExtension(file);
            _fileName = Guid.NewGuid().ToString();
            _fullPathRoot = GetFullPath(entityImage, _fileName, _extension);
            _fullPathRelative = _fullPathRoot + "/" + imageSize;

            if (_fileSize > _maximumAllowedSizeInKb)
                return FileUploadResult.SizeExceeded;

            if (!_allowedMimeTypes.Contains(_extension))
                return FileUploadResult.MimeTypeNotValid;
            file.CopyTo(new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, _fullPathRoot), FileMode.Create));

            using (var image = Image.FromFile(_fullPathRoot, true))
            {
                using (var newImage = ScaleImage(image, width, height))
                {
                    if (!File.Exists(_fullPathRelative))
                    {
                        newImage.Save(_fullPathRelative, ImageFormat.Jpeg);
                    }
                }
            }
            Delete(entityImage, _fileName);
            return FileUploadResult.Success;
        }





        public DeleteFileResult Delete(string entityImage, string fileName)
        {
            string relativePath = "/" + entityImage + "/" + fileName;
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return DeleteFileResult.Success;
            }
            return DeleteFileResult.NotExist;
        }

        public string GetImageExtension(IFormFile file)
        {
            var fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            return extension;
        }

        public int GetImageSize(IFormFile file)
        {
            return (int)file.Length;
        }

        public string GetFullPath(string entityImage, string fileName, string fileExtension)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, entityImage);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, entityImage, fileName);
            return fullPath;
        }


        public string GetImageFileName(IFormFile file)
        {
            return file.FileName;
        }

        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public FileUploadResult UploadByteArray(string base64String, string entityImage, string imageSizeFolder, int width, int height, ref string fileName)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            _extension = "jpg";
            Image image;
            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                bitmap = new Bitmap(image);
            }
            string fullFileName = Guid.NewGuid().ToString() + ".jpg";
            fileName = fullFileName;
            if (bytes.Length > _maximumAllowedSizeInKb)
                return FileUploadResult.SizeExceeded;

            _fullPathRoot = GetFullPath(entityImage, fullFileName, _extension);
            _fullPathRelative = GetFullPath(entityImage + "/" + imageSizeFolder, fullFileName, _extension);

            string path = "";

            //bitmap.Save(_fullPathRoot, ImageFormat.Jpeg);

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(_fullPathRoot, FileMode.Create, FileAccess.ReadWrite))
                {
                    bitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes2 = memory.ToArray();
                    fs.Write(bytes2, 0, bytes2.Length);
                }
            }



            using (var img = Image.FromFile(_fullPathRoot, true))
            {
                using (var newImage = ScaleImage(img, width, height))
                {
                    if (!File.Exists(_fullPathRelative))
                    {
                        newImage.Save(_fullPathRelative, ImageFormat.Jpeg);
                    }
                }
            }
            Delete(entityImage, fullFileName);
            fileName = fullFileName;
            return FileUploadResult.Success;

        }
    }
}
