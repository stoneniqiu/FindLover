using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Findlover.Helper;
using Findlover.Models;
using Findlover.ViewModel;

namespace Findlover.Controllers
{
    public class UserController : BaseController
    {
        private string _uName;

        #region 个人主页
        /// <summary>
        /// 个人主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            var lid = CheckValid();
            if (lid == -1 || Session["uid"] == null)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }

            //切换编辑状态
            if (Session["edit"] != null)
            {
                if (Session["edit"].ToString() != "0")
                {
                    ViewBag.Edit = Session["edit"].ToString();
                    Session["edit"] = 0;
                }
            }
            //区分是不是自己
            if (id == null)
            {
                id = lid;
                ViewBag.User = "Self";
            }
            else
            {
                ViewBag.User = "Visitor";
                if (Session["uid"] != null && Session["uid"].ToString() == id.ToString())
                {
                    ViewBag.User = "Self";
                }
            }
            //同性处理
            var isSame = false; ViewBag.SameSex = "diff";
            if (id != lid && GetMyself().User.Sex == GetSex((int)id))
            {
                ViewBag.SameSex = "same";
                isSame = true;
            }

            _uName = GetUserNameById((int)id);
            var user = LoveDb.One((User n) => n.UserId == id);
            ViewBag.Oid = lid;
            if (id != lid)
            {
                ViewBag.Oid = id;
                // 记录访问次数
                if (!isSame)//同性不记录
                LoveDb.VisitCount(user, LoveDb.One((User n) => n.UserId == CheckValid()));
            }

            //需要处理开始没有资料 为空的情况，建议User创建之后马上加入这个表 
            //还需要创建资料完整度表 

            var userinfo = new UserInfo
            {
                UserId = user.UserId,
                ImgUrl = user.ImgUrl,
                UserName = user.UserName,
                Enable = user.Enable,
                BaseInfo = LoveDb.One((BaseInfo n) => n.UserId == id),
                LoveView = LoveDb.One((LoveView n) => n.UserId == id),
                DetailInfo = LoveDb.One((DetailInfo n) => n.UserId == id),
                Requirement = LoveDb.One((Requirement n) => n.UserId == id),
                LastState = GetLastState(id)
            };

            ViewBag.Sex = user.Sex == "man" ? "他" : "她";

            int agenow = GetOld(user.BrithDate);
            ViewData["Age"] = agenow;
            if (user.Age != agenow)//updata user's age  可以生日送祝福。
            {
                var dictinary = new Dictionary<string, object>
                    {
                        {"Age", agenow}
                    };
                LoveDb.Update<User>(user.UserId, dictinary);
            }

            return View(userinfo);
        }

        public ActionResult EditIndex(int index)
        {
            Session["edit"] = index;
            return RedirectToAction("Index");
        }

        public ActionResult RequireEdit()
        {
            var id = CheckValid();
            var lid = CheckValid();
            if (lid == -1 || Session["uid"] == null)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            var require = LoveDb.One((Requirement r) => r.UserId == id);
            if (require != null)
            {
                ViewBag.Requir = GetRequirStr(require);
            }
            return PartialView();
        }

        #endregion

        #region 用户副页  对比，个人资料统计
        /// <summary>
        /// 比较两者
        /// </summary>
        /// <param name="userid">被比较的人</param>
        /// <returns></returns>
        public ActionResult CompareUser(int userid)
        {
            var id = CheckValid();
            if (userid == id)
            {
                return null;
            }
            var rate = GetRecommendRate(id, userid);
            var meuin = GetMyself();
            var other = LoveDb.GetUninUser(userid);
            var myre = LoveDb.One((Requirement r) => r.UserId == id);
            var youre = LoveDb.One((Requirement r) => r.UserId == userid);
            ViewBag.Sex = meuin.User.Sex == "man" ? "她" : "他";

            var comp = new CompareUsers
            {
                Wo = meuin,
                Ta = other,
                LoveInts = GetLoveInts(other, meuin),
                MyRequirement = myre,
                OtherRequirement = youre,
                TaInts = GetTomeInts(meuin, youre),
                WoInts = GetTomeInts(other, myre),
                Rate = rate
            };

            return PartialView(comp);
        }

        /// <summary>
        /// 展示个人的一些资料
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInfo()
        {
            var my = GetMyself();
            var pinfo = new PersonInfo
            {
                Myself = my,
                IloveCount = LoveDb.MyLoveAll().Count(n => n.UserId == my.User.UserId),
                LoveMeCount = LoveDb.MyLoveAll().Count(n => n.LoverId == my.User.UserId),
                Persent = GetPersent(my.User.UserId),
                Visitcount = LoveDb.VisitorAll().Where(n => n.UserId == my.User.UserId).Sum(n => n.Count),
            };
            pinfo.Info = pinfo.Persent < 0.3 ? "还需要完善" : "还不错哦";

            return PartialView(pinfo);
        }

        #endregion

        #region 搜索

        /// <summary>
        /// 最初是根据需求里面的标准。
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            if (CheckValid() == -1)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            return View();
        }

        public ActionResult UserPage(Search conditon)
        {
            //首先有两个条件: 地方，年龄。
            var id = CheckValid();
            if (CheckValid() == -1)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            var baseusers = GetBaseUsers(id);
            var simpleUsers = new List<SimpleUser>();
            if (conditon.AgeUp != 0)
            {
                // 找到uninusers
                simpleUsers.AddRange(from user1 in baseusers
                                     select new UninUser
                                     {
                                         BaseInfo = LoveDb.One((BaseInfo b) => b.UserId == user1.UserId),
                                         DetailInfo = LoveDb.One((DetailInfo d) => d.UserId == user1.UserId),
                                         User = user1
                                     }
                                         into uni
                                         where MarchUser(conditon, uni)
                                         select new SimpleUser
                                         {
                                             Age = uni.User.Age,
                                             ImgUrl = uni.User.ImgUrl,
                                             UserName = uni.User.UserName,
                                             UserId = uni.User.UserId,
                                             City = uni.BaseInfo.ResidenceCity
                                         });
            }
            else
            {
                simpleUsers = GetSimpleUsers(baseusers);
            }
            return PartialView(simpleUsers);
        }

        /// <summary>
        /// 转换为simpleusers
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private List<SimpleUser> GetSimpleUsers(IEnumerable<User> users)
        {
            return users.Select(user => new SimpleUser
                {
                    Age = user.Age,
                    ImgUrl = user.ImgUrl,
                    UserName = user.UserName,
                    UserId = user.UserId,
                    City = LoveDb.One((BaseInfo b) => b.UserId == user.UserId).ResidenceCity
                }).ToList();
        }

        /// <summary>
        /// 匹配user是否符合搜索条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="biguser"></param>
        /// <returns></returns>
        private bool MarchUser(Search condition, UninUser biguser)
        {
            // 先判断user的部分
            var residenceflag = false;
            var ageflag = false;
            var educationflag = false;
            var incomeflag = false;
            var housingflag = false;
            var carflag = false;
            var conflag = false;
            var hightflag = false;
            var nativeflag = false;
            var peopleflag = false;
            var stateflag = false;

            // 还要判断user 自身的资料是否是不为null的。

            if ((biguser.BaseInfo.ResidenceCity != null && biguser.BaseInfo.ResidenceCity.Contains(condition.ResidenceCity)) || string.IsNullOrEmpty(condition.ResidenceCity))
            {
                residenceflag = true;
            }
            //年龄
            if ((condition.AgeUp == 0 || (biguser.User.Age != 0 && biguser.User.Age <= condition.AgeLow && biguser.User.Age >= condition.AgeUp))//agelow 要比 ageup 大
                || (condition.AgeLow == 0 && biguser.User.Age >= condition.AgeUp))
            {
                ageflag = true;
            }
            //学历
            if (string.IsNullOrEmpty(condition.Education) || condition.Education == "学历不限" || biguser.BaseInfo.Education != null && ((condition.Education.Contains("及以上") && IntEducation(biguser.BaseInfo.Education) >= IntEducation(condition.Education))
                 || (!condition.Education.Contains("及以上") && IntEducation(biguser.BaseInfo.Education) == IntEducation(condition.Education))))
            {
                educationflag = true;
            }
            //收入
            if (string.IsNullOrEmpty(condition.MonthlyIncome) ||
              (biguser.BaseInfo.MonthlyIncome != null && ((IntIncome(biguser.BaseInfo.MonthlyIncome) >= IntIncome(condition.MonthlyIncome) && condition.MonthlyIncome != "3000元以下") ||
              IntIncome(biguser.BaseInfo.MonthlyIncome) <= IntIncome(condition.MonthlyIncome) && condition.MonthlyIncome == "3000元以下")))
            {
                incomeflag = true;
            }
            // 汽车
            if (string.IsNullOrEmpty(condition.Car) || (biguser.DetailInfo.Car != null && biguser.DetailInfo.Car == condition.Car))
            {
                carflag = true;
            }
            //住房
            if (string.IsNullOrEmpty(condition.Housing) ||
                (biguser.DetailInfo.Housing != null && biguser.DetailInfo.Housing == condition.Housing))
            {
                housingflag = true;
            }
            //星座
            if (string.IsNullOrEmpty(condition.Constellation) || (biguser.DetailInfo.Constellation != null && biguser.DetailInfo.Constellation == condition.Constellation))
            {
                conflag = true;
            }
            //身高
            int hight = Convert.ToInt32(biguser.BaseInfo.Height.Split('厘')[0]);
            if (condition.HightUp == 0 || (biguser.BaseInfo.Height != null && ((hight <= condition.HightLow && hight >= condition.HightUp && condition.HightLow != 0)
                || (condition.HightLow == 0 && hight >= condition.HightUp))))
            {
                hightflag = true;
            }
            //籍贯
            if (string.IsNullOrEmpty(condition.NativePlace) || (biguser.DetailInfo.NativePlace != null && (condition.NativePlace == Trimplace(biguser.DetailInfo.NativePlace) ||
                biguser.DetailInfo.NativePlace.Contains(condition.NativePlace))))
            {
                nativeflag = true;
            }
            if (string.IsNullOrEmpty(condition.People) || (biguser.DetailInfo.People != null && condition.People == biguser.DetailInfo.People))
            {
                peopleflag = true;
            }
            if (string.IsNullOrEmpty(condition.State) || (biguser.BaseInfo.State != null && condition.State == biguser.BaseInfo.State))
            {
                stateflag = true;
            }

            return residenceflag && ageflag && educationflag && incomeflag && housingflag &&
                   carflag && conflag && hightflag && nativeflag && peopleflag && stateflag;
        }

        private string Trimplace(string place)
        {
            var ss = place.Split(' ');
            return ss[0] + ss[1];
        }

        //转换薪资
        private int IntIncome(string income)
        {
            var xinshui = 1000;
            switch (income)
            {
                case "3000元以下":
                    xinshui = 2000;//这个值 比3500小就行
                    break;
                case "3000元以上":
                case "3000-4000元":
                    xinshui = 3500;
                    break;
                case "4000元以上":
                case "4000-5000元":
                    xinshui = 4500;
                    break;
                case "5000元以上":
                case "5000-6000元":
                    xinshui = 5500;
                    break;
                case "6000元以上":
                case "6000-8000元":
                    xinshui = 6500;
                    break;
                case "8000元以上":
                case "8000-10000元":
                    xinshui = 8500;
                    break;
                case "10000元以上":
                case "10000-20000元":
                    xinshui = 11000;
                    break;
                case "20000元以上":
                    xinshui = 21000;
                    break;
            }
            return xinshui;
        }

        //转换学历
        private int IntEducation(string edu)
        {
            var index = 0;//表示不限。
            switch (edu)
            {
                case "大专以下":
                    index = 1;
                    break;
                case "大专":
                case "大专及以上":
                    index = 2;
                    break;
                case "本科":
                case "本科及以上":
                    index = 3;
                    break;
                case "硕士":
                case "硕士及以上":
                    index = 4;
                    break;
                case "博士":
                case "博士及以上":
                    index = 5;
                    break;
            }
            return index;
        }

        #endregion

        #region 修改图像
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UpLoadPhoto()
        {
            var user = LoveDb.One((User n) => n.UserId == CheckValid());
            return View(user);
        }

        /// <summary>
        /// 获取该用户的相片
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult ImageBox(int userid)
        {
            var imgs = LoveDb.IamgAll().Where(n => n.UserId == userid);
            ViewBag.IsSelf =userid == CheckValid()? "Self":"Other";
            return PartialView(imgs);
        }

        public void DeletImg(int id)
        {
            LoveDb.Delete<Iamgbox>(id);
            // 没有真正的删除图片。只是相册中看不见了。
        }

        public ActionResult StateList(int userid)
        {
            var states = LoveDb.StateAll().Where(n => n.UserId == userid).OrderByDescending(n=>n.Id);
            ViewBag.IsSelf = userid == CheckValid() ? "Self" : "Other";
            return PartialView(states);
        }

        /// <summary>
        /// 头像，相册上传
        /// </summary>
         [AcceptVerbs(HttpVerbs.Post)]
        public void UpLoadPhoto(HttpPostedFileBase  file)
        {
          var res = CheckImg(file,false);
            if (res == "ok")
            {
                var fileName = file.FileName;//Path.GetExtension() 也许可以解决这个问题，先不管了。
                var pathtemp = Path.Combine(Server.MapPath("../Content/TempFile/"), fileName);//先存入临时文件夹
                var scrtemp = Path.Combine("../../Content/TempFile/", fileName);//图片展示的地址

                var list = Session["Imgscr"] as List<string>;
                var slist = Session["ImgServerscr"] as List<string>;
                if (list != null)
                {
                    list.Add(scrtemp);
                }
                else
                {
                    list = new List<string> { scrtemp };
                    Session["Imgscr"] = list;
                }
                if (slist != null)
                {
                    slist.Add(pathtemp);
                }
                else
                {
                    slist = new List<string> { pathtemp };
                    Session["ImgServerscr"] = slist;
                }

                file.SaveAs(pathtemp);
                Response.Write("<img src='../../Content/TempFile/" + fileName + "' /> ");
            }
            else
            {
                Response.Write(res);
            }
        }

        /// <summary>
        /// 话题中的图片 意中人呢？ 为什么topic 图片存不下去！
        /// </summary>
        /// <param name="file"></param>
        [HttpPost]
        public void UploadTextPhoto(HttpPostedFileBase file)
        {   
            var res = CheckImg(file,false);
            if (res == "ok")
            {
                _uName = GetUserNameById(CheckValid());
                var fileName = file.FileName;//Path.GetExtension() 也许可以解决这个问题，先不管了。
                string path = Path.Combine(HttpContext.Server.MapPath("../Content/UploadFiles/"), _uName, "Photos", "Topic");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var pathtemp = Path.Combine(path, fileName);//先存入临时文件夹

                file.SaveAs(pathtemp);
                Response.Write("<img style='max-height: 300px;max-width:500px;' src='../../Content/UploadFiles/" + _uName + "/Photos/Topic/" + fileName + "' /> ");
               // Response.Write("<img src='../../Content/TempFile/"+ fileName + "' /> ");
            }
            else
            {
                Response.Write(res);
            }
        }

        /// <summary>
        /// 删除预览中的照片
        /// </summary>
        /// <param name="str">这个str 已经是处理过的，不是之前上传的图片名称</param>
        public void DeleteImg(string str)
        {
            //删除地址 删除文件
            var list = Session["Imgscr"] as List<string>;
            if (list == null) return;
            var index = list.IndexOf(str);
            var slist = Session["ImgServerscr"] as List<string>;
            if (slist != null && index != -1)
            {
                var imgone = slist[index];
                if (imgone != null)
                {
                    var img = new FileInfo(imgone);
                    if (img.Exists) img.Delete();
                }
            }
            list.Remove(str);
        }

        private string CheckImg(HttpPostedFileBase file,bool ischecksame=true)
        {
            if (file == null) return "图片不能空！";
            if (file.ContentLength / 1024 > 8000)
            {
                return "图片太大";
            }
          
            var image = GetExtensionName(file.FileName).ToLower();
            if (image != ".bmp" && image != ".png" && image != ".gif" && image != ".jpg" && image != ".jpeg")// 这里你自己加入其他图片格式，最好全部转化为大写再判断，我就偷懒了
            {
                return "格式不对";
            }

            var scrtemp = Path.Combine("../../Content/TempFile/", file.FileName);//图片展示的地址
            if (ischecksame)
            {
                if (file.ContentLength / 1024 < 10)
                {
                    return "图片太小！";
                }

                var list = Session["Imgscr"] as List<string>;
                if (list != null && list.Find(n => n == scrtemp) != null)
                {
                    return "同样的照片已经存在！";
                }

            }
         
            return "ok";
        }

        public JsonResult SaveImgs(string content)
        {
            var uid = CheckValid();
            if (_uName == null) _uName = GetUserNameById(uid);

            string path = Path.Combine(HttpContext.Server.MapPath("../Content/UploadFiles/"), _uName, "Photos", "ImgBox");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var list = Session["Imgscr"] as List<string>;
            var finallist = new List<string>();
            if (list == null) return Json(0);
            foreach (var str in list)
            {
                var scrtemp1 = Server.MapPath(str.Substring(3, str.Length - 3));//去掉第一个../
                var img = new FileInfo(scrtemp1);
                if (img.Exists)
                {
                    var image = GetExtensionName(img.Name);
                    //处理照片名称
                    var imgname = string.Format("{0:yyyMMdd}", DateTime.Now).Replace("/", "") + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(7, 11) + image;
                    var scrdestination = Path.Combine(HttpContext.Server.MapPath("../Content/UploadFiles/"), _uName, "Photos", "ImgBox", imgname);
                 
                    var scrshow = Path.Combine(("../../Content/UploadFiles/"), _uName, "Photos", "ImgBox", imgname);
                    finallist.Add(scrshow);
                    //移动照片
                    img.MoveTo(scrdestination);
                    //存入imgbox
                    var box = new Iamgbox
                    {
                        ActionTime = DateTime.Now,
                        BoxName = _uName,
                        ImgUrl = scrshow,
                        IsValid = true, //默认是正规合适的图片 不合适在检举
                        PraiseCount = 0,
                        Remark = content,
                        UserId = uid,
                        VisitCount = 0,
                    };
                    LoveDb.Add(box);
                };
            }
            if (finallist.Count() != 0)
            {
                var sbstr = new StringBuilder("<br/><div class='imgtigger'>");
                foreach (var str in finallist)
                {
                    sbstr.Append("<img src='" + str + "' />");
                }
                sbstr.Append("</div>");
                var state = new State
                {
                    ActionTime = DateTime.Now,
                    Content = (content == "" ? string.Format("刚刚上传了{0}张照片:", finallist.Count) : content)+sbstr,
                    PraiseCount = 0,
                    StateType = StateType.Image.ToString(),
                    UserId = uid
                };
                LoveDb.Add(state);
                Session.Remove("Imgscr");
                Session.Remove("ImgServerscr");
            }
              

            return Json(1);
        }

        /// <summary>
        /// 存头像
        /// </summary>
        /// <returns></returns>
        public int SaveImg()
        {
            var uid = CheckValid();
            if (_uName == null) _uName = GetUserNameById(uid);
            //先创建文件夹目录
            string path = Path.Combine(HttpContext.Server.MapPath("../Content/UploadFiles/"), _uName, "Photos", "Portrait");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var list = Session["Imgscr"] as List<string>;
            if (list == null) return 0; //失败
            var scrtemp = list[0];//图片在临时文件夹的地址
            var scrtemp1 = Server.MapPath(scrtemp.Substring(3, scrtemp.Length - 3));//去掉第一个../
            var img = new FileInfo(scrtemp1);
            if (!img.Exists) return 0; //失败
            var image = GetExtensionName(img.Name);
            var imgname = string.Format("{0:yyyMMdd}", DateTime.Now).Replace("/", "") + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(7, 11) + image;
            var scrdestination = Path.Combine(HttpContext.Server.MapPath("../Content/UploadFiles/"), _uName, "Photos", "Portrait", imgname);
            var scrshow = Path.Combine(("../../Content/UploadFiles/"), _uName, "Photos", "Portrait", imgname);
            img.MoveTo(scrdestination);
            var dictinary = new Dictionary<string, object>
                {
                    {"ImgUrl", scrshow}
                };
            LoveDb.Update<User>(uid, dictinary);

            var imgbox = new Iamgbox
            {
                UserId = uid,
                ActionTime = DateTime.Now,
                BoxName = _uName,
                ImgUrl = scrshow,
                Remark = "我的头像",
                IsValid = true,
            };
            LoveDb.Add(imgbox);

            var state = new State
            {
                UserId = uid,
                ActionTime = imgbox.ActionTime,
                Content = "我刚刚更换了头像:<br/><div class='imgtigger'><img src='" + scrshow + "' /></div>",
                StateType = StateType.Image.ToString(),
            };
            LoveDb.Add(state);


            // 图片消息还没有处理呢。先放在这
            return 1;//成功
        }

        public string GetExtensionName(string fileName)
        {
            if (fileName.LastIndexOf("\\", StringComparison.Ordinal) > -1)//在不同浏览器下，filename有时候指的是文件名，有时候指的是全路径，所有这里要要统一。
            {
                fileName = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);//IndexOf 有时候会受到特殊字符的影响而判断错误。加上这个就纠正了。
            }
            return Path.GetExtension(fileName.ToLower());
        }

        #endregion

        #region 资料保存

        /// <summary>
        /// 修改基本资料
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult UpdateBaseInfo(BaseInfo info)
        {
            LoveDb.UpdateBaseInfo(CheckValid(), info);
            return Json(info);
        }
        /// <summary>
        /// 修改详细资料
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult UpdateDetailInfo(DetailInfo info)
        {
            LoveDb.UpdateDetail(CheckValid(), info);
            if (info.NativeCity == "选择城市")
            {
                info.NativeCity = "";
            }

            return Json(info);
        }
        /// <summary>
        /// 修改恋爱观
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public ActionResult UpdateLoveView(LoveView info)
        {
            LoveDb.UpdateLoveView(CheckValid(), info);
            return Json(info);
        }

        public ActionResult UpdateRequirement(RequirementClone info)
        {
            info.HightUl = info.HightUl == "不限" ? "0" : info.HightUl.Substring(0, 3);
            info.HightLl = info.HightLl == "不限" ? "0" : info.HightLl.Substring(0, 3);
            info.AgeUl = info.AgeUl == "不限" ? "0" : info.AgeUl.Substring(0, 2);
            info.MonthlyIncomeLl = info.MonthlyIncomeLl == "不限" ? "0" : info.MonthlyIncomeLl.Split('元')[0];
            info.MonthlyIncomeUl = info.MonthlyIncomeUl == "不限" ? "0" : info.MonthlyIncomeUl.Split('元')[0];
            info.Education = info.Education == "不限" ? "" : info.Education;

            var id = CheckValid();
            if (info.ResidenceCity == "选择城市")
            {
                info.ResidenceCity = "";
            }
            var requirement = new Requirement
                {
                    HightLl = info.HightLl == "0" ? 0 : Convert.ToInt16(info.HightLl.Substring(0, 3)),
                    HightUl = Convert.ToInt16(info.HightUl),
                    AgeLl = Convert.ToInt16(info.AgeLl.Substring(0, 2)),
                    AgeUl = Convert.ToInt16(info.AgeUl),
                    Education = info.Education,
                    MonthlyIncomeLl = Convert.ToInt16(info.MonthlyIncomeLl),
                    MonthlyIncomeUl = Convert.ToInt16(info.MonthlyIncomeUl),
                    ResidenceProvince = info.ResidenceProvince,
                    ResidenceCity = info.ResidenceCity,
                    UserId = id

                };
            LoveDb.UpdateRequirement(requirement);
            info.UserId = id;
            return Json(requirement);
        }

        /// <summary>
        /// 发布内心告白
        /// </summary>
        /// <param name="content"></param>
        /// <returns>返回0 说明发布时间太快，不过3分钟</returns>
        public JsonResult SendPersonalState(string content)
        {
            // 检查最后一条状态，不准发的太频繁。
            var laststate = LoveDb.LastOne((State s) => s.UserId == CheckValid());
            var min = LoveDb.DiffMinute(laststate.ActionTime, DateTime.Now);
            if (min <= 3)
            {
                return Json(0);
            }

            var state = new State
            {
                ActionTime = DateTime.Now,
                UserId = CheckValid(),
                Content = content,
                PraiseCount = 0,
                StateType = StateType.Personal.ToString(),
            };
            LoveDb.Add(state);
            return Json(content);
        }

        #endregion

        #region 登陆退出

        public ActionResult Logon()
        {
            if (CheckValid() != -1)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Logon(LogOnModel model)
        {
            var user = model.UserName.Contains("@") ? LoveDb.One(n => n.Email == model.UserName.Trim()) : LoveDb.One((User n) => n.UserName == model.UserName.Trim());
            if (user != null)
            {
                if (user.Enable == 0)
                {
                    return Json("账户已被封禁,不能登陆！");
                }

                var pd = Helpers.GetMd5Code(model.Password);
                if (pd == user.Password)
                {

                    Session["uid"] = user.UserId;
                    Session.Timeout = 600;
                    if (model.RememberMe)//存储密码及用户名
                    {
                        var httpCookie = Response.Cookies["fdpwd"];
                        if (httpCookie != null)
                        {
                            httpCookie.Value = pd;
                            httpCookie.Expires = DateTime.Now.AddDays(30);
                        }
                        var uidCookie = Response.Cookies["fduid"];//密码不用存吧？
                        if (uidCookie != null)
                        {
                            uidCookie.Value = user.UserId.ToString(CultureInfo.InvariantCulture);
                            uidCookie.Expires = DateTime.Now.AddDays(30);
                        }
                    }

                    LoveDb.LoginCountAdd(user.UserId);

                    return Json(user.UserId);
                }
                return Json("密码输错了吧~");
            }
            return Json("用户名或邮箱还未注册哦~");
        }

        /// <summary>
        /// 退出函数 还需要处理，退出时统计退出时间,然后关闭网页。
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Session.RemoveAll();
            Session.Abandon();
            var httpCookie = Response.Cookies["fdpwd"];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(0);
            var uidCookie = Response.Cookies["fduid"];
            if (uidCookie != null)
            {
                uidCookie.Expires = DateTime.Now.AddDays(0);
            }
            LoveDb.Logoff(CheckValid());
            return RedirectToAction("Index","Home");
        }

        #endregion

        #region 注册

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                    {
                        Email = model.Email,
                        Enable = 1,
                        ImgUrl = "../../Content/Photos/luren.jpg",
                        Password = Helpers.GetMd5Code(model.Password),
                        RegisterTime = DateTime.Now,
                        Sex = model.Sex,
                        UserName = model.UserName,
                        BrithDate = model.BrithDate,
                        Age = GetOld(model.BrithDate),
                        IsOpen = true,
                    };
                LoveDb.Add(user);
                var id = LoveDb.One((User u) => u.UserName == user.UserName).UserId;
                LoveDb.Add(new BaseInfo { UserId = id });
                LoveDb.Add(new Requirement { UserId = id, AgeLl = 18 });
                LoveDb.Add(new DetailInfo { UserId = id });
                LoveDb.Add(new UserHot { UserId = id, LogCount = 1, HotValue = 1 });
                LoveDb.Add(new Search { UserId = id });
                LoveDb.Add(new LastState { UserId = id });
                LoveDb.Add(new InfoStatistic
                {
                    UserId = id,
                    BaseInfoBase = 9,
                    BaseInfoReal = 0,
                    DetialsInfoBase = 9,
                    DetialsInfoReal = 0,
                    LoveViewsBase = 10,
                    LoveViewsReal = 0,
                    Percent = 0          // 基数按50算 24项基本资料 + 4张图片+18个问题
                });
                LoveDb.Add(new LoveView { UserId = id });
                Session["uid"] = user.UserId;
                Session.Timeout = 600;

                var httpCookie = Response.Cookies["fdpwd"];
                if (httpCookie != null)
                {
                    httpCookie.Value = model.Password;
                    httpCookie.Expires = DateTime.Now.AddDays(30);
                }
                var uidCookie = Response.Cookies["fduid"];
                if (uidCookie != null)
                {
                    uidCookie.Value = user.UserId.ToString(CultureInfo.InvariantCulture);
                    uidCookie.Expires = DateTime.Now.AddDays(30);
                }
                // 增加一个欢迎消息
                var sysMsg = new Message
                {
                    ActionTime = DateTime.Now,
                    Content = "欢迎来到意中人，祝你尽快找到幸福",
                    FromUserId = 1,
                    FromUserName = "意中人管理员",
                    IsReaded = false,
                    MegType = MegType.System,
                    StateId = id,
                    StateType = StateType.None,
                    ToUserId = id,
                };
                LoveDb.Add(sysMsg);

                SystemSendMail(DefaultAdminMailAdress, "新人注册", "有新人注册，速度审核");

                return RedirectToAction("Index", "User");
            }

            return View();
        }
        #endregion

        #region 验证及辅助方法

        public string GetRequirStr(Requirement re)
        {
            var sb = new StringBuilder("我正在看:<span>");
            if (re != null)
            {
                //居住地
                if (re.ResidenceProvince != null)
                {
                    sb.Append(re.ResidenceProvince + re.ResidenceCity + ",");
                }
                //年龄
                if (re.AgeLl != 0)
                {
                    sb.Append(re.AgeUl != 0 ? re.AgeLl + "到" + re.AgeUl + "岁," : re.AgeLl + "岁以上,");
                }
                //身高
                if (re.HightLl != 0)
                {
                    sb.Append(re.HightUl == 0 ? re.HightLl + "厘米以上" : re.HightLl + "到" + re.HightUl + "厘米");
                }
            }
            else
            {
                sb.Append("18岁以上");
            }
            sb.Append(GetMyself().User.Sex == "man" ? " 的女生</span>" : "的男生</span>");
            return sb.ToString();
        }

        public string GetLastState(int? id)
        {
            if (id == null) id = CheckValid();
            var state = LoveDb.LastOne((State n) => n.UserId == id&&n.StateType==StateType.Personal.ToString());
            if (state != null && !string.IsNullOrEmpty(state.Content))
            {
                return state.Content;
            }
            return "让那个意中人,听到你的心声吧!";
        }

        /// <summary>
        /// 为前台脚本提供数据 用于对象初始化
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBaseInfo()
        {
            var id = CheckValid();
            var bs = LoveDb.One((BaseInfo n) => n.UserId == id);
            return Json(bs);
        }

        /// <summary>
        /// 获取详细资料 数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDetailInfo()
        {
            var id = CheckValid();
            var bs = LoveDb.One((DetailInfo n) => n.UserId == id);
            return Json(bs);
        }

        /// <summary>
        /// 获取恋爱观数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLoveView()
        {
            var id = CheckValid();
            var bs = LoveDb.One((LoveView n) => n.UserId == id);
            return Json(bs);
        }

        public JsonResult GetRequirement()
        {
            var id = CheckValid();
            var requirement = LoveDb.One((Requirement n) => n.UserId == id);
            return Json(requirement);
        }




        /// <summary>
        /// 赞的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PariseHelloView()
        {
            var userid = CheckValid();
            var megs = LoveDb.MessageAll().Where(n => n.MegType == MegType.Praise && n.ToUserId == userid).OrderByDescending(a => a.ActionTime).ToList();
            var pariseHelloUsers = new List<PariseHelloUser>();
            foreach (var message in megs)
            {
                string content = ""; Message message1 = message;
                var state = LoveDb.One((State s) => s.Id == message1.StateId);
                switch (message.StateType)
                {
                    case StateType.None:
                        content = message.Content;
                        break;
                    case StateType.Personal:
                        if (state != null)
                        {
                            content = "赞了你发布的状态：" + state.Content;
                            //? "赞了你发布的状态" + "<div class='statediv'>" + state.Content.Substring(0, 36) + "...</div>"
                            //: "赞了你发布的状态" + "<div class='statediv'>" + state.Content + "</div>";
                        }
                        else
                        {
                            content = "该状态已经删除...";
                        }
                        
                        break;
                    case StateType.Image:
                        content = state != null ? state.Content : "该状态已经删除...";
                        break;
                }
                if (!message.IsReaded)
                {
                    LoveDb.ReadMessage<Message>(message.Id);
                }

                var mm = new PariseHelloUser
                {
                    UninUser = LoveDb.GetUninUser(message.FromUserId),
                    Message = message,
                    Content = content,
                };
                pariseHelloUsers.Add(mm);
            }

            return View(pariseHelloUsers);
        }

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public JsonResult CheckUserName(string username)
        {
            var result = !(LoveDb.UserAll().Where(n => n.UserName == username.Trim())).Any();
            result = result && username.Length < 10;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查是否有同名邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult CheckMail(string email)
        {
            var result = !(LoveDb.UserAll().Where(n => n.Email == email.Trim())).Any() && email.Contains("deltaww.com.cn");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查年龄是否合格
        /// </summary>
        /// <param name="brithdate"></param>
        /// <returns></returns>
        public JsonResult CheckBrithDate(DateTime brithdate)
        {
            var year = GetOld(brithdate);
            var result = (year >= 15 && year < 55);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="brithdate"></param>
        /// <returns></returns>
        public int GetOld(DateTime brithdate)
        {
            var year = DateTime.Now.Year - brithdate.Year;
            var month = DateTime.Now.Month - brithdate.Month;
            var day = DateTime.Now.Day - brithdate.Day;
            if (month < 0)
            {
                year = year - 1;
            }
            else if (month == 0)
            {
                if (day < 0)
                {
                    year = year - 1;
                }
            }
            return year;
        }

        #endregion


    }
}
