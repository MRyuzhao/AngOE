namespace AngOE.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Name", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.User", "AuthenticationToken", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "AuthenticationTokenValidTo", c => c.DateTime());
            AddColumn("dbo.User", "ResetPasswordToken", c => c.String());
            AddColumn("dbo.User", "ResetPasswordTokenValidTo", c => c.DateTime());
            AddColumn("dbo.User", "LastLoginDate", c => c.DateTime());
            DropColumn("dbo.User", "UserName");
            DropColumn("dbo.User", "EmailConfirmed");
            DropColumn("dbo.User", "SecurityStamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "SecurityStamp", c => c.String(maxLength: 128));
            AddColumn("dbo.User", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.User", "LastLoginDate");
            DropColumn("dbo.User", "ResetPasswordTokenValidTo");
            DropColumn("dbo.User", "ResetPasswordToken");
            DropColumn("dbo.User", "AuthenticationTokenValidTo");
            DropColumn("dbo.User", "AuthenticationToken");
            DropColumn("dbo.User", "Name");
        }
    }
}
