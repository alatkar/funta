using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.DTO.General
{
    public class SearchResult<TResultType, TSearchParameter>
    {
        public TSearchParameter SearchParameter { get; set; }
        public List<TResultType> Result { get; set; }
        public int TotalCount { get; set; }
        public int FilterCount { get; set; }
    }

    public class SearchResult<TResultType>
    {
        public List<TResultType> Result { get; set; }
        public int TotalCount { get; set; }
    }
}
