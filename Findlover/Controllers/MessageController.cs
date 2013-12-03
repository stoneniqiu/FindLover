using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Findlover.Models;
using Findlover.ViewModel;

namespace Findlover.Controllers
{
    public class MessageController : BaseController
    {
        /// <summary>
        /// 打招呼
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult MakeaHello(int userid)
        {
            var id = CheckValid();
            if (LoveDb.One((Hello n) => n.ToUserId == userid && n.UserId == id) != null)
            {
                return Json(0);
            }
            //增加一个Hello
            var h = new Hello
            {
                ActionTime = DateTime.Now,
                ToUserId = userid,
                UserId = id
            };
            LoveDb.Add(h);
            //转化为消息 
            var name = GetUserNameById(id);
            LoveDb.Add(new Message
            {
                ActionTime = h.ActionTime,
                Content = "觉得你很赞,给你打了个招呼.",
                FromUserId = id,//当时人发的
                ToUserId = userid,
                FromUserName = name,
                IsReaded = false,
                MegType = MegType.Praise,//是赞的一中.
                StateType = StateType.None,
                StateId = 0
            });
            // 用户热度加1 被赞的用户的
            LoveDb.HelloCountAdd(userid);

            return Json(1);
        }

        /// <summary>
        /// 赞 专门赞状态的 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="stateid">状态id</param>
        /// <param name="statetype">判断是状态 1还是图片 2 </param>
        /// <returns>0 说明已经赞过了 1是成功的</returns>
        public JsonResult MakeaParise(int userid, int stateid, int statetype)
        {
            // 重复判断
            var stype = (StateType)statetype;
            var id = CheckValid();
            
            var pr = LoveDb.One((Praise p) => p.UserId == id && p.ToUserId == userid && p.StateType == stype && p.StateId == stateid);
            if (pr != null)
            {
                return Json(0);//说明已经赞过了
            }
            // 增加一个赞
            var praise = new Praise
            {
                ActionTime = DateTime.Now,
                StateId = stateid,
                StateType = stype,
                ToUserId = userid,
                UserId = id
            };
            LoveDb.Add(praise);
            // 转化为消息
            LoveDb.Add(new Message
            {
                ActionTime = praise.ActionTime,
                Content = statetype == 1 || statetype == 0 ? "赞了你发布的状态:" : "赞了你的图片",
                FromUserId = id,//当时人发的
                ToUserId = userid,
                FromUserName = GetUserNameById(id),
                IsReaded = false,
                MegType = MegType.Praise,//是赞的一中.
                StateType = stype,
                StateId = stateid
            });

            //这条状态或者图片的赞也要加一
            LoveDb.StateOrImagePraiseCountAdd(stateid, stype);

            // 用户热度加1 
            LoveDb.PraiseCountAdd(userid);


            // 用户热度加1 
            return Json(1);
        }

        /// <summary>
        /// 所有的私信
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckPrivateMessage()
        {
            var id = CheckValid();
            //msgtype=0 表示是私信
            var msgList = LoveDb.MessageAll().Where(m => (m.ToUserId == id || m.FromUserId == id) && m.MegType == 0).OrderByDescending(n => n.ActionTime).ToList();
            var msgInfoList = new List<MessageInfo>();
            foreach (Message message in msgList)
            {
                var tempid = message.FromUserId == id ? message.ToUserId : message.FromUserId;

                if (msgInfoList.FirstOrDefault(n => n.UserId == tempid) == null)//过滤掉重复的 每个人只出现一次。
                {
                    var msgInfo = new MessageInfo();
                    UninUser biguser;

                    msgInfo.LastMessageTime = message.ActionTime;
                    msgInfo.MsgCount = msgList.Count(n => n.ToUserId == tempid || n.FromUserId == tempid);
                    msgInfo.Message = message.Content;
                    if (message.StateType == StateType.Personal)//评论的状态
                    {
                        var state = LoveDb.One((State s) => s.Id == message.StateId);
                        if (state != null)
                        {
                            msgInfo.Message += "<br/><div class='oldmsg'>" + state.Content + "</div>";
                        }
                    }
                    if (message.StateType == StateType.Image)
                    {
                        var img = LoveDb.One((Iamgbox i) => i.Id == message.StateId);
                        if (img != null)
                        {
                            msgInfo.Message += "<br/><div class='oldmsg'><div class='imgtigger'><img src='" + img.ImgUrl + "' /></div></div>";
                        }

                    }


                    if (message.FromUserId == id)//说明发送者是本人，此时显示 接收者的图片和信息
                    {
                        biguser = LoveDb.GetUninUser(message.ToUserId);
                        msgInfo.UserId = message.ToUserId;
                        msgInfo.UserName = GetUserNameById(message.ToUserId);
                        msgInfo.Tag = "发给:";

                    }
                    else//说明别人给我发送
                    {
                        biguser = LoveDb.GetUninUser(message.FromUserId);
                        msgInfo.UserId = message.FromUserId;
                        msgInfo.UserName = message.FromUserName;
                        msgInfo.Tag = "";
                    }
                    msgInfo.ImgUrl = biguser.User.ImgUrl;
                    msgInfo.Age = biguser.User.Age;
                    msgInfo.City = biguser.BaseInfo.ResidenceCity;
                    msgInfoList.Add(msgInfo);

                }
            }

            return View(msgInfoList);
        }

        public ActionResult CheckMessageRecords(int? fid)
        {
            var uid = CheckValid();
            ViewBag.myid = uid;
            ViewBag.ImgUrl = LoveDb.One((User u) => u.UserId == uid).ImgUrl;//自己的图片
            List<Message> msg;
            if (fid != null)
            {
                ViewBag.name = GetUserNameById((int)fid);
                var user = LoveDb.One((User u) => u.UserId == fid);
                ViewBag.OtherImgUrl = user.ImgUrl;//她人的图片
                ViewBag.OtherId = user.UserId;
                ViewBag.Sex = user.Sex == "man" ? "他" : "她";
                //我发给他的和他发给我的都显示出来
                msg = LoveDb.MessageAll().Where(m => m.MegType == MegType.Private && ((m.ToUserId == fid && m.FromUserId == uid) ||
                    (m.ToUserId == uid && m.FromUserId == fid))).OrderByDescending(n => n.ActionTime).ToList();
            }
            else
            {
                ViewBag.name = "意中人管理员";
                ViewBag.Sex = "Ta";
                ViewBag.OtherId = 0;
                ViewBag.OtherImgUrl = "../../Content/Photos/bkq.jpg";//她人的图片
                msg = LoveDb.MessageAll().Where(n => n.MegType == MegType.System && n.ToUserId == uid).OrderByDescending(n => n.ActionTime).ToList();
            }
            //设置已读信息
            foreach (var message in msg.Where(n => n.ToUserId == uid && n.IsReaded == false).ToList())
            {
                LoveDb.ReadMessage<Message>(message.Id);
            }

            //找到对于我而言，处理引用
            foreach (var message in msg)
            {
                if (message.StateType == StateType.Personal)//评论的状态
                {
                    var state = LoveDb.One((State s) => s.Id == message.StateId);
                    if (state != null)
                    {
                        message.Content += string.Format("<br/><div class='oldmsg' data-userid='{0}'>{1}</div>",message.FromUserId, state.Content);
                    }
                }
                if (message.StateType == StateType.Image)
                {
                    var img = LoveDb.One((Iamgbox i) => i.Id == message.StateId);
                    if (img != null)
                    {
                        message.Content += string.Format("<br/><div class='oldmsg' data-userid='{0}'>{1}</div>", message.FromUserId, img.ImgUrl);
                    }
                }
            }
            return View(msg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="toid"></param>
        /// <param name="content"></param>
        /// <param name="type">msgtype 0为私信，1为系统消息</param>
        /// <param name="statetype">statetype</param>
        /// <param name="stateid">stateid</param>
        /// <returns></returns>
        public JsonResult SendMessage(int toid, string content, int type, int statetype, int stateid)
        {
            var id = CheckValid();
            var name =GetUserNameById(id);
            Message msg;
            if (type == 0)
            {
                msg = new Message
                {
                    ToUserId = toid,
                    FromUserId = id,
                    FromUserName = name,
                    Content = content,
                    IsReaded = false,
                    MegType = MegType.Private,
                    ActionTime = DateTime.Now,
                    StateId = stateid,//表示是状态 纯私信 图片
                    StateType = (StateType)statetype
                };
                //try
                //{
                //  //  var user1 = LoveDb.One((User u) => u.UserId == toid);
                //  //  SystemSendMail(user1.Email, "意中人_私信", string.Format("{0}给你发私信了快去看看吧,<a href='http://cnwj6iapc006:81/Message/CheckPrivateMessage'>查看私信</a>", user1.UserName));
                //}
                //catch 
                //{
                    
                  
                //}
            }
            else
            {
                msg = new Message
                {
                    ToUserId = toid,
                    FromUserId = id,
                    FromUserName = "意中人管理员",
                    Content = content,
                    IsReaded = false,
                    MegType = MegType.System,
                    ActionTime = DateTime.Now,
                };
            }

            LoveDb.Add(msg);
            return Json(msg);
        }

        public int RemoveMessage(int id)
        {
            if (id > 0)
            {
                LoveDb.Delete<Message>(id);
                return 1;
            }
            return 0;
        }
    }
}
