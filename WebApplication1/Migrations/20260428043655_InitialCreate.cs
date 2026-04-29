using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EduDocFlow.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "longtext", nullable: false),
                    TemplateKind = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    LegalBasis = table.Column<string>(type: "varchar(700)", maxLength: 700, nullable: false),
                    IsRecipientRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsStudyPeriodRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsWorkplaceRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RequiresOriginalSignature = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    RecipientOrganization = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    DestinationPlace = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Purpose = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    StudyPeriodStart = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StudyPeriodEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Workplace = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    StudentComment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    EmployeeComment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRequests_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentRequests_Users_AssignedEmployeeId",
                        column: x => x.AssignedEmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentRequests_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Specialty = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Course = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    EducationForm = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    StudentStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RequestComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DocumentRequestId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestComments_DocumentRequests_DocumentRequestId",
                        column: x => x.DocumentRequestId,
                        principalTable: "DocumentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestComments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RequestStatusHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DocumentRequestId = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<string>(type: "longtext", nullable: true),
                    NewStatus = table.Column<string>(type: "longtext", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestStatusHistoryItems_DocumentRequests_DocumentRequestId",
                        column: x => x.DocumentRequestId,
                        principalTable: "DocumentRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestStatusHistoryItems_Users_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_AssignedEmployeeId",
                table: "DocumentRequests",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_DocumentTypeId",
                table: "DocumentRequests",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_Number",
                table: "DocumentRequests",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRequests_StudentId",
                table: "DocumentRequests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Code",
                table: "DocumentTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_AuthorId",
                table: "RequestComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_DocumentRequestId",
                table: "RequestComments",
                column: "DocumentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusHistoryItems_ChangedByUserId",
                table: "RequestStatusHistoryItems",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusHistoryItems_DocumentRequestId",
                table: "RequestStatusHistoryItems",
                column: "DocumentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_UserId",
                table: "StudentProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestComments");

            migrationBuilder.DropTable(
                name: "RequestStatusHistoryItems");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "DocumentRequests");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
