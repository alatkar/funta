using Funta.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funta.Core.Domain.Entity.Base
{
    [NotMapped]
    public abstract class BaseEntity<Type> : IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Type Id { get; set; }
        public DateTime RegDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public bool IsRemoved { get; set; } = false;
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
