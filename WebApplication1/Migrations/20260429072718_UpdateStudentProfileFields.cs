using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduDocFlow.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "StudentProfiles");

            migrationBuilder.RenameColumn(
                name: "EducationForm",
                table: "StudentProfiles",
                newName: "StudyForm");

            migrationBuilder.AlterColumn<int>(
                name: "Course",
                table: "StudentProfiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "EducationProgram",
                table: "StudentProfiles",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollmentDate",
                table: "StudentProfiles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDormitoryResident",
                table: "StudentProfiles",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "StudentProfiles",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationProgram",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "IsDormitoryResident",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "StudentProfiles");

            migrationBuilder.RenameColumn(
                name: "StudyForm",
                table: "StudentProfiles",
                newName: "EducationForm");

            migrationBuilder.AlterColumn<string>(
                name: "Course",
                table: "StudentProfiles",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "StudentProfiles",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
