using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace StudyDoIT.Models.Common
{
    public class SentContact
    {
        [Required(ErrorMessage = " Vui lòng nhập email ")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Vui lòng nhập email hợp lệ ")]
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; } 
    }
}