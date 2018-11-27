using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Domain.Entity.Auth
{
    public enum StatusUserEnum
    {
        NotActive,
        UserNameNotExist,
        PasswordIsWrong,
        LoggeIn
    }
}
