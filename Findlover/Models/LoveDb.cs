using System;
using System.Collections.Generic;
using System.Data.Entity;
using Findlover.Migrations;

namespace Findlover.Models
{
    public class LoveDb:DbContext
    {
        private const string DbNameOrDbConnectionstring =
           
          "Data Source=your-pc;Initial Catalog=LoveDB;User ID=sa;Password=123;Persist Security Info=True";  
       
        public DbSet<BaseInfo> BaseInfos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<DetailInfo> DetailInfos { get; set; }
        public DbSet<LoveView> LoveViews { get; set; }
        public DbSet<InfoStatistic> InfoStatistics { get; set; }
        public DbSet<UserHot> UserHots { get; set; }
        public DbSet<Praise> Praises { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Iamgbox> Iamgboxes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Hello> Hellos { get; set; }
        public DbSet<RoleLog> RoleLogs { get; set; }
        public DbSet<Authority> Authoritys { get; set; }
        public DbSet<AdminStatistic> AdminStatistics { get; set; }
        public DbSet<VisitLog> VisitLogs { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportLog> ReportLogs { get; set; }
        public DbSet<MyLove> MyLoves { get; set; }
        public DbSet<DisLove> DisLoves { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EnjoyTopic> EnjoyTopics { get; set; }



        public LoveDb() : base(DbNameOrDbConnectionstring)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LoveDb, Configuration<LoveDb>>());
        }
    }
}