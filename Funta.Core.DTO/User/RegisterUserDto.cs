using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Funta.Core.DTO.User
{
    public class RegisterUserDto
    {
        //private const string Pattern = @"^((\+۹|\+۹۸۹|\+\+۹۸۹|۹|۰۹|۹۸۹|۰۹۸۹|۰۰۹۸۹)(۰۱|۰۲|۰۳|۱۰|۱۱|۱۲|۱۳|۱۴|۱۵|۱۶|۱۷|۱۸|۱۹|۲۰|۲۱|۲۲|۳۰|۳۱|۳۲|۳۳|۳۴|۳۵|۳۶|۳۷|۳۸|۳۹|۹۰))(\d{۷})$";
        private const string Pattern = @"^((\+9|\+989|\+\+989|9|09|989|0989|00989)(01|02|03|10|11|12|13|14|15|16|17|18|19|20|21|22|30|31|32|33|34|35|36|37|38|39|90))(\d{7})$";

        [RegularExpression(Pattern, ErrorMessage = "شماره همراه معتبر نمیباشد")]
        [Display(Name = "شماره همراه")]
        [StringLength(11, ErrorMessage = "{0} میبایست {1} کارکتر باشد")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [Description("شماره همراه کاربر برای ورود به سیستم")]
        public string Mobile { get; set; }

        [Display(Name = "نام")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [Description("نام کاربر")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [Description("نام خانوادگی کاربر")]
        public string Family { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [Description("شهر محل سکونت کاربر")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [Description("استان محل سکونت کاربر")]
        public string State { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "رمزعبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [MinLength(6, ErrorMessage = "{0} باید از {1}  بیشتر باشد")]
        [MaxLength(20, ErrorMessage = "{0} باید از {1}  کمتر باشد")]
        [Description("رمز عبور")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "رمز عبور مطابقت ندارد")]
        [Display(Name = " تکرار مزعبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [DataType(DataType.Password)]
        [Description("تکرار رمز عبور")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "{0} معتبر نمیباشد")]
        [Display(Name = "ایمیل")]
        [Description("ایمیل کاربر")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "نام کاربری")]
        [Description("نام کاربر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} الزامی میباشد")]
        [MinLength(6, ErrorMessage = "{0} باید از {1}  بیشتر باشد")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "تاریخ تولد")]
        [Description("تاریخ تولد کاربر")]
        public DateTime? BirthDay { get; set; }


        [Display(Name = "شناسه")]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool SoftDelete { get; set; }
        public string SaltForHashing { get; set; }
        public string SerialNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
