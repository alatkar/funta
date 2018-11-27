using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.DTO.General
{
    public class SearchParameters<T, Type>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 1000000;
        public bool NeedTotalCount { get; set; } = false;
        public T SearchParameter { get; set; }
        public string Search { get; set; } = "";
    }

    public class BaseSearchParameter : SearchParameters<DateTime, Type>
    {

    }
}
