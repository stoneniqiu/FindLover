using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace NoteSystem.Helper
{
    public class ValidateCode
    {
        public ValidateCode()
        {

        }

        ///<summary>

        /// 验证码的最大长度

        ///</summary>

        public int MaxLength
        {

            get { return 10; }

        }

        ///<summary>

        /// 验证码的最小长度

        ///</summary>

        public int MinLength
        {

            get { return 1; }

        }

        ///<summary>

        /// 生成验证码

        ///</summary>

        ///<param name="length">指定验证码的长度</param>

        ///<returns></returns>

        public string CreateValidateCode(int length)
        {

            var randMembers = new int[length];

            var validateNums = new int[length];

            var validateNumberStr = "";

            //生成起始序列值

            var seekSeek = unchecked((int) DateTime.Now.Ticks);

            var seekRand = new Random(seekSeek);

            var beginSeek = (int) seekRand.Next(0, Int32.MaxValue - length*10000);

            var seeks = new int[length];

            for (var i = 0; i < length; i++)
            {

                beginSeek += 10000;

                seeks[i] = beginSeek;

            }

            //生成随机数字

            for (int i = 0; i < length; i++)
            {

                var rand = new Random(seeks[i]);

                var pownum = 1*(int) Math.Pow(10, length);

                randMembers[i] = rand.Next(pownum, Int32.MaxValue);

            }

            //抽取随机数字

            for (int i = 0; i < length; i++)
            {

                var numStr = randMembers[i].ToString(CultureInfo.InvariantCulture);

                var numLength = numStr.Length;

                var rand = new Random();

                var numPosition = rand.Next(0, numLength - 1);

                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));

            }

            //生成验证码

            for (var i = 0; i < length; i++)
            {

                validateNumberStr += validateNums[i].ToString();

            }

            return validateNumberStr;

        }

        /// <summary>
        ///  创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        /// <param name="validateCode"></param>
        public byte[] CreateValidateGraphic(string validateCode)
        {

            var image = new Bitmap((int) Math.Ceiling(validateCode.Length*14.0), 22);

            var g = Graphics.FromImage(image);

            try
            {

                //生成随机生成器

                var random = new Random();

                //清空图片背景色

                g.Clear(Color.White);

                //画图片的干扰线

                for (var i = 0; i < 25; i++)
                {

                    int x1 = random.Next(image.Width);

                    int x2 = random.Next(image.Width);

                    int y1 = random.Next(image.Height);

                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);

                }

                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),

                                                                    Color.Blue, Color.DarkRed, 1.5f, true);

                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰点

                for (int i = 0; i < 100; i++)
                {

                    int x = random.Next(image.Width);

                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));

                }

                //画图片的边框线

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据

                MemoryStream stream = new MemoryStream();

                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流

                return stream.ToArray();

            }

            finally
            {

                g.Dispose();

                image.Dispose();

            }

        }
    }
}