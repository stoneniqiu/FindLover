using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Findlover.Helper;
using Findlover.Models;
using Findlover.ViewModel;

namespace Findlover.Controllers
{
    public class AdminController : BaseController
    {

        #region 视图
        //
        // GET: /Admin/
        /// <summary>
        /// 判断管理员身份 加载不同资料
        /// </summary>
        /// <param name="type">检索的类型 0表示全部,1表示仅图片,2表示仅仅资料,3表示仅男生资料,4表示仅女生资料,5男生图片,6女生图片</param>
        /// <returns></returns>
        public ActionResult Index(int? type)
        {
            var indextype = 0;
            if (type != null)
            {
                ViewData["type"] = type;
                indextype = (int)type;
            }
            else
            {
                if (ViewData["type"] != null) indextype = Convert.ToInt32(ViewData["type"]);
                else
                {
                    ViewData["type"] = 0;
                }
            }
            var id = CheckValid();

            var role = LoveDb.One(n => n.UserId == id && (n.RoleType == RoleType.SysAdmin.ToString() || n.RoleType == RoleType.Admin.ToString()));
            if (role != null)//是管理员就可以
            {
                ViewBag.ImgCount = CheckedImgCount();
                ViewBag.InfoCount = Checkinfocout();
                var data = new AdminViewModel
                    {
                        Admin = GetAdmin(role),
                        UncheckedUsers = GetUncheckUsers(indextype)
                    };
                return View(data);

            }


            return RedirectToAction("SorryView");

        }

        public JsonResult IsAdmin()
        {
            var id = CheckValid();
            return Json(LoveDb.One(
                n =>
                    n.UserId == id &&
                    (n.RoleType == RoleType.SysAdmin.ToString() || n.RoleType == RoleType.Admin.ToString())) != null ? 1 : 0);
        }


        public ActionResult SysAdmin()
        {
            var id = CheckValid();
            var role = LoveDb.One(n => n.UserId == id && n.RoleType == RoleType.SysAdmin.ToString());
            if (role != null) //是管理员就可以
            {
                var adminlist = GetAllAdmins(id);
                return View(adminlist);
            }
            return RedirectToAction("SorryView");
        }

        /// <summary>
        /// 没有权限跳转到遗憾页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SorryView()
        {
            return View();
        }

        /// <summary>
        /// 解除封禁的视图,当前被封禁人员
        /// </summary>
        /// <returns></returns>
        public ActionResult ForbidView()
        {
            var users = LoveDb.UserAll().Where(n => n.Enable == 0).ToList();
            return View(users);
        }

        //举报管理
        public ActionResult ReportAdmin(int? pageIndex)
        {
            List<Report> reports;
            switch (pageIndex)
            {
                case null:
                case 0://未处理 默认是未处理
                    reports = LoveDb.ReportAll().Where(n => n.IsDone == false).ToList();
                    break;
                case 1: //已经处理
                    reports = LoveDb.ReportAll().Where(n => n.IsDone).ToList();
                    break;
                case 2:
                    reports = LoveDb.ReportAll();
                    break;
                default:
                    reports = LoveDb.ReportAll().Where(n => n.IsDone == false).ToList();
                    break;
            }



            return View(reports);
        }

        /// <summary>
        /// 添加举报信息
        /// </summary>
        /// <param name="reportedId">被举报者Id</param>
        /// <param name="reportedName">被举报者姓名</param>
        /// <param name="description">描述</param>
        /// <param name="isReport">是否举报 因为拉黑优先</param>
        /// <param name="relateId">相关Id</param>
        /// <param name="reportType">举报类型（是举报人 还是举报私信 状态这些）</param>
        /// <param name="messageType">举报信息类型（举报信息是色情 骚扰还是什么）</param>
        /// <returns></returns>
        public JsonResult ReportDeal(int reportedId, string reportedName, string description, bool isReport, int relateId, string reportType, string messageType)
        {
            var id = CheckValid();
            var name = GetUserNameById(id);

            //重复举报判断
            if (LoveDb.One(r => r.ReportType == GetReportType(reportType) && r.RelateId == relateId) != null)
            {
                return Json("2");
            }

            //处理拉黑。 取消两者的喜欢关系。加入黑名单 黑名单还么有创建 - -！
            //取消喜欢关系 是多个
            if (reportedId != id && GetSex(id) != GetSex(reportedId))
            {
                var loveone = LoveDb.MyLoveAll().Where(n => (n.UserId == id && n.LoverId == reportedId) || (n.LoverId == id && n.UserId == reportedId));
                foreach (var myLove in loveone)
                {
                    LoveDb.Delete<MyLove>(myLove.Id);
                }
                //加入黑名单
                var dislike = LoveDb.DisLoveAll().SingleOrDefault(n => (n.UserId == id && n.DisLoveId == reportedId));
                if (dislike == null)
                {
                    dislike = new DisLove
                    {
                        ActionTime = DateTime.Now,
                        DisLoveId = reportedId,
                        UserId = id,
                    };
                    LoveDb.Add(dislike);
                }
            }

            //举报处理
            if (isReport)
            {
                var report = new Report
                {
                    ActionTime = DateTime.Now,
                    Description = description,
                    RelateId = relateId,
                    ReportedUserId = reportedId,
                    ReportedUserName = reportedName,
                    UserId = id,
                    UserName = name,
                    ReportType = GetReportType(reportType),
                    MessageType = GetMessageType(messageType)
                };

                LoveDb.Add(report);
            }

            SystemSendMail(DefaultAdminMailAdress, "信息举报", "有举报，速度处理");
            //信息，mail 通知管理员。
            return Json("1");//1 成功 2表示举报过
        }


        /// <summary>
        /// 管理员处理，是禁止还是警告这个人。
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult ReportUser(int userId)
        {
            var user = LoveDb.GetUninUser(userId);
            var reports = LoveDb.ReportAll().Where(n => n.ReportedUserId == userId).ToList();
            var total = reports.Count();
            var undone = reports.Count(n => n.IsDone == false);
            var sreport = new List<SimpleReport>();
            foreach (var report in reports.Where(n => n.IsDone == false))
            {
                var sre = new SimpleReport { ReportRaw = report };
                var report1 = report;
                switch (report.ReportType)
                {
                     
                    case ReportType.Personal:
                        sre.Content = "直接举报本人";
                        break;
                    case ReportType.Message:
                        var c = LoveDb.One((Message m) => m.Id == report1.RelateId);
                        sre.Content = "私信举报:";
                        if (c != null)
                        {
                            sre.Content = c.Content.Length < 20 ? c.Content : c.Content.Substring(0, 16) + "...";
                        }
                        else
                        {
                            sre.Content = "相关信息已经被删除!";
                        }
                        break;
                    case ReportType.Topic:
                        sre.Content = "话题举报:";
                        var top = LoveDb.One((Topic m) => m.Id == report1.RelateId);
                        if (top != null)
                        {
                            sre.Content += string.Format("<a href='/Interactive/Detail?topicId={0}'>{1}</a>", top.Id,
                                top.Title);
                        }
                        else
                        {
                            sre.Content = "相关信息已经被删除!";
                        }
                        break;
                    case ReportType.Comment:
                        sre.Content = "评论举报:";
                        var com = LoveDb.One((Comment m) => m.Id == report1.RelateId);
                        if (com != null)
                        {
                            sre.Content += com.Content;
                        }
                        else
                        {
                            sre.Content = "相关信息已经被删除!";
                        }
                        break;
                    default:
                        sre.Content = "管理员还未处理此类型";
                        break;
                }
                sreport.Add(sre);
            }
            var rdu = new ReportDetailUser
            {
                UninUser = user,
                Reports = sreport,
                Sum = new ReportSum
                {
                    ReportedNum = total,
                    UnDone = undone,
                    Done = total - undone
                }
            };

            return PartialView(rdu);
        }

        /// <summary>
        /// 处理举报
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult DealReport(int reportId)
        {
            // 设置为done
            var report = LoveDb.One((Report r) => r.Id == reportId);
            if (report == null || report.IsDone)
            {
                return null;
            }
            LoveDb.DoneReport(reportId);
            //发给举报人 说信息已经处理 如果已经处理或者不存在就返回
            var id = CheckValid();
            var msg = new Message
            {
                ActionTime = DateTime.Now,
                Content = string.Format("你在{0}举报{1}的信息,管理员已经处理,谢谢你的监督！", report.ActionTime, report.ReportedUserName),
                FromUserId = id,
                FromUserName = "意中人管理员",
                IsReaded = false,
                ToUserId = report.UserId,
                MegType = MegType.System,
                StateType = StateType.None,
            };
            LoveDb.Add(msg);

            //记录管理员 处理的情况
            var log = new ReportLog
            {
                ActionTime = DateTime.Now,
                AdminId = id,
                ReportId = reportId,

            };
            LoveDb.Add(log);

            return Json("1");
        }

        #endregion


        #region 前台调用方法

        public JsonResult ValidImgOrInfo(int userid, bool result, int type)
        {
            //写入真正的结果 是通过还是未通过  需要后台办法支持！ 这样修改无效
            var r = LoveDb.CheckImgOrInfo(userid, result, type);
            //写入管理员的操作记录, 并统计管理的操作数
            LoveDb.AdminStatistics(CheckValid(), type);
            //返回后 隐藏栏目
            Logger.Trace(GetUserNameById(CheckValid()) + "管理员验证资料，用户" + GetUserNameById(userid) + "类型" + type + "审核结果" + result);
            return Json(r);
        }

        /// <summary>
        /// 封号
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult ForbidUser(int userid)
        {
            if (LoveDb.One((User e) => e.UserId == userid).Enable == 0)
            {
                return Json(0);//已经被封禁过
            }

            LoveDb.ForbidUser(userid);
            //通知此人
            var msg = new Message
            {
                ActionTime = DateTime.Now,
                Content = string.Format("由于你的非法操作和举报信息,你的号已经被系统封禁,该账户功能会被大量削弱！如有问题请联系管理员"),
                FromUserId = CheckValid(),
                FromUserName = "意中人管理员",
                IsReaded = false,
                ToUserId = userid,
                MegType = MegType.System,
                StateType = StateType.None,
            };
            LoveDb.Add(msg);

            Logger.Trace(GetUserNameById(userid) + "被管理员：" + GetUserNameById(CheckValid()) + "封号");

            return Json(1);//操作成功
        }

        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult UnForbidUser(int userid)
        {
            if (LoveDb.One((User e) => e.UserId == userid).Enable == 1)
            {
                return Json(0);//已经被封禁过
            }

            LoveDb.UnForbidUser(userid);
            //通知此人
            var msg = new Message
            {
                ActionTime = DateTime.Now,
                Content = string.Format("你的账户已经解封，祝你愉快。"),
                FromUserId = CheckValid(),
                FromUserName = "意中人管理员",
                IsReaded = false,
                ToUserId = userid,
                MegType = MegType.System,
                StateType = StateType.None,
            };
            LoveDb.Add(msg);

            Logger.Trace(GetUserNameById(userid) + "被管理员：" + GetUserNameById(CheckValid()) + "解封");
            return Json(1);//操作成功
        }
        public JsonResult GetImgCount()
        {
            return Json(CheckedImgCount());
        }

        public JsonResult GetInfoCount()
        {
            return Json(Checkinfocout());
        }

        /// <summary>
        /// 添加管理员，此处还可以扩展加为小编等其他角色
        /// </summary>
        /// <param name="username"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult AddAdmin(string username, string type)
        {
            var user = LoveDb.One((User u) => u.UserName == username);//先判断有无这个人
            string str;
            if (user != null)
            {
                //再判断这个人有没有加入Role
                var o = LoveDb.One(r => r.UserId == user.UserId && r.RoleType == type);
                if (o != null)
                {
                    str = "该用户已经是" + type;
                }
                else
                {
                    var role = new Role(type, user.UserId);
                    LoveDb.Add(role);
                    var ads = LoveDb.AdminStatisticAll().SingleOrDefault(n => n.UserId == user.UserId);

                    if (ads == null)
                    {
                        var amdst = new AdminStatistic
                        {
                            UserId = user.UserId,
                            CheckImgCount = 0,
                            CheckInfoCount = 0,
                            LastLogin = DateTime.Now,
                            SignInDays = 0,
                            Experience = 0,
                            Gold = 0
                        };
                        LoveDb.Add(amdst);
                    }

                    str = "ok";
                    Logger.Trace("管理员：" + GetUserNameById(CheckValid()) + "添加了" + user.UserName + "为" + type);
                }

            }
            else
            {
                str = "该用户名不存在";
            }
            return Json(str);
        }

        public JsonResult DeleteAdmin(int userid, string admintype)
        {
            var num = LoveDb.RoleAll().Count(n => n.UserId == userid);
            if (num == 1)
            {
                var admins = LoveDb.AdminStatisticAll().SingleOrDefault(n => n.UserId == userid);
                if (admins != null)
                {
                    LoveDb.Delete<AdminStatistic>(admins.Id);
                }
            }
            var rs = LoveDb.One(r => r.UserId == userid && r.RoleType == admintype);//如果他有多重角色呢？ 对应的权限是否也要删除？
            if (rs != null) LoveDb.Delete<Role>(rs.Id);

            Logger.Trace("管理员：" + GetUserNameById(CheckValid()) + "删除了用户" + GetUserNameById(userid) + "的" + admintype);
            return Json("ok");
        }

        public Admin GetAdmin(Role role)
        {
            var user = LoveDb.One((User n) => n.UserId == role.UserId);
            return new Admin
            {
                Role = role,
                ImgUrl = user.ImgUrl,
                UserName = user.UserName
            };
        }

        /// <summary>
        /// 获取所有身份或者图片没有审核通过的人
        /// </summary>
        /// <returns></returns>
        /// 检索的类型 0表示全部,1表示仅图片,2表示仅仅资料,3表示仅男生资料,4表示仅女生资料,5男生图片,6女生图片 
        public List<User> GetUncheckUsers(int type)
        {
            List<User> db = LoveDb.UserAll();
            switch (type)
            {
                case 0:
                    db = db.Where(n => (n.IsVerified == false && n.IsChecked == false) || (n.IsVerifiedImg == false && n.IsCheckedImg == false)).ToList();
                    break;
                case 1:
                    db = db.Where(n => (n.IsVerifiedImg == false && n.IsCheckedImg == false)).ToList();
                    break;
                case 2:
                    db = db.Where(n => (n.IsVerified == false && n.IsChecked == false)).ToList();
                    break;
                case 3:
                    db = db.Where(n => (n.IsVerified == false && n.IsChecked == false && n.Sex == "man")).ToList();
                    break;
                case 4:
                    db = db.Where(n => (n.IsVerified == false && n.IsChecked == false && n.Sex == "woman")).ToList();
                    break;
                case 5:
                    db = db.Where(n => (n.IsVerifiedImg == false && n.IsCheckedImg == false && n.Sex == "man")).ToList();
                    break;
                case 6:
                    db = db.Where(n => (n.IsVerifiedImg == false && n.IsCheckedImg == false && n.Sex == "woman")).ToList();
                    break;
                case 7:
                    db = db.Where(n => n.IsChecked || n.IsCheckedImg).ToList();
                    break;
                default:
                    db = db.Where(n => (n.IsVerified == false && n.IsChecked == false) || (n.IsVerifiedImg == false && n.IsCheckedImg == false)).ToList();
                    break;

            }
            return db;
        }

        /// <summary>
        /// 获取所有管理员，除了操作者本身
        /// </summary>
        /// <returns></returns>
        public List<Admin> GetAllAdmins(int id)
        {
            // var roles = LoveDb.RoleAll().Where(n => n.UserId != id).OrderBy(n=>id).ToList();
            var roles = LoveDb.RoleAll();
            return roles.Select(GetAdmin).ToList();//可以简化成这样！
        }

        /// <summary>
        /// 获取所以用户姓名
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllUserName()
        {
            var users = LoveDb.UserAll();
            return Json(users.Select(user => user.UserName).ToList());
        }

        #endregion


        #region 私有方法
        private int CheckedImgCount()
        {
            return LoveDb.One((AdminStatistic n) => n.UserId == CheckValid()).CheckImgCount;
        }

        private int Checkinfocout()
        {
            return LoveDb.One((AdminStatistic n) => n.UserId == CheckValid()).CheckInfoCount;
        }



        private ReportType GetReportType(string type)
        {
            switch (type)
            {
                case "personal":
                    return ReportType.Personal;
                case "message":
                    return ReportType.Message;
                case "image":
                    return ReportType.Image;
                case "state":
                    return ReportType.State;
                case "topic":
                    return ReportType.Topic;
                case "comment":
                    return ReportType.Comment;
                default:
                    return ReportType.Personal;
            }

        }

        private ReportMessageType GetMessageType(string type)
        {
            switch (type)
            {
                case "色情污秽":
                    return ReportMessageType.Pornographic;
                case "资料虚假或冒用":
                    return ReportMessageType.FakeMssage;
                case "骚扰":
                    return ReportMessageType.Harass;
                case "广告":
                    return ReportMessageType.Advertisement;
                case "诈骗或虚假信息":
                    return ReportMessageType.Swindle;
                case "其他理由":
                    return ReportMessageType.Others;
                default:
                    return ReportMessageType.None;
            }
        }

        #endregion
    }
}
