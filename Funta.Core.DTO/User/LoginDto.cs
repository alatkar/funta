using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Funta.Core.DTO.User
{
    [DataContract]
    public class LoginDto
    {
        private const string Pattern = @"^((\+۹|\+۹۸۹|\+\+۹۸۹|۹|۰۹|۹۸۹|۰۹۸۹|۰۰۹۸۹)(۰۱|۰۲|۰۳|۱۰|۱۱|۱۲|۱۳|۱۴|۱۵|۱۶|۱۷|۱۸|۱۹|۲۰|۲۱|۲۲|۳۰|۳۱|۳۲|۳۳|۳۴|۳۵|۳۶|۳۷|۳۸|۳۹|۹۰))(\d{۷})$";



        [DataType(DataType.Password)]
        [Display(Name = "رمزعبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [MinLength(6, ErrorMessage = "{0} باید از {1}  بیشتر باشد")]
        [MaxLength(20, ErrorMessage = "{0} باید از {1}  کمتر باشد")]
        [Description("رمز عبور کاربر برای ورود به سیستم")]
        [DataMember(Name = "password")]
        public string Password { get; set; }


        [Description("شماره همراه کاربر برای ورود به سیستم")]
        [DataType(DataType.Text)]
        [RegularExpression(Pattern, ErrorMessage = "شماره همراه معتبر نمیباشد")]
        [Display(Name = "شماره همراه")]
        [StringLength(11, ErrorMessage = "{0} میبایست {1} کارکتر باشد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [DataMember(Name = "mobile")]
        public string Mobile { get; set; }
    }
}
