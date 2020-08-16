using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace justibot_server.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roleMessages",
                columns: table => new
                {
                    roleMessageId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<ulong>(nullable: false),
                    guildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleMessages", x => x.roleMessageId);
                });

            migrationBuilder.CreateTable(
                name: "roleReactions",
                columns: table => new
                {
                    roleReactionId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reaction = table.Column<string>(nullable: true),
                    roleId = table.Column<ulong>(nullable: false),
                    guildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleReactions", x => x.roleReactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roleMessages");

            migrationBuilder.DropTable(
                name: "roleReactions");
        }
    }
}
