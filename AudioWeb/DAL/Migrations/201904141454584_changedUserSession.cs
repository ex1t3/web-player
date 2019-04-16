namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedUserSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSessions", "UserAgent", c => c.String());
            AddColumn("dbo.UserSessions", "IpAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSessions", "IpAddress");
            DropColumn("dbo.UserSessions", "UserAgent");
        }
    }
}
