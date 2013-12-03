using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Schema;
using Findlover.ViewModel;
using NoteSystem.Helper;

namespace Findlover.Models
{
    public class LoveDbRepository
    {
        private static LoveDbRepository _loveDb;

        private LoveDbRepository()
        {
        }

        public static LoveDbRepository GetIntance()
        {
            return _loveDb ?? (_loveDb = new LoveDbRepository());
        }

        #region  常规方法 Add Delete All One  Ones LastOne

        #region Add
        
        public void Add<T>(T t)
        {
            using (var loveDb = new LoveDb())
            {
                switch (t.GetType().Name)
                {
                    case "User":
                        loveDb.Users.Add(t as User);
                        break;
                    case "BaseInfo":
                        loveDb.BaseInfos.Add(t as BaseInfo);
                        break;
                    case "Requirement":
                        loveDb.Requirements.Add(t as Requirement);
                        break;
                    case "DetailInfo":
                        loveDb.DetailInfos.Add(t as DetailInfo);
                        break;
                    case "LoveView":
                        loveDb.LoveViews.Add(t as LoveView);
                        break;
                    case "InfoStatistic":
                        loveDb.InfoStatistics.Add(t as InfoStatistic);
                        break;
                    case "UserHot":
                        loveDb.UserHots.Add(t as UserHot);
                        break;
                    case "Praise":
                        loveDb.Praises.Add(t as Praise);
                        break;
                    case "State":
                        loveDb.States.Add(t as State);
                        break;
                    case "Iamgbox":
                        loveDb.Iamgboxes.Add(t as Iamgbox);
                        break;
                    case "Message":
                        loveDb.Messages.Add(t as Message);
                        break;
                    case "LoginLog":
                        loveDb.LoginLogs.Add(t as LoginLog);
                        break;
                    case "Role":
                        loveDb.Roles.Add(t as Role);
                        break;
                    case "Hello":
                        loveDb.Hellos.Add(t as Hello);
                        break;
                    case "RoleLog":
                        loveDb.RoleLogs.Add(t as RoleLog);
                        break;
                    case "Authority":
                        loveDb.Authoritys.Add(t as Authority);
                        break;
                    case "AdminStatistic":
                        loveDb.AdminStatistics.Add(t as AdminStatistic);
                        break;
                    case "VisitLog":
                        loveDb.VisitLogs.Add(t as VisitLog);
                        break;
                    case "Report":
                        loveDb.Reports.Add(t as Report);
                        break;
                    case "ReportLog":
                        loveDb.ReportLogs.Add(t as ReportLog);
                        break;
                    case "MyLove":
                        loveDb.MyLoves.Add(t as MyLove);
                        break;
                    case "DisLove":
                        loveDb.DisLoves.Add(t as DisLove);
                        break;
                    case "Topic":
                        loveDb.Topics.Add(t as Topic);
                        break;
                    case "EnjoyTopic":
                        var en = t as EnjoyTopic;
                        loveDb.EnjoyTopics.Add(en);
                        if (en != null)
                        {
                            var top = loveDb.Topics.Find(en.TopicId);
                            top.LikeCount += 1;
                        }
                        break;
                    case "Comment":
                        var com = t as Comment;
                        loveDb.Comments.Add(com);
                        //让话题评论数加1 
                        if (com != null)
                        {
                            var top = loveDb.Topics.Find(com.TopicId);
                            top.ReplyCount += 1;
                            top.UpDataTime = DateTime.Now;
                        }


                        break;
                }
                loveDb.SaveChanges();
            }
        }
        
        #endregion
        
        #region Delete
        
        public void Delete<T>(int id)
        {
            using (var loveDb = new LoveDb())
            {
                switch (typeof(T).Name)
                {
                    case "User":
                        loveDb.Users.Remove(loveDb.Users.SingleOrDefault(m => m.UserId == id));
                        break;
                    case "BaseInfo":
                        loveDb.BaseInfos.Remove(loveDb.BaseInfos.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Requirement":
                        loveDb.Requirements.Remove(loveDb.Requirements.SingleOrDefault(m => m.Id == id));
                        break;
                    case "DetailInfo":
                        loveDb.DetailInfos.Remove(loveDb.DetailInfos.SingleOrDefault(m => m.Id == id));
                        break;
                    case "LoveView":
                        loveDb.LoveViews.Remove(loveDb.LoveViews.SingleOrDefault(m => m.Id == id));
                        break;
                    case "InfoStatistic":
                        loveDb.InfoStatistics.Remove(loveDb.InfoStatistics.SingleOrDefault(m => m.Id == id));
                        break;
                    case "UserHot":
                        loveDb.UserHots.Remove(loveDb.UserHots.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Praise":
                        loveDb.Praises.Remove(loveDb.Praises.SingleOrDefault(m => m.Id == id));
                        break;
                    case "State":
                        loveDb.States.Remove(loveDb.States.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Iamgbox":
                        loveDb.Iamgboxes.Remove(loveDb.Iamgboxes.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Message":
                        loveDb.Messages.Remove(loveDb.Messages.SingleOrDefault(m => m.Id == id));
                        break;
                    case "LoginLog":
                        loveDb.LoginLogs.Remove(loveDb.LoginLogs.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Role":
                        loveDb.Roles.Remove(loveDb.Roles.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Hello":
                        loveDb.Hellos.Remove(loveDb.Hellos.SingleOrDefault(m => m.Id == id));
                        break;
                    case "RoleLog":
                        loveDb.RoleLogs.Remove(loveDb.RoleLogs.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Authority":
                        loveDb.Authoritys.Remove(loveDb.Authoritys.SingleOrDefault(m => m.Id == id));
                        break;
                    case "AdminStatistic":
                        loveDb.AdminStatistics.Remove(loveDb.AdminStatistics.SingleOrDefault(m => m.Id == id));
                        break;
                    case "VisitLog":
                        loveDb.VisitLogs.Remove(loveDb.VisitLogs.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Report":
                        loveDb.Reports.Remove(loveDb.Reports.SingleOrDefault(m => m.Id == id));
                        break;
                    case "ReportLog":
                        loveDb.ReportLogs.Remove(loveDb.ReportLogs.SingleOrDefault(m => m.Id == id));
                        break;
                    case "MyLove":
                        loveDb.MyLoves.Remove(loveDb.MyLoves.SingleOrDefault(m => m.Id == id));
                        break;
                    case "DisLove":
                        loveDb.DisLoves.Remove(loveDb.DisLoves.SingleOrDefault(m => m.Id == id));
                        break;
                    case "Topic":
                        loveDb.Topics.Remove(loveDb.Topics.SingleOrDefault(m => m.Id == id));
                        break;
                    case "EnjoyTopic":
                     
                        var ens = loveDb.EnjoyTopics.SingleOrDefault(m => m.Id == id);
                        loveDb.EnjoyTopics.Remove(ens);
                        if (ens != null)
                        {
                            var top = loveDb.Topics.Find(ens.TopicId);
                            top.LikeCount -= 1;
                        }
                        break;
                    case "Comment":
                        var com = loveDb.Comments.SingleOrDefault(m => m.Id == id);
                        loveDb.Comments.Remove(com);
                        if (com != null)
                        {
                            var top = loveDb.Topics.Find(com.TopicId);
                            top.ReplyCount -= 1;
                        }
                        break;
                }
                loveDb.SaveChanges();
            }
        }

        #endregion
      
        #region All
        public List<User> UserAll()
        {
            using (var db = new LoveDb())
            {
                return db.Users.ToList();
            }
        }
        public List<EnjoyTopic> EnjoyTopicAll()
        {
            using (var db = new LoveDb())
            {
                return db.EnjoyTopics.ToList();
            }
        }

        public UninUser GetUninUser(int id)
        {
           
            using (var db = new LoveDb())
            {
                var userinfo = new UninUser
                    {
                        User = db.Users.FirstOrDefault(n => n.UserId == id) ,
                        BaseInfo = db.BaseInfos.FirstOrDefault(n => n.UserId == id),
                        DetailInfo = db.DetailInfos.FirstOrDefault(n => n.UserId == id),
                        LoveView = db.LoveViews.FirstOrDefault(n => n.UserId == id)
                    };
                return userinfo;
            }
        }

        public List<MyLove> MyLoveAll()
        {
            using (var db = new LoveDb())
            {
                return db.MyLoves.ToList();
            }
        }

        public List<Topic> TopicAll()
        {
            using (var db = new LoveDb())
            {
                return db.Topics.ToList();
            }
        }

        public List<DisLove> DisLoveAll()
        {
            using (var db = new LoveDb())
            {
                return db.DisLoves.ToList();
            }
        }
        public List<State> StateAll()
        {
            using (var db = new LoveDb())
            {
                return db.States.ToList();
            }
        }

        public List<Iamgbox> IamgAll()
        {
            using (var db = new LoveDb())
            {
                return db.Iamgboxes.ToList();
            }
        }

        public List<Role> RoleAll()
        {
            using (var db = new LoveDb())
            {
                return db.Roles.ToList();
            }
        }
        public List<ReportLog> ReportLogAll()
        {
            using (var db = new LoveDb())
            {
                return db.ReportLogs.ToList();
            }
        }

        public List<Message> MessageAll()
        {
            using (var db = new LoveDb())
            {
                return db.Messages.ToList();
            }
        }

        public List<Comment> CommentAll()
        {
            using (var db = new LoveDb())
            {
                return db.Comments.ToList();
            }
        }

        public List<AdminStatistic> AdminStatisticAll()
        {
            using (var db = new LoveDb())
            {
                return db.AdminStatistics.ToList();
            }
        }

        public List<VisitLog> VisitorAll()
        {
            using (var db = new LoveDb())
            {
                return db.VisitLogs.ToList();
            }
        }

        public List<Praise> PraiseAll()
        {
            using (var db = new LoveDb())
            {
                return db.Praises.ToList();
            }
        }

        public List<Hello> HelloAll()
        {
            using (var db = new LoveDb())
            {
                return db.Hellos.ToList();
            }
        }

        /// <summary>
        /// 已经倒叙
        /// </summary>
        /// <returns></returns>
        public List<Report> ReportAll()
        {
            using (var db = new LoveDb())
            {
                return db.Reports.OrderByDescending(n=>n.Id).ToList();
            }
        } 



        public List<UserHot> UserHotAllDes()
        {
            using (var db = new LoveDb())
            {
                return db.UserHots.OrderByDescending(n=>n.HotValue).ToList();
            }
        }

        #endregion

        #region One
        public User One(Func<User, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Users.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public BaseInfo One(Func<BaseInfo, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.BaseInfos.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Iamgbox One(Func<Iamgbox, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Iamgboxes.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public LoveView One(Func<LoveView, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.LoveViews.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Requirement One(Func<Requirement, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Requirements.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public DetailInfo One(Func<DetailInfo, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.DetailInfos.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Comment One(Func<Comment, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Comments.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public MyLove One(Func<MyLove, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.MyLoves.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public DisLove One(Func<DisLove, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.DisLoves.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public InfoStatistic One(Func<InfoStatistic, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.InfoStatistics.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public State One(Func<State, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.States.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Hello One(Func<Hello, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Hellos.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Role One(Func<Role, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Roles.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public EnjoyTopic One(Func<EnjoyTopic, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.EnjoyTopics.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public RoleLog One(Func<RoleLog, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.RoleLogs.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public UserHot One(Func<UserHot, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.UserHots.ToList().Where(predicate).FirstOrDefault();
            }
        }

        public Authority One(Func<Authority, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Authoritys.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Topic One(Func<Topic, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Topics.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public AdminStatistic One(Func<AdminStatistic, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.AdminStatistics.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Message One(Func<Message, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Messages.ToList().Where(predicate).FirstOrDefault();
            }
        }

        public Praise One(Func<Praise, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Praises.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public Report One(Func<Report, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Reports.ToList().Where(predicate).FirstOrDefault();
            }
        }
        public ReportLog One(Func<ReportLog, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.ReportLogs.ToList().Where(predicate).FirstOrDefault();
            }
        }
        #endregion 

        #region LastOne
        public Message LastOne(Func<Message, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Messages.ToList().Where(predicate).LastOrDefault();
            }
        }
        public Topic LastOne(Func<Topic, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.Topics.ToList().Where(predicate).LastOrDefault();
            }
        }
 
        public State LastOne(Func<State, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                var st=db.States.ToList().Where(predicate).LastOrDefault();
                return st;
            }
        }

        public State GetOneState(Func<State, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                var state = db.States.ToList().Where(predicate).LastOrDefault() ?? new State
                {
                    Content = "我刚来到意中人,赶快发现我吧",
                };
                return state;
            }
        }

        public VisitLog LastOne(Func<VisitLog, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.VisitLogs.ToList().Where(predicate).LastOrDefault();
            }
        }

        public LoginLog LastOne(Func<LoginLog, bool> predicate)
        {
            using (var db = new LoveDb())
            {
                return db.LoginLogs.ToList().Where(predicate).LastOrDefault();
            }
        }

        #endregion

        #endregion

        #region 定制方法,Update相关,logon,check
        /// <summary>
        /// 更新恋爱观资料表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="love"></param>
        public void UpdateLoveView(int id, LoveView love)
        {
            using (var db = new LoveDb())
            {
                var dst = db.LoveViews.SingleOrDefault(n => n.UserId == id);
                if (dst == null) return;
                var i = 0;
                dst.WorkTimePlan = Modifycount(love.WorkTimePlan, ref i);
                dst.Smoking = Modifycount(love.Smoking, ref i);
                dst.Drinking = Modifycount(love.Drinking, ref i);
                dst.LoveDuration = Modifycount(love.LoveDuration, ref i);
                dst.WantaBaby = Modifycount(love.WantaBaby, ref i);
                dst.ParentLiveTogether = Modifycount(love.ParentLiveTogether, ref i);
                dst.Housework = Modifycount(love.Housework, ref i);
                dst.ManageMoney = Modifycount(love.ManageMoney, ref i);
                dst.Cooking = Modifycount(love.Cooking, ref i);
                dst.Allopatry = Modifycount(love.Allopatry, ref i);

                var statistic = db.InfoStatistics.SingleOrDefault(b => b.UserId == id);
                if (statistic != null)
                {
                    statistic.LoveViewsReal = i;
                    var real = statistic.DetialsInfoReal + statistic.BaseInfoReal + statistic.LoveViewsReal;
                    var imgsum = db.Iamgboxes.Count(n => n.UserId == id);
                    imgsum = imgsum > 22 ? 22 : imgsum;
                    real += imgsum;
                    //还差一条标准 .. 回答问题 或者别的。  准备18道题目。
                    var x = (float)real / 50;
                    statistic.Percent = x; // (float)Math.Round(x, 2);
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 更新消息资料表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="info"></param>
        public void UpdateBaseInfo(int id, BaseInfo info)
        {
            using (var db = new LoveDb())
            {
                var dst = db.BaseInfos.SingleOrDefault(n => n.UserId == id);
                if (dst == null) return;
                var i = 0;
                dst.Height = Modifycount(info.Height, ref i);
                dst.Education = Modifycount(info.Education, ref i);
                dst.MonthlyIncome = Modifycount(info.MonthlyIncome, ref i);
                dst.ResidenceProvince = Modifycount(info.ResidenceProvince, ref i);
                dst.ResidenceCity = Modifycount(info.ResidenceCity, ref i);
                if (info.ResidenceCity != "选择城市")
                {
                    i--;
                }
                dst.School = Modifycount(info.School, ref i);
                dst.Profession = Modifycount(info.Profession, ref i);
                dst.Position = Modifycount(info.Position, ref i);
                dst.Company = Modifycount(info.Company, ref i);
                dst.State = Modifycount(info.State, ref i);

                var statistic = db.InfoStatistics.SingleOrDefault(b => b.UserId == id);
                if (statistic != null)
                {
                    statistic.BaseInfoReal = i;
                    var real = statistic.DetialsInfoReal + statistic.BaseInfoReal + statistic.LoveViewsReal;
                    var imgsum = db.Iamgboxes.Count(n => n.UserId == id);
                    imgsum = imgsum > 22 ? 22 : imgsum;
                    real += imgsum;
                    //还差一条标准 .. 回答问题 或者别的。  准备18道题目。
                    var x = (float)real / 50;
                    statistic.Percent = x; // (float)Math.Round(x, 2);
                }
                db.SaveChanges();
            }

        }

        public double GetPercent(int userid)
        {
            using (var db = new LoveDb())
            {
                var statistic = db.InfoStatistics.SingleOrDefault(b => b.UserId == userid);
                if (statistic != null)
                {
                    var real = statistic.DetialsInfoReal + statistic.BaseInfoReal + statistic.LoveViewsReal;
                    var imgsum = db.Iamgboxes.Count(n => n.UserId == userid);
                    imgsum = imgsum > 22 ? 22 : imgsum;
                    real += imgsum;
                    //还差一条标准 .. 回答问题 或者别的。  准备18道题目。
                    var x = (float)real / 50;
                    statistic.Percent = x; // (float)Math.Round(x, 2);
                    db.SaveChanges();
                    return x;
                }
            }
            return 0;
        }

        public void UpdateTopic(Topic model)
        {
            using (var db = new LoveDb())
            {
                var one = db.Topics.Find(model.Id);
                if (one != null)
                {
                    one.Title = model.Title;
                    one.Content = model.Content;
                    one.UpDataTime = DateTime.Now;

                    db.SaveChanges();
                }

            }
        }

        /// <summary>
        /// 将IsRead=false的消息设置为true
        /// </summary>
        public void ReadMessage<T>(int id)
        {
            using (var db=new LoveDb())
            {
                switch (typeof(T).Name)
                {
                        //消息
                    case "Message":
                        db.Messages.Find(id).IsReaded = true;
                        break;
                    case "MyLove":
                        db.MyLoves.Find(id).IsRead = true;
                        break;
                    case "VisitLog":
                        db.VisitLogs.Find(id).IsRead = true;
                        break;
                    case "EnjoyTopic":
                        db.EnjoyTopics.Find(id).IsRead = true;
                        break;
                }
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 更新消息资料表
        /// </summary>
        /// <param name="info"></param>
        public void UpdateRequirement(Requirement info)
        {
            using (var db = new LoveDb())
            {
                var dst = db.Requirements.SingleOrDefault(n => n.UserId == info.UserId);
                if (dst == null) return;
                dst.AgeLl = info.AgeLl;
                dst.AgeUl = info.AgeUl;
                dst.HightLl = info.HightLl;
                dst.HightUl = info.HightUl;
                dst.ResidenceCity = info.ResidenceCity;
                dst.Education = info.Education;
                dst.MonthlyIncomeLl = info.MonthlyIncomeLl;
                dst.MonthlyIncomeUl = info.MonthlyIncomeUl;
                dst.ResidenceProvince = info.ResidenceProvince;
                db.SaveChanges();
            }

        }
        /// <summary>
        /// 更新详细资料表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="detail"></param>
        public void UpdateDetail(int id, DetailInfo detail)
        {
            using (var db = new LoveDb())
            {
                var dst = db.DetailInfos.SingleOrDefault(n => n.UserId == id);
                if (dst == null) return;
                var i = 0;

                dst.Housing = Modifycount(detail.Housing, ref i);
                dst.Car = Modifycount(detail.Car, ref i);
                dst.People = Modifycount(detail.People, ref i);
                dst.Constellation = Modifycount(detail.Constellation, ref i);
                dst.BloodType = Modifycount(detail.BloodType, ref i);
                dst.NativePlace = Modifycount(detail.NativePlace, ref i);
                dst.NativeCity = Modifycount(detail.NativeCity, ref i);

                if (detail.NativeCity != "请选择" && detail.NativeCity!="")
                {
                    i--;
                }
                dst.Weight = Modifycount(detail.Weight, ref i);

                if (!string.IsNullOrEmpty(detail.DouBan))
                {
                    dst.DouBan = detail.DouBan.Trim();
                    i++;
                }
                else
                {
                    dst.DouBan = null;
                }

                if (!string.IsNullOrEmpty(detail.MicroBlog))
                {
                    dst.MicroBlog = detail.MicroBlog.Trim();
                    i++;
                }
                else
                {
                    dst.MicroBlog = null;
                }

                var statistic = db.InfoStatistics.SingleOrDefault(b => b.UserId == id);
                if (statistic != null)
                {
                    statistic.DetialsInfoReal = i;
                    var real = statistic.DetialsInfoReal + statistic.BaseInfoReal + statistic.LoveViewsReal;
                    var imgsum = db.Iamgboxes.Count(n => n.UserId == id);
                    imgsum = imgsum > 22 ? 22 : imgsum;
                    //还差一条标准 .. 回答问题 或者别的。  准备18道题目。
                    statistic.Percent = (float)(real + imgsum) / 50;
                }

                db.SaveChanges();
            }
        }


        /// <summary>
        /// 将举报信息处理为已处理
        /// </summary>
        /// <param name="id"></param>
        public void DoneReport(int id)
        {
            using (var db = new LoveDb())
            {
                var rep = db.Reports.Find(id);
                rep.IsDone = true;
                db.SaveChanges();
            }
        }

       
        /// <summary>
        /// 审核资料或图片
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="result"></param>
        /// <param name="type">0表示图片,1表示资料</param>
        /// <returns></returns>
        public bool CheckImgOrInfo(int userid, bool result,int type)
        {
            using (var db = new LoveDb())
            {
                var user = db.Users.SingleOrDefault(n => n.UserId == userid);
                if (user == null) return false;
                if (type == 0)
                {
                    user.IsCheckedImg = true;
                    user.IsVerifiedImg = result;
                }
                else
                {
                    user.IsChecked = true;
                    user.IsVerified = result;
                }
              
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 封号
        /// </summary>
        /// <param name="userId"></param>
        public void ForbidUser(int userId)
        {
            using (var db = new LoveDb())
            {
                var u = db.Users.Find(userId);
                if (u != null)
                {
                    u.Enable = 0;
                    db.SaveChanges();
                }
            }
        }
        /// <summary>
        /// 解封
        /// </summary>
        /// <param name="userId"></param>
        public void UnForbidUser(int userId)
        {
            using (var db = new LoveDb())
            {
                var u = db.Users.Find(userId);
                if (u != null)
                {
                    u.Enable =1;
                    db.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// 统计管理员数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type">0图片,1资料,2登录最近一天,签到</param>
        public void AdminStatistics(int userid, int type)
        {
            //经验值如何算
            using (var db = new LoveDb())
            {
                var role = db.AdminStatistics.SingleOrDefault(n => n.UserId == userid);
                if (role != null)
                {
                    switch (type)
                    {
                        case 0:
                            role.CheckImgCount += 1;
                            break;
                        case 1:
                            role.CheckInfoCount += 1;
                            break;
                        case 2:
                            if (DateTime.Now.DayOfYear - role.LastLogin.DayOfYear > 1)// 需要判断是否重复 
                            {
                                role.SignInDays = 1;
                            }
                            else
                            {
                                role.SignInDays += 1;
                            }
                            role.LastLogin = DateTime.Now;
                            break;
                    }
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 访问记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="visitor"></param>
        public void VisitCount(User user, User visitor)
        {
            // 记录访问次数， 记录访问人物。
            using (var db = new LoveDb())
            {
                var userhot = db.UserHots.SingleOrDefault(n => n.UserId == user.UserId);
                if (userhot == null) return;
               
                //为防止刷屏，最近两次访问要超过30分钟
                var visitlog = db.VisitLogs.SingleOrDefault(n => n.VisitorId == visitor.UserId && n.UserId == user.UserId);
                if (visitlog == null)
                {
                    var newlog = new VisitLog
                    {
                        UserId = user.UserId,
                        VisitorId = visitor.UserId,
                        VisitorName = visitor.UserName,
                        ActionTime = DateTime.Now,
                        IsRead = false,
                        Count = 1,
                    };
                    db.VisitLogs.Add(newlog);
                    Logger.Debug(visitor.UserName + "(Id:" + visitor.UserId + ") 访问了 " + user.UserName + "(Id:" + user.UserId + ")");
                }
                if ((visitlog != null && DiffMinute(visitlog.ActionTime, DateTime.Now) >= 30) )
                {
                    userhot.VistitCount += 1;
                    userhot.HotValue += 1;
                    visitlog.Count += 1;
                    visitlog.IsRead = false;//这时候相当于一个新的消息。
                    visitlog.ActionTime = DateTime.Now;
                  Logger.Debug(visitor.UserName+"(Id:"+visitor.UserId+") 访问了 "+user.UserName+"(Id:"+user.UserId+")");
                } 
                
                db.SaveChanges();
            }
        }

        public int IamgeCount(int userId)
        {
            using (var db = new LoveDb())
            {
                return db.Iamgboxes.Count(n => n.UserId == userId) + 1;
            }
        }

        #region 用户热度相关  登录次数，打招呼，喜欢，赞
        /// <summary>
        /// 登陆处理 次数加1，并写入日志
        /// </summary>
        /// <param name="id"></param>
        public void LoginCountAdd(int id)
        {
            using (var db = new LoveDb())
            {
                var user = db.Users.FirstOrDefault(n => n.UserId == id);
                if (user == null) return;
               // user.LogCount += 1;
                var visitlog = db.LoginLogs.ToList().LastOrDefault(n => n.UserId==id);
                if ((visitlog != null && DiffMinute(visitlog.LoginTime, DateTime.Now) >= 600) || visitlog == null)
                {
                    var hot = db.UserHots.FirstOrDefault(n => n.UserId == id);
                    if (hot != null)
                    {
                        hot.LogCount += 1;
                        hot.HotValue += 1;
                    }

                    var userlog = new LoginLog
                    {
                        UserId = user.UserId,
                        LoginTime = DateTime.Now,
                        IpAddress = Dns.GetHostName(),
                    };
                    db.LoginLogs.Add(userlog);
                    Logger.Debug(user.UserName + "(Id:" + user.UserId + ") 登录了");
                } 
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 打招呼加1 
        /// </summary>
        /// <param name="id"></param>
        public void HelloCountAdd(int id)
        {
            using (var db = new LoveDb())
            {
                var hot = db.UserHots.FirstOrDefault(n => n.UserId == id);
                if (hot != null)
                {
                    hot.HelloCount += 1;
                    hot.HotValue += 1;
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 赞加一
        /// </summary>
        /// <param name="id"></param>
        public void PraiseCountAdd(int id)
        {
            using (var db = new LoveDb())
            {
                var hot = db.UserHots.FirstOrDefault(n => n.UserId == id);
                if (hot != null)
                {
                    hot.PraiseCount += 1;
                    hot.HotValue += 1;
                }
                db.SaveChanges();
            }
        }

        public void LoveCountAdd(int id)
        {
            using (var db = new LoveDb())
            {
                var hot = db.UserHots.FirstOrDefault(n => n.UserId == id);
                if (hot != null)
                {
                    hot.CollectCount += 1;
                    hot.HotValue += 1;
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 修正状态的赞
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="stateType"></param>
        public void StateOrImagePraiseCountAdd(int stateId, StateType stateType)
        {
            using (var db = new LoveDb())
            {
                if (stateType == StateType.Personal)
                {
                    var s = db.States.Find(stateId);
                    if (s != null)
                    {
                        s.PraiseCount++;
                    }
                }
                if (stateType==StateType.Image)
                {
                    var i = db.Iamgboxes.Find(stateId);
                    if (i != null)
                    {
                        i.PraiseCount++;
                    }
                }
                db.SaveChanges();
            }
        }

        #endregion


        /// <summary>
        /// 退出 换成logger
        /// </summary>
        /// <param name="userid"></param>
        public void Logoff(int userid)
        {
            using (var db = new LoveDb())
            {
                var user = db.Users.SingleOrDefault(n => n.UserId == userid);
                if (user == null) return;
                var userlog =
                    db.LoginLogs.Where(n => n.UserId == user.UserId)
                    .OrderByDescending(n => n.Id)
                    .FirstOrDefault();
                if (userlog != null)
                {
                    userlog.LogoutTime = DateTime.Now;
                    //DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                }
                db.SaveChanges();
            }
        }



        #endregion

        #region 辅助方法

        /// <summary>
        /// 计算两个时间差
        /// </summary>
        /// <param name="beforeTime"></param>
        /// <param name="afterTime"></param>
        /// <returns>返回分钟数</returns>
        public double DiffMinute(DateTime beforeTime, DateTime afterTime)
        {
            TimeSpan timeSpan = afterTime - beforeTime;
            return timeSpan.TotalMinutes;
        }

        private static string Modifycount(string str, ref int i)
        {
            if (!string.IsNullOrEmpty(str) && str != "请选择"&& str != "选择城市")
            {
                i++;
                return str;
            }
            return null;
        }
        private float Modifycount(float str, ref int i)
        {
            if (str > 0)
            {
                i++;
                return str;
            }
            return 0;
        }

        /// <summary>
        /// 字典的最大不好 就是不允许空。当初这个方法设计有问题。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="dictionary"></param>
        public void Update<T>(int id, Dictionary<string, object> dictionary)
        {
            using (var loveDb = new LoveDb())
            {
                object obj = null;
                switch (typeof(T).Name)
                {
                    case "User":
                        obj = loveDb.Users.SingleOrDefault(n => n.UserId == id);
                        break;
                    case "BaseInfo":
                        obj = loveDb.BaseInfos.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Requirement":
                        obj = loveDb.Requirements.SingleOrDefault(n => n.Id == id);
                        break;
                    case "DetailInfo":
                        obj = loveDb.DetailInfos.SingleOrDefault(n => n.Id == id);
                        break;
                    case "LoveView":
                        obj = loveDb.LoveViews.SingleOrDefault(n => n.Id == id);;
                        break;
                    case "InfoStatistic":
                        obj = loveDb.InfoStatistics.SingleOrDefault(n => n.Id == id);
                        break;
                    case "UserHot":
                        obj = loveDb.UserHots.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Praise":
                        obj = loveDb.Praises.SingleOrDefault(n => n.Id == id);
                        break;
                    case "State":
                        obj = loveDb.States.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Iamgbox":
                        obj = loveDb.Iamgboxes.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Message":
                        obj = loveDb.Messages.SingleOrDefault(n => n.Id == id);
                        break;
                    case "LoginLog":
                        obj = loveDb.LoginLogs.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Role":
                        obj = loveDb.Roles.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Hello":
                        obj = loveDb.Hellos.SingleOrDefault(n => n.Id == id);
                        break;
                    case "RoleLog":
                        obj = loveDb.RoleLogs.SingleOrDefault(n => n.Id == id);
                        break;
                    case "Authority":
                        obj = loveDb.Authoritys.SingleOrDefault(n => n.Id == id);
                        break;
                    case "AdminStatistic":
                        obj = loveDb.AdminStatistics.SingleOrDefault(n => n.Id == id);
                        break;  
                    case "Topic":
                        obj = loveDb.Topics.SingleOrDefault(n => n.Id == id);
                        break;
                   
                }
                if (obj == null) return;
                foreach (var element in dictionary)
                {
                    ObjectCopier.CopyProperty(obj, element.Key, element.Value);
                }
                loveDb.SaveChanges();
            }

        }
       
        public static LoveDbRepository GetInstance()
        {
            return _loveDb ?? (_loveDb = new LoveDbRepository());
        }
        #endregion

       
    }

}