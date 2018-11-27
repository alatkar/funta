using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Helper.Enums
{
    public enum FileUploadResult
    {
        Success,
        MimeTypeNotValid,
        SizeExceeded,
    }

    public enum DeleteFileResult
    {
        Success,
        NotExist,
        NotAccess,
    }
}
