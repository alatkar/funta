using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Funta.Core.Helper.FileUploader
{
    public static class UploadFile
    {
        public static void UploadExcelFile(string path, string base64File, string fileName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(Path.Combine(path, fileName)))
                File.Create(Path.Combine(path, fileName)).Close();
            Byte[] bytes = Convert.FromBase64String(base64File);
            File.WriteAllBytes(Path.Combine(path, fileName), bytes);
        }
    }
}
