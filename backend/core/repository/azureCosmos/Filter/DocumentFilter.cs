using System;
using System.Collections.Generic;
using System.Text;

namespace core.repository.azureCosmos.Filter
{
    public class DocumentFilter
    {
        public DocumentFilter() { }

        public DocumentFilter(DocumentFilter query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            //this.Expression = query.Expression;
            this.OrderByDescending = query.OrderByDescending;
            this.OrderByPath = query.OrderByPath;
            this.Top = query.Top;
        }

        /// <summary>
        /// Gets or sets an optional <see cref="IExpression"/> which acts a filter for results returned.
        /// </summary>


        /// <summary>
        /// Gets or sets a value which indicates whether the results should be order in a descending manner.
        /// </summary>
        public bool OrderByDescending { get; set; }

        /// <summary>
        /// Gets or sets the path of value to use for ordering.
        /// </summary>
        /// <example>createdDate.epoch</example>
        public string OrderByPath { get; set; }

        /// <summary>
        /// Gets or sets an optional limit to the number of matching results.
        /// </summary>
        public int? Top { get; set; }
    }
}
