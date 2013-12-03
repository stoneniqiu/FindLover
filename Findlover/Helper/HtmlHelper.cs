using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Findlover.Helper
{
    public class HtmlHelper
    {
        /// <summary>
        /// 将安全的字符串 替换为html 这样有样式的字符串到了前台才会显示成元素
        /// </summary>
        /// <param name="safeStr"></param>
        /// <returns></returns>
        public static string TransStringToHtml(string safeStr)
        {
            return safeStr.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&nbsp;", " ").Replace("&amp;nbsp;", " ").Replace("&amp;", "").Replace("nbsp;", "");
        }

        /// <summary>
        /// 专门检测前台editor传过来的数据是不是为空！且长度小于 lenth
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty(string str,int lenth=5)
        {
            var htmlstr = TransStringToHtml(str);
            var strs = TransHtmlToString(htmlstr).Trim();
            return strs == "" || strs.Length < lenth;
        }

        public static string GetRealString(string editorstr)
        {
            var htmlstr = TransStringToHtml(editorstr);
            return TransHtmlToString(htmlstr).Trim();
        }

        public static string TransHtmlToString(string htmlString)
        {
            var regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            var strOutput = regex.Replace(htmlString, "");//替换掉"<"和">"之间的内容
            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("&nbsp;", "");
            return strOutput;
        }

       
    }
}