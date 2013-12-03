namespace Findlover.Models
{
    /// <summary>
    /// 状态 分状态还是照片信息等， 以供私信和赞来区分 
    /// </summary>
    public enum StateType
    {
        None,// 没有相关，表示只是私信。
        /// <summary>
        /// 表示状态 个人独白
        /// </summary>
        Personal,//状态

        Image,//图片
        Topic,// 话题
        Comment,//评论
    }
}