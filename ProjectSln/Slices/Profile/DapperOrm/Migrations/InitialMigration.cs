using FluentMigrator;

namespace Main.Slices.Discovery.DapperOrm.Migrations
{
    [Migration(202211120001)]
    public class InitialMigration_202206160001 : Migration
    {
        public override void Down()
        {
            Delete.Table("Profile");
            Delete.Table("Days");
            Delete.Table("Location");
        }

        public override void Up()
        {
            Create.Table("Profile")
                .WithColumn("Id").AsString().NotNullable().PrimaryKey()
                .WithColumn("Username").AsString().NotNullable()
                .WithColumn("ShowInSearch").AsBoolean().WithDefaultValue(true)
                .WithColumn("ActivelyLooking").AsBoolean().WithDefaultValue(true)
                .WithColumn("Bio").AsString(int.MaxValue).WithDefaultValue("")
                .WithColumn("Role").AsString().Nullable();

            Create.Table("Location")
                .WithColumn("Id").AsString().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("ProfileId").AsString().NotNullable()
                .WithColumn("Latitude").AsDouble().Nullable()
                .WithColumn("Longitude").AsDouble().Nullable();

            Create.Table("Days")
                .WithColumn("Id").AsString().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("ProfileId").AsString().ForeignKey()
                .WithColumn("Monday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Tuesday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Wednesday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Thursday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Friday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Saturday").AsBoolean().WithDefaultValue(false)
                .WithColumn("Sunday").AsBoolean().WithDefaultValue(false);


            Create.Table("Threads")
                .WithColumn("Id").AsString().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("InitiatorId").AsString().ForeignKey()
                .WithColumn("ResponderId").AsString().ForeignKey()
                .WithColumn("ThreadState").AsInt32().WithDefaultValue(0);


            Create.Table("MessageRequests")
                .WithColumn("Id").AsString().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("ReceiverId").AsString().ForeignKey()
                .WithColumn("Message").AsString().Nullable()
                .WithColumn("Date").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("Messages")
                .WithColumn("Id").AsString().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("ThreadId").AsString().ForeignKey()
                .WithColumn("SenderId").AsString().ForeignKey()
                .WithColumn("Body").AsString(int.MaxValue).Nullable()
                .WithColumn("Date").AsDateTime().WithDefaultValue(SystemMethods.CurrentUTCDateTime);
            
        }
    }
}