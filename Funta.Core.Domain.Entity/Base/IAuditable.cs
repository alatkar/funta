using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Domain.Entity.Base
{
    public interface IAuditable
    {
        DateTime RegDate { get; set; }
        DateTime? UpdateDate { get; set; }
        bool IsRemoved { get; set; }
    }
}
