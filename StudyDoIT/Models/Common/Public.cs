using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using StudyDoIT.Models;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;

namespace StudyDoIT.Models.Common
{
    public class Public
    {
        public static int CountString(string keyword, string review)
        {
            string sN = Convert.ToString(keyword);
            Regex thegex = new Regex(sN.ToLower());
            MatchCollection theMatches = thegex.Matches(review);
            return theMatches.Count;
        }

        public static string FormatNumber(string num)
        {
            string str = "";
            int k = num.Length % 3;
            if (k > 0)
            {
                str += num.Substring(0, k) + ".";
            }

            while (k < num.Length)
            {
                str += num.Substring(k, 3) + ".";
                k = k + 3;
            }

            return str = str.Substring(0, str.Length - 1);

        }

        public enum TrangThai
        {
            [Display(Name = "Sử dụng")]
            SuDung,
            [Display(Name = "Không sử dụng")]
            KhongSuDung
        }

        public static int TotalDay(DateTime start, DateTime end)
        {
            TimeSpan Time = start - end;
            return Time.Days;
        }


        public static string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",  
                "đ",  
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",  
                "í","ì","ỉ","ĩ","ị",  
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",  
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",  
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",  
                "d",  
                "e","e","e","e","e","e","e","e","e","e","e",  
                "i","i","i","i","i",  
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",  
                "u","u","u","u","u","u","u","u","u","u","u",  
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }

            return Regex.Replace(text.ToLower().Replace(@"'", String.Empty), @"[^\w]+", "-").Replace("\"", "-").Replace(":", "-").ToLower();
        }

        public static string NonUnicodeJapan(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",  
                "đ",  
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",  
                "í","ì","ỉ","ĩ","ị",  
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",  
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",  
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",  
                "d",  
                "e","e","e","e","e","e","e","e","e","e","e",  
                "i","i","i","i","i",  
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",  
                "u","u","u","u","u","u","u","u","u","u","u",  
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }

            return Regex.Replace(text.ToLower().Replace(@"'", String.Empty), @"[^\w]+", "-").Replace("\"", "-").Replace(":", "-").ToLower();
        }

        public static string MaHoaMD5(string str)
        {
            Byte[] dauvao = ASCIIEncoding.Default.GetBytes(str);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                var daura = md5.ComputeHash(dauvao);
                return BitConverter.ToString(daura).Replace("-", "").ToLower();
            }
        }

        public static string SHA256(string strData)
        {
            var message = Encoding.ASCII.GetBytes(strData);
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string RandomNumber(int length)
        {
            string str = "0123456789";
            int strlen = str.Length;
            Random rnd = new Random();
            string retVal = String.Empty;

            for (int i = 0; i < length; i++)
                retVal += str[rnd.Next(strlen)];

            return retVal;
        }

        public static string RandomStringNumber(int length)
        {
            string str = "0123456789abcdefghijklmnopqrstvxyzABCDÈGHIJKLMNOPQRSTVXYZ";
            int strlen = str.Length;
            Random rnd = new Random();
            string retVal = String.Empty;

            for (int i = 0; i < length; i++)
                retVal += str[rnd.Next(strlen)];

            return retVal;
        }

        public static string GetID()
        {
            Random rd = new Random();
            string str = DateTime.Now.ToString("yyMMddhhmmss")+rd.Next(100,999);
            int milliseconds = 10;
            Thread.Sleep(milliseconds);
            return str;
        }

        public static string GetID2()
        {
            Random rd = new Random();
            string str = DateTime.Now.ToString("yyMMddhhmmss") + rd.Next(100, 999);
            int milliseconds = 1000;
            Thread.Sleep(milliseconds);
            return str;
        }

    }
}