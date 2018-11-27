using Funta.Core.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funta.Core.Domain.Entity.Auth
{
    [Table("UserToken", Schema = "dbo")]
    public class UserToken : BaseEntity<int>
    {
        [DataType(DataType.Text)]
        public string AccessTokenHash { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        [DataType(DataType.Text)]
        public string RefreshTokenIdHashSource { get; set; }

        [DataType(DataType.Text)]
        public string RefreshTokenIdHash { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        [ForeignKey(nameof(UserKey))]
        public virtual Users User { get; set; }
        public Guid UserKey { get; set; } // one-to-one association
    }
}
