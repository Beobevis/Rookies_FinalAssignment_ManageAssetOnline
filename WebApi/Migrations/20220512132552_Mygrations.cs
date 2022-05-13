using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class Mygrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenLogout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenLogout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DoB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Specification = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InstalledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    AssignToId = table.Column<int>(type: "int", nullable: false),
                    AssignedById = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInProgress = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_User_AssignedById",
                        column: x => x.AssignedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_User_AssignToId",
                        column: x => x.AssignToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReturnRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(type: "int", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    AcceptedById = table.Column<int>(type: "int", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_User_AcceptedById",
                        column: x => x.AcceptedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnRequest_User_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CategoryName", "CreateAt", "CreateBy", "Prefix", "UpdateAt", "UpdateBy" },
                values: new object[,]
                {
                    { 1, "Laptop", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "LA", null, null },
                    { 2, "Desktop", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "DE", null, null },
                    { 3, "Printer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "PR", null, null },
                    { 4, "Scanner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "SC", null, null },
                    { 5, "Network", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "NE", null, null }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateAt", "CreateBy", "DoB", "Firstname", "Gender", "IsDisabled", "IsFirstLogin", "JoinDate", "Lastname", "Location", "PasswordHash", "StaffCode", "Type", "UpdateAt", "UpdateBy", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1991, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ed", "Male", false, true, new DateTime(2021, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pendlebery", "Hochiminh", "$2a$11$.xgpMUQ2rBXNeJnptO3BUuhZueGgl51duYO9iXrRZN/CZzlguCr5.", "SD0001", 0, null, null, "edp" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1992, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Courtney", "Male", true, false, new DateTime(2021, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "O'Loinn", "Hochiminh", "$2a$11$pJgjTZ/fH2DwRuuCm76LEu1NqXYV3KT/huutk7GB77r/FzkMotrkO", "SD0002", 1, null, null, "courtneyo" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1991, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eudora", "Female", false, false, new DateTime(2021, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Renahan", "Hanoi", "$2a$11$pcteVmRbKqHr.KX5SgYxCeQxmJrdad0NdbBtaZ3kEp3mPEf6UkqAq", "SD0003", 0, null, null, "eudorar" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1991, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bevin", "Male", false, false, new DateTime(2021, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hugueville", "Hochiminh", "$2a$11$CTnhAGUdJVohrE5YJcp6g.BFOWqJub1X78.kh9liq928x6vu3yGz2", "SD0004", 1, null, null, "bevinh" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1998, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrew", "Male", false, true, new DateTime(2021, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Broadis", "Hochiminh", "$2a$11$oVFJ8adaxbBbgk8MAzzj8.rn00uYNl3jjhSkqhkWrAH7jLkPiWOqS", "SD0005", 1, null, null, "andrewb" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1994, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tades", "Male", false, false, new DateTime(2022, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zecchi", "Hanoi", "$2a$11$YnXpyf8dfRQyljCm3JmFrOmwI9tRygr18F5Q51.BHRgKBgybrmgXK", "SD0006", 1, null, null, "tadesz" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1999, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vernor", "Male", true, false, new DateTime(2022, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Huson", "Hochiminh", "$2a$11$wWGeEQUszA.5w0Uv3IPCrur/.hpfZZASI.qAe354Z8rwl.dJCv9/C", "SD0007", 1, null, null, "vernorh" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1990, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rufe", "Male", false, false, new DateTime(2022, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yole", "Hochiminh", "$2a$11$2oOWZOYQ2uNhyOZPdX26weuUZJVlHk6peqPrflRP09rlUZ1QN16tu", "SD0008", 1, null, null, "rufey" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1994, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Orton", "Male", true, false, new DateTime(2021, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Woodyear", "Hochiminh", "$2a$11$YRMjP2dOF0aeTLfgjocvKOTLDm8OVE1drxVVvBtDQS99uG1spG0gC", "SD0009", 1, null, null, "ortonw" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2000, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Peyter", "Male", true, false, new DateTime(2021, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carmichael", "Hochiminh", "$2a$11$GM8IWL8zDdHyZ31K9mFOuOzilOAj1VMSmbAy4xoSM9yw9zvEu5LC6", "SD0010", 1, null, null, "peyterc" },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1995, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kathy", "Female", false, true, new DateTime(2021, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pitchers", "Hanoi", "$2a$11$wjUPZStN3VSaG8oGVifMsOPSEoyNCq0Io./zDe0VgmxwS/c7k3XVi", "SD0011", 1, null, null, "kathyp" },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(1991, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beau", "Male", true, true, new DateTime(2021, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thorndycraft", "Hochiminh", "$2a$11$5NfbuXfOhmXpPWxxges8aehX2Z39pfcQvqHIpsuUWJby86VvOXdV6", "SD0012", 1, null, null, "beaut" }
                });

            migrationBuilder.InsertData(
                table: "Asset",
                columns: new[] { "Id", "AssetCode", "AssetName", "CategoryId", "CreateAt", "CreateBy", "InstalledDate", "Location", "Specification", "State", "UpdateAt", "UpdateBy" },
                values: new object[] { 1, "LA000001", "HP Laptop", 1, new DateTime(2022, 5, 12, 20, 25, 50, 817, DateTimeKind.Local).AddTicks(7540), 1, new DateTime(2022, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hochiminh", "HP Laptop", 1, null, null });

            migrationBuilder.InsertData(
                table: "Asset",
                columns: new[] { "Id", "AssetCode", "AssetName", "CategoryId", "CreateAt", "CreateBy", "InstalledDate", "Location", "Specification", "State", "UpdateAt", "UpdateBy" },
                values: new object[] { 2, "LA000002", "Dell Laptop", 1, new DateTime(2022, 5, 12, 20, 25, 50, 817, DateTimeKind.Local).AddTicks(7570), 1, new DateTime(2022, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hochiminh", "Dell Laptop", 1, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_CategoryId",
                table: "Asset",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_AssetId",
                table: "Assignment",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_AssignedById",
                table: "Assignment",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_AssignToId",
                table: "Assignment",
                column: "AssignToId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_AcceptedById",
                table: "ReturnRequest",
                column: "AcceptedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_AssetId",
                table: "ReturnRequest",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_RequestedById",
                table: "ReturnRequest",
                column: "RequestedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "ReturnRequest");

            migrationBuilder.DropTable(
                name: "TokenLogout");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
