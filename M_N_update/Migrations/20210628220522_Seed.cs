using Microsoft.EntityFrameworkCore.Migrations;

namespace M_N_update.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Nome" },
                values: new object[,]
                {
                    { 1, "A" },
                    { 2, "B" },
                    { 3, "C" },
                    { 4, "D" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "ID", "Description", "Nome" },
                values: new object[,]
                {
                    { 1, "description First Lesson", "name First Lesson" },
                    { 2, "description Second Lesson", "name Second Lesson" },
                    { 3, "description Third Lesson ", "name Third Lesson" }
                });

            migrationBuilder.InsertData(
                table: "CategoryLesson",
                columns: new[] { "CategoriesListID", "LessonListID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 4, 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryLesson",
                keyColumns: new[] { "CategoriesListID", "LessonListID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryLesson",
                keyColumns: new[] { "CategoriesListID", "LessonListID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryLesson",
                keyColumns: new[] { "CategoriesListID", "LessonListID" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "CategoryLesson",
                keyColumns: new[] { "CategoriesListID", "LessonListID" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "CategoryLesson",
                keyColumns: new[] { "CategoriesListID", "LessonListID" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lessons",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
