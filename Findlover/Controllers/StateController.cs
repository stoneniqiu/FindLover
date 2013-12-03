using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Findlover.Models;
using Findlover.ViewModel;

namespace Findlover.Controllers
{
    /// <summary>
    /// 状态，喜欢，不喜欢
    /// </summary>
    public class StateController : BaseController
    {

        public ActionResult Index()
        {
            if (CheckValid() == -1)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            return View();
        }

        
        /// <summary>
        /// 用户状态
        /// </summary>
        /// <returns></returns>
        public ActionResult UserState( )
        {
            var id = CheckValid();
            if (id == -1)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            var baseusers = GetBaseUsers(id);
            var require = LoveDb.One((Requirement r) => r.UserId == id);
            var views = (from baseuser1 in baseusers
                let stateuser = LoveDb.One((User u) => u.UserId == baseuser1.UserId)
                let baseinfo = LoveDb.One((BaseInfo u) => u.UserId == baseuser1.UserId)
                let state = LoveDb.LastOne((State u) => u.UserId == baseuser1.UserId)
                where state != null&&PartMathUser(require,LoveDb.GetUninUser(state.UserId))
                select new UserStateView
                {
                    SatateUser = stateuser,
                    ImgCount = LoveDb.IamgeCount(baseuser1.UserId), Education = baseinfo.Education, Hight = baseinfo.Height, State = state, RecommentRate = GetRecommendRate(id, baseuser1.UserId).TotalRate
                }).ToList();

            //获取复合我的条件的所有女生  这里先不管
            return PartialView(views.OrderByDescending(n=>n.State.ActionTime));
        }

        /// <summary>
        /// 状态删除
        /// </summary>
        /// <param name="id"></param>
        public void DeleteState(int id)
        {
            LoveDb.Delete<State>(id);
        }

        /// <summary>
        /// 新增喜欢
        /// </summary>
        /// <returns></returns>
        public ActionResult MyLoverView()
        {

            return View();
        }

        public ActionResult PartLoverView()
        {
            var id = CheckValid();
            if (id == -1 || Session["uid"] == null)
            {
                return RedirectToAction("Logon", "User");//不知道有没有效果
            }
            var lt = LoveDb.MyLoveAll().Where(u => u.LoverId == id).ToList();
            foreach (var myLove in lt.Where(n=>!n.IsRead))
            {
                LoveDb.ReadMessage<MyLove>(myLove.Id);
            }
            var lovers = lt.Select(myLove => new VisitUser
            {
                LastVisitTime = myLove.ActionTime, Visitor = LoveDb.GetUninUser(myLove.UserId), Times = 0
            }).ToList();
            ViewBag.Sex = GetObjectSex(id);
            return PartialView(lovers.OrderByDescending(n=>n.LastVisitTime));
        }

        /// <summary>
        /// 我喜欢的人
        /// </summary>
        /// <returns></returns>
        public ActionResult IloveView()
        {
            var id = CheckValid();
            var lt = LoveDb.MyLoveAll().Where(u => u.UserId == id).ToList();
            var lls = lt.Select(myLove => new LoverState
            {
                ActionTime = myLove.ActionTime,
                LastState = LoveDb.GetOneState(u => u.UserId == myLove.LoverId),
                Lover = LoveDb.GetUninUser(myLove.LoverId),
            }).ToList();
           
            ViewBag.Sex = GetObjectSex(id);
            return PartialView(lls.OrderByDescending(n => n.ActionTime));
        }

        public ActionResult IdisloveView()
        {
            var id = CheckValid();
            var lls = LoveDb.DisLoveAll().Where(u => u.UserId == id).Select(dislove => new LoverState
            {
                ActionTime = dislove.ActionTime,
                LastState = LoveDb.GetOneState(u => u.UserId == dislove.DisLoveId),
                Lover = LoveDb.GetUninUser(dislove.DisLoveId),
            }).ToList();
            ViewBag.Sex = GetObjectSex(id);
            return PartialView(lls.OrderByDescending(n => n.ActionTime));
        }

        /// <summary>
        /// 增加一个喜欢的人
        /// </summary>
        /// <param name="loverid">喜欢人的Id</param>
        /// <returns>0是表示已经喜欢过了,1表示成功</returns>
        public JsonResult AddMyLover(int loverid)
        {
            var id = CheckValid();
            var love = LoveDb.One((MyLove m) => m.LoverId == loverid && m.UserId == id);
            if (love != null)//存在 就删除 
            {
                LoveDb.Delete<MyLove>(love.Id);
                return Json(0);//已经取消喜欢
            }
            love = new MyLove
            {
                LoverId = loverid,
                UserId = id,
                ActionTime = DateTime.Now,
                IsRead = false,
            };
            LoveDb.Add(love);
            //用户热度

            return Json(1);
        }

        /// <summary>
        /// 增加一个不感兴趣的人
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>1表示添加成功0表示取消</returns>
        public JsonResult AddDisLike(int userid)
        {
            var id = CheckValid();
            var love = LoveDb.One((DisLove m) => m.DisLoveId == userid && m.UserId == id);
            if (love != null)//存在 就删除 
            {
                LoveDb.Delete<DisLove>(love.Id);
                return Json(0);//已经取消喜欢
            }

            love = new DisLove
            {
                DisLoveId = userid,
                UserId = id,
                ActionTime = DateTime.Now,
            };
            LoveDb.Add(love);
            return Json(1);
        }

        public JsonResult GetImgInfos(int userid,string firstimgurl)
        {
            // 首先要判断进来的这张 是不是头像。 不是头像就要删除一张
            //再加到最前
            var imglists = new List<Iamgbox>();
            var imgs = LoveDb.IamgAll().Where(n => n.UserId == userid).OrderByDescending(n=>n.Id).ToList();
            var user = LoveDb.One((User u) => u.UserId == userid);
            var fisrtone = imgs.Find(n => n.ImgUrl == firstimgurl);
            if (fisrtone != null)
            {
                imgs.Remove(fisrtone);
            }
            else
            {
                fisrtone = new Iamgbox
                {
                    ActionTime = DateTime.Now,
                    BoxName = "1",//特殊图片... 特殊处理吧
                    ImgUrl = firstimgurl,
                    UserId = user.UserId,
                    Id = 0,
                    PraiseCount = 0,
                    Remark = "我的图片",
                    VisitCount = 0,
                };
            }
            imglists.Add(fisrtone);
            imglists.AddRange(imgs);
            return Json(imglists);
        }

    }
}
