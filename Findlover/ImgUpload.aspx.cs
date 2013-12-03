using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Findlover.Models;

namespace Findlover
{
    public partial class ImgUpload : System.Web.UI.Page
    {
        private LoveDbRepository db;
        private int _userId;
        protected void Page_Load(object sender, EventArgs e)
        {
            db = LoveDbRepository.GetInstance();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile == true)//HasFile用来检查FileUpload是否有指定文件
            {
                var user = db.One((User u) => u.UserId == CheckValid());
                if (user != null)
                {
                    string url = "";
                    var extension = Path.GetExtension(FileUpload1.FileName);
                    if (extension != null)
                    {
                        string image = extension.ToLower();//System.IO.Path.GetExtension获得文件的扩展名
                        if (image != ".bmp" && image != ".png" && image != ".gif" && image != ".jpg")// 这里你自己加入其他图片格式，最好全部转化为大写再判断，我就偷懒了
                        {
                            Response.Write("<script>alert('请选择图片！')</script>");
                            return; // 这样用return 避免太多的嵌套
                        }
                    }

                    string filename = FileUpload1.FileName;
                    string savePath = Server.MapPath(("Content/UploadFiles/"));
                    string path = Path.Combine(savePath, user.UserName, "Photos", "Portrait");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var pathtemp = Path.Combine(path, filename);//先存入临时文件夹

                    FileUpload1.SaveAs(pathtemp);// 把照片存入了文件夹
                    url = Path.Combine("Content/UploadFiles/", user.UserName, "Photos", "Portrait", filename);
                    Image1.ImageUrl = url;// 显示图片~     就ok了。

                }
                else
                {
                    Response.Write("<script>alert('还没有登录')</script>");
                }
              
            }
        }
        public int CheckValid()
        {
            if (_userId != 0)
            {
                return _userId;
            }
            var cook = Request.Cookies["fduid"];
            if (cook != null)
            {
                var uid = Convert.ToInt16(cook.Value);
                Session["uid"] = uid;
                Session.Timeout = 600;
                //   Logger.Debug("用户id:"+uid+"登陆了");
                db.LoginCountAdd(uid);
                _userId = uid;
                return uid;
            }
            if (Session["uid"] != null)
            {
                _userId = Convert.ToInt16(Session["uid"].ToString());
                return _userId;
            }
            return -1;
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            var user = db.One((User u) => u.UserId == CheckValid());
            if (user != null)
            {
                var path = Path.Combine("../../", Image1.ImageUrl);
                var dictinary = new Dictionary<string, object>
                {
                    {"ImgUrl", path}
                };
                db.Update<User>(user.UserId, dictinary);
                var box = new Iamgbox
                {
                    ActionTime = DateTime.Now,
                    BoxName = user.UserName,
                    ImgUrl = path,
                    IsValid = true, //默认是正规合适的图片 不合适在检举
                    PraiseCount = 0,
                    Remark = "我的头像",
                    UserId = user.UserId,
                    VisitCount = 0,
                };
                db.Add(box);
                var state = new State
                {
                    UserId = user.UserId,
                    ActionTime = box.ActionTime,
                    Content = "我刚刚更换了头像:<br/><div class='imgtigger'><img src='" + path + "' /></div>",
                    StateType = StateType.Image.ToString(),
                };
                db.Add(state);
            }
            Response.Redirect("/User/Index");
        }
    }
}