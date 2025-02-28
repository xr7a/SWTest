using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(1)]
public class M0000_InitialMigration: Migration
{
    public override void Up()
    {
        Create.Table("departments")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique();
        
        Create.Table("employees")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("surname").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable().Unique()
            .WithColumn("company_id").AsInt32().NotNullable()
            .WithColumn("department_id").AsInt32().ForeignKey().NotNullable();
        
        Create.ForeignKey()
            .FromTable("employees")
            .ForeignColumn("department_id")
            .ToTable("departments")
            .PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);
        
        Create.Table("passports")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("employee_id").AsInt32().ForeignKey().NotNullable().Unique()
            .WithColumn("type").AsString().NotNullable()
            .WithColumn("number").AsString().NotNullable().Unique();

        Create.ForeignKey()
            .FromTable("passports")
            .ForeignColumn("employee_id")
            .ToTable("employees")
            .PrimaryColumn("id")
            .OnDelete(System.Data.Rule.Cascade);

        Create.UniqueConstraint()
            .OnTable("passports")
            .Columns("type", "number");
    }

    public override void Down()
    {
        Delete.Table("employees");
        Delete.Table("departments");
        Delete.Table("passports");
    }
}