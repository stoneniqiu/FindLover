using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Findlover.Helper;
using Findlover.Models;
using Findlover.ViewModel;

namespace Findlover.Controllers
{
    public class BaseController : Controller
    {
        // GET: /User/
        public readonly LoveDbRepository LoveDb = LoveDbRepository.GetIntance();
        //
        // GET: /Recommend/
        private UninUser _meUninUser;
        public string DefaultAdminMailAdress = "yourmail@163.com.cn";
        private int _userId;
        /// <summary>
        /// 检查用户id是否存在，先判断session 再判断cook
        /// </summary>
        /// <returns></returns>
        public int CheckValid()
        {
            if (_userId != 0)
            {
                return _userId;
            }

            if (Session["uid"] != null)
            {
                _userId=Convert.ToInt16(Session["uid"].ToString());
                return _userId;
            }
            var cook = Request.Cookies["fduid"];
            if (cook != null)
            {
                var uid = Convert.ToInt16(cook.Value);
                Session["uid"] = uid;
                Session.Timeout = 600;
             //   Logger.Debug("用户id:"+uid+"登陆了");
                LoveDb.LoginCountAdd(uid);
                _userId = uid;
                return uid;
            }
            return -1;
        }

        public UninUser GetMyself()
        {
            var id = CheckValid();
            if (id != -1)
            {
                return _meUninUser ?? (_meUninUser = LoveDb.GetUninUser(CheckValid()));
            }

            return null;
        }

        /// <summary>
        /// 通过id获取用户姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserNameById(int id)
        {
            return _meUninUser != null ? _meUninUser.User.UserName : LoveDb.One((User r) => r.UserId == id).UserName;
        }

        /// <summary>
        /// 获取基本的异性集合 可以允许未审核图片的人进来！ && n.IsVerifiedImg
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected IEnumerable<User> GetBaseUsers(int id)
        {
            var lz = LoveDb.One((User n) => n.UserId == id);
            var dislikelist = GetDislikeList();//过滤的名单。 过掉我不喜欢的人。  //还要过掉不喜欢我的人？
          
            if (lz != null)
            {
                //选出所有异性中图片审核过的，资料审核过的人,且资料开放，没有被禁止的人
                return
                    LoveDb.UserAll()
                          .Where(n => n.Sex != lz.Sex  && n.IsVerified && n.IsOpen && n.Enable == 1&&!dislikelist.Contains(n.UserId))
                          .ToList();
            }
            return null;
        }

        public JsonResult CheckUser()
        {
            var self = GetMyself();
            var check=new CheckUser();
            if (self != null)
            {

                check.IsImgOk = self.User.IsVerifiedImg;
                check.IsInfoOk = self.User.IsVerified;
                check.IsPersentOk = (GetPersent(self.User.UserId) >= 0.3);
                check.IsLogon = true;
              
                check.IsOk = check.IsImgOk && check.IsPersentOk && check.IsInfoOk;
            }
            else
            {
                check.IsLogon = false;
                check.IsOk = false;
            }

            return Json(check);
        }



        public JsonResult GetUnReadMessageSum()
        {
            var id = CheckValid();
            var msglist = LoveDb.MessageAll().Where(n => n.ToUserId == id&&!n.IsReaded).ToList();
            var ur = new UnReadMessageSum
            {
                Praises = msglist.Count(n => n.MegType == MegType.Praise),
                Private = msglist.Count(n => n.MegType == MegType.Private),
                System = msglist.Count(n => n.MegType == MegType.System),
                LoveMe = LoveDb.MyLoveAll().Count(n => n.LoverId == id && !n.IsRead ),
                Visitor = LoveDb.VisitorAll().Count(n => n.UserId == id && !n.IsRead),
            };
            ur.Total = ur.LoveMe + ur.Praises + ur.Private + ur.System + ur.Visitor;
            return Json(ur);
        }

        /// <summary>
        /// 获取我不喜欢的人列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetDislikeList()
        {
            var id = CheckValid();
            var list= LoveDb.DisLoveAll().Where(n => n.UserId == id).Select(n => n.DisLoveId).ToList();
            var listme = LoveDb.DisLoveAll().Where(n => n.DisLoveId == id).Select(n => n.UserId).ToList();
            list.AddRange(listme);
            return list ;
        } 

        /// <summary>
        /// 计算资料完成度 三项资料完整度+照片数(是否大于4)+回答问题数or其它
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public double GetPersent(int? uid)
        {
            if (uid == null)
                uid = CheckValid();
            return Math.Round(LoveDb.GetPercent((int)uid), 2);// 还未加上回答问题数
        }

        public bool PartMathUser(Requirement condition, UninUser biguser)
        {
            var residenceflag = false;
            var ageflag = false;
            var hightflag = false;

            if (string.IsNullOrEmpty(condition.ResidenceCity)||(biguser.BaseInfo.ResidenceCity != null && biguser.BaseInfo.ResidenceCity.Contains(condition.ResidenceCity)) )
            {
                residenceflag = true;
            }

            //年龄 
            if ((condition.AgeLl == 0 || (biguser.User.Age != 0 && biguser.User.Age <= condition.AgeUl && biguser.User.Age >= condition.AgeLl))//agelow 要比 ageup 大
                || (condition.AgeUl == 0 && biguser.User.Age >= condition.AgeLl))
            {
                ageflag = true;
            }

            //身高
            int hight = Convert.ToInt32(biguser.BaseInfo.Height.Split('厘')[0]);
            if (condition.HightLl == 0 || (biguser.BaseInfo.Height != null && ((hight <= condition.HightUl && hight >= condition.HightLl && condition.HightUl != 0)
                || (condition.HightUl == 0 && hight >= condition.HightLl))))
            {
                hightflag = true;
            }
            return hightflag && residenceflag && ageflag;
        }
      

        /// <summary>
        /// 用户热度，相互符合度，生活观契合度
        /// </summary>
        /// <param name="selfid">本人</param>
        /// <param name="otherid">比较者</param>
        /// <returns></returns>
        public RecommendRate GetRecommendRate(int selfid, int otherid)
        {
            var usersum = LoveDb.UserAll().Count;
            //得到两个人的需求表 来计算相互符合度
            var myre = LoveDb.One((Requirement r) => r.UserId == selfid);
            var youre = LoveDb.One((Requirement r) => r.UserId == otherid);
            var yourUnin = LoveDb.GetUninUser(otherid);
            var me = GetMyself();
            var rr = new RecommendRate
            {
                //用户热度排名UserHotRate
                UserHotRate = (double)(usersum - LoveDb.UserHotAllDes().FindIndex(n => n.UserId == otherid)) / usersum,
                ForMeRate = TomeRate(yourUnin, myre),
                ForOtherRate = TomeRate(me, youre),
                LoveViwRate = GetLoveViewRate(yourUnin, me)
            };
            rr.TotalRate = (rr.UserHotRate + rr.LoveViwRate + rr.ForMeRate + rr.ForOtherRate) / 4;

            return rr;
        }
        /// <summary>
        /// 对比恋爱观
        /// </summary>
        /// <param name="user"></param>
        /// <param name="me"></param>
        /// <returns></returns>
        private double GetLoveViewRate(UninUser user, UninUser me)
        {
            var loveInts = GetLoveInts(user, me);
            return (double)loveInts.Sum() / 10;
        }

         public int[] GetLoveInts(UninUser user, UninUser me)
        {
//工作安排
            var loveInts = new int[10];
            if (Workplan(user.LoveView.WorkTimePlan, me.LoveView.WorkTimePlan))
            {
                loveInts[0] = 1;
            }
            // 抽烟
            if (user.LoveView.Smoking != null && me.LoveView.Smoking != null)
            {
                var ostr = user.LoveView.Smoking;
                var istr = me.LoveView.Smoking;
                if (ostr == "不吸烟,但也不介意" || (ostr == "不吸烟,而且反感吸烟" && (istr == "不吸烟,但也不介意" || istr == "不吸烟,而且反感吸烟"))
                    || ((ostr == "经常偶尔吸烟" || ostr == "经常吸烟") && istr == "不吸烟,但也不介意"))
                {
                    loveInts[1] = 1;
                }
            }
            //喝酒
            if (user.LoveView.Drinking != null && me.LoveView.Drinking != null)
            {
                var ostr = user.LoveView.Drinking;
                var istr = me.LoveView.Drinking;
                if (ostr == "不喝酒,但也不介意" || (ostr == "不喝酒,且很反感喝酒" && (istr == "不喝酒,但也不介意" || istr == "不喝酒,且很反感喝酒"))
                    || ((ostr == "偶尔社交需要才喝酒" || ostr == "经常喝酒") && istr == "不喝酒,但也不介意"))
                {
                    loveInts[2] = 1;
                }
            }
            //恋爱多久结婚
            if (user.LoveView.LoveDuration != null && me.LoveView.LoveDuration != null &&
                user.LoveView.LoveDuration == me.LoveView.LoveDuration)
            {
                loveInts[3] = 1;
            }
            //婚后是否要小孩
            if (user.LoveView.WantaBaby != null && me.LoveView.WantaBaby != null &&
                user.LoveView.WantaBaby == me.LoveView.WantaBaby)
            {
                loveInts[4] = 1;
            }
            //婚后是否与父母同住
            if (user.LoveView.ParentLiveTogether != null && me.LoveView.ParentLiveTogether != null &&
                user.LoveView.ParentLiveTogether == me.LoveView.ParentLiveTogether)
            {
                loveInts[5] = 1;
            }
            //家务事
            if (user.LoveView.Housework != null && me.LoveView.Housework != null &&
                user.LoveView.Housework == me.LoveView.Housework)
            {
                loveInts[6] = 1;
            }
            //理财
            if (user.LoveView.ManageMoney != null && me.LoveView.ManageMoney != null &&
                user.LoveView.ManageMoney == me.LoveView.ManageMoney)
            {
                loveInts[7] = 1;
            }
            //厨艺
            if (user.LoveView.Cooking != null && me.LoveView.Cooking != null)
            {
                var ostr = user.LoveView.Cooking;
                var istr = me.LoveView.Cooking;
                if ((ostr == "会做饭,希望对方也会" && (istr == "会做饭,对另一半没要求" || istr == "会做饭,希望对方也会"))
                    || ostr == "会做饭,对另一半没要求" ||
                    (ostr == "不太会,对另一半没要求" && istr != "会做饭,希望对方也会" && istr != "不太会,希望对方厨艺比我好")
                    || (ostr == "不太会,希望对方厨艺比我好" && istr != "不太会,对另一半没要求" && istr != "不太会,希望对方厨艺比我好"))
                {
                    loveInts[8] = 1;
                }
            }

            //异地
            if (user.LoveView.Allopatry != null && me.LoveView.Allopatry != null &&
                user.LoveView.Allopatry == me.LoveView.Allopatry)
            {
                loveInts[9] = 1;
            }
            return loveInts;
        }


        private bool Workplan(string other, string mine)
        {
            if (other != null && mine != null)
            {
                if (other == "工作时间固定,不介意对方出差" || (other == "工作时间固定,不接受对方出差" && (mine == "工作时间固定,不接受对方出差" || mine == "工作时间固定,不介意对方出差"))
                    || ((other == "工作经常出差" || other == "工作偶尔出差") && mine != "工作时间固定,不接受对方出差"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// first和second比较，second为主体  到时候要改成返回对象 
        /// </summary>
        /// <param name="user">被比较者的Id</param>
        /// <param name="second">比较着的资料</param>
        /// <returns></returns>
        private double TomeRate(UninUser user, Requirement second)
        {
            var tomeInts = GetTomeInts(user, second);

            return (double)tomeInts.Sum()/ 5;
        }

        /// <summary>
        /// 两者需求匹配度
        /// </summary>
        /// <param name="user"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public int[] GetTomeInts(UninUser user, Requirement second)
        {
            var firstage = user.User.Age;
            var high =string.IsNullOrEmpty(user.BaseInfo.Height)?0:Convert.ToInt16(user.BaseInfo.Height.Split('厘')[0]);

            var tomeInts = new int[5];

            //没有年龄限制，或者在second的要求范围内，Ageui=0表示是没有上限。
            if (second.AgeLl == 0 || (second.AgeLl <= firstage && (second.AgeUl == 0 || second.AgeUl >= firstage)))
            {
                tomeInts[0] = 1;
            }
          
            // 地方
            if (string.IsNullOrEmpty(second.ResidenceProvince) ||
                (second.ResidenceProvince == user.BaseInfo.ResidenceProvince &&
                 (second.ResidenceCity == user.BaseInfo.ResidenceCity || string.IsNullOrEmpty(second.ResidenceCity))))
            {
                tomeInts[1] = 1;
            }
            //身高
            if (second.HightLl == 0 || (second.HightLl <= high && (second.HightUl == 0 || second.HightUl >= high))||high==0)
            {
                tomeInts[2] = 1;
            }

            //学历 null 大专 本科 硕士 博士 /不限 大专以下 大专及以上  本科+  硕士+ 博士+
            if (string.IsNullOrEmpty(second.Education) ||
                (ChangEducationToNum(second.Education) <= ChangEducationToNum(user.BaseInfo.Education) && !string.IsNullOrEmpty(user.BaseInfo.Education)))
            {
                tomeInts[3] = 1;
            }

            //月收入 先算出中间值。
            int mm=0;
            if (!string.IsNullOrEmpty(user.BaseInfo.MonthlyIncome))
            {
                mm = user.BaseInfo.MonthlyIncome == "3000元以下" ? 2500 : Convert.ToInt16(user.BaseInfo.MonthlyIncome.Split('-')[0]) + 500;
            }
        
               

            if (second.MonthlyIncomeLl == 0 ||
                (second.MonthlyIncomeLl < mm || (second.MonthlyIncomeUl == 0 || second.MonthlyIncomeUl > mm)) || mm==0)
            {
                tomeInts[4] = 1;
            }
            return tomeInts;
        }

        /// <summary>
        /// 比较学历高低
        /// </summary>
        /// <param name="edu"></param>
        /// <returns></returns>
        public int ChangEducationToNum(string edu)
        {
            switch (edu)
            {
                case "":
                case "大专以下":
                    return -1;
                case "大专":
                case "大专以上":
                    return 0;
                case "本科":
                case "本科以上":
                    return 1;
                case "硕士":
                case "硕士以上":
                    return 2;
                case "博士":
                case "博士以上":
                    return 3;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 根据类型，返回当前人已经赞过的list
        /// </summary>
        /// <param name="type">1状态，2图片，0打招呼</param>
        /// <returns></returns>
        public JsonResult GetPraiseOrHelloList(int type)
        {
            if (type > 2) return Json(0);
            
            if (type == 0)
            {
                var hellos = LoveDb.HelloAll().Where(n => n.UserId == CheckValid());
                return Json(hellos.Select(i => i.ToUserId).ToList());
            }
            var pras = LoveDb.PraiseAll().Where(n => n.UserId == CheckValid() && n.StateType == (StateType)type).Select(i => i.StateId).ToList();//图片其实也是这个
            return Json(pras);
        }

        /// <summary>
        /// 获取我喜欢的人
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLikeList()
        {
            var id = CheckValid();
            var likes = LoveDb.MyLoveAll().Where(n => n.UserId == id);
            return Json(likes.Select(i => i.LoverId).ToList());
        }

        /// <summary>
        /// 获取对象的性别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetObjectSex(int id)
        {
            var str = LoveDb.One((User w)=>w.UserId==id).Sex == "man" ? "她" : "他";
            return str;
        }

        /// <summary>
        /// 返回id的性别
        /// </summary>
        /// <param name="id"></param>
        /// <returns>man women</returns>
        public string GetSex(int id)
        {
            return LoveDb.One((User w) => w.UserId == id).Sex;
        }

        /// <summary>
        /// 获取Id的Sex
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetIdSex(int id)
        {
            return LoveDb.One((User w) => w.UserId == id).Sex == "man" ? "他" : "她";
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="toMails"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public void SystemSendMail(string toMails,string subject,string content)
        {
            //Action invokeAction = () => SendMail.WebMailTo("renjian.zhang@deltaww.com.cn", "信息举报", "有举报，速度处理");
            Action invokeAction = () => SendMail.WebMailTo(toMails, subject, content);
            invokeAction.BeginInvoke(Callback, invokeAction);
        }
        private void Callback(IAsyncResult ar)
        {
            var action = ar.AsyncState as Action;
            if (action != null) action.EndInvoke(ar);
        }
        /// <summary>
        /// 将用户的登录时间换成语义化的
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string LastLoginstr(int uid)
        {
            var time = LoveDb.LastOne((LoginLog l) => l.UserId == uid);
            if (time != null)
            {
                TimeSpan ts = DateTime.Now - time.LoginTime;
                var days = (int)ts.TotalDays;
                string str;
                if (days < 10)
                {
                    switch (days)
                    {
                        case 0:
                            str = "今天登录过";
                            break;
                        case 1:
                            str = "昨天登录过";
                            break;
                        case 2:
                        case 3:
                            str = "三天内登录过";
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            str = "七天内登录过";
                            break;
                        default:
                            str = "7天前登录过";
                            break;
                    }
                }
                else if (days < 30)
                {
                    str = "10天前登录过";
                }
                else if (days < 60)
                {
                    str = "一个月前登录过";
                }
                else if (days < 180)
                {
                    str = "两个月前登录过";
                }
                else
                {
                    str = "半年前登录过";
                }

                return str;
            }
            return "";
        }
    }
}
