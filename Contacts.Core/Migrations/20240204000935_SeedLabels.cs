using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Contacts.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedLabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Labels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Electronic" },
                    { 2, "Reggae" },
                    { 3, "Electronic" },
                    { 4, "Country" },
                    { 5, "Stage And Screen" },
                    { 6, "Rap" },
                    { 7, "Classical" },
                    { 8, "Latin" },
                    { 9, "Latin" },
                    { 10, "Country" },
                    { 11, "Country" },
                    { 12, "Latin" },
                    { 13, "Electronic" },
                    { 14, "Funk" },
                    { 15, "Rap" },
                    { 16, "Metal" },
                    { 17, "Rap" },
                    { 18, "Non Music" },
                    { 19, "Pop" },
                    { 20, "Jazz" }
                });

            migrationBuilder.InsertData(
                table: "ContactLabel",
                columns: new[] { "ContactId", "LabelId" },
                values: new object[,]
                {
                    { 3, 2 },
                    { 6, 17 },
                    { 9, 12 },
                    { 9, 19 },
                    { 12, 14 },
                    { 15, 9 },
                    { 18, 4 },
                    { 18, 11 },
                    { 21, 6 },
                    { 24, 1 },
                    { 26, 2 },
                    { 27, 16 },
                    { 30, 17 },
                    { 33, 12 },
                    { 35, 14 },
                    { 36, 7 },
                    { 39, 9 },
                    { 42, 4 },
                    { 44, 6 },
                    { 45, 19 },
                    { 48, 1 },
                    { 50, 3 },
                    { 51, 16 },
                    { 53, 18 },
                    { 54, 11 },
                    { 57, 13 },
                    { 59, 14 },
                    { 60, 8 },
                    { 62, 9 },
                    { 63, 3 },
                    { 66, 4 },
                    { 68, 6 },
                    { 69, 19 },
                    { 71, 1 },
                    { 72, 14 },
                    { 74, 16 },
                    { 77, 18 },
                    { 78, 11 },
                    { 80, 13 },
                    { 81, 6 },
                    { 83, 8 },
                    { 86, 10 },
                    { 87, 3 },
                    { 89, 5 },
                    { 90, 18 },
                    { 92, 20 },
                    { 95, 1 },
                    { 96, 15 },
                    { 98, 16 },
                    { 101, 11 },
                    { 104, 13 },
                    { 105, 6 },
                    { 107, 8 },
                    { 110, 3 },
                    { 113, 5 },
                    { 114, 18 },
                    { 116, 20 },
                    { 119, 15 },
                    { 122, 10 },
                    { 122, 17 },
                    { 125, 12 },
                    { 128, 7 },
                    { 131, 2 },
                    { 131, 8 },
                    { 134, 3 },
                    { 137, 18 },
                    { 140, 13 },
                    { 140, 20 },
                    { 143, 15 },
                    { 146, 10 },
                    { 149, 5 },
                    { 149, 12 },
                    { 152, 7 },
                    { 155, 2 },
                    { 155, 9 },
                    { 158, 4 },
                    { 158, 17 },
                    { 161, 19 },
                    { 164, 14 },
                    { 164, 20 },
                    { 167, 8 },
                    { 167, 15 },
                    { 170, 10 },
                    { 173, 5 },
                    { 173, 12 },
                    { 176, 7 },
                    { 176, 20 },
                    { 179, 2 },
                    { 182, 4 },
                    { 182, 17 },
                    { 185, 12 },
                    { 185, 19 },
                    { 188, 14 },
                    { 191, 9 },
                    { 191, 16 },
                    { 194, 4 },
                    { 194, 11 },
                    { 197, 5 },
                    { 200, 7 },
                    { 200, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 6, 17 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 9, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 9, 19 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 12, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 15, 9 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 18, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 18, 11 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 21, 6 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 26, 2 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 27, 16 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 30, 17 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 33, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 35, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 36, 7 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 39, 9 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 42, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 44, 6 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 45, 19 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 48, 1 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 50, 3 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 51, 16 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 53, 18 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 54, 11 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 57, 13 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 59, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 60, 8 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 62, 9 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 63, 3 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 66, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 68, 6 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 69, 19 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 71, 1 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 72, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 74, 16 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 77, 18 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 78, 11 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 80, 13 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 81, 6 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 83, 8 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 86, 10 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 87, 3 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 89, 5 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 90, 18 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 92, 20 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 95, 1 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 96, 15 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 98, 16 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 101, 11 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 104, 13 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 105, 6 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 107, 8 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 110, 3 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 113, 5 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 114, 18 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 116, 20 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 119, 15 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 122, 10 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 122, 17 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 125, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 128, 7 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 131, 2 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 131, 8 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 134, 3 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 137, 18 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 140, 13 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 140, 20 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 143, 15 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 146, 10 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 149, 5 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 149, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 152, 7 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 155, 2 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 155, 9 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 158, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 158, 17 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 161, 19 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 164, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 164, 20 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 167, 8 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 167, 15 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 170, 10 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 173, 5 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 173, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 176, 7 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 176, 20 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 179, 2 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 182, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 182, 17 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 185, 12 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 185, 19 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 188, 14 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 191, 9 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 191, 16 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 194, 4 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 194, 11 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 197, 5 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 200, 7 });

            migrationBuilder.DeleteData(
                table: "ContactLabel",
                keyColumns: new[] { "ContactId", "LabelId" },
                keyValues: new object[] { 200, 20 });

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Labels",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
