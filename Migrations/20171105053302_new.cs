using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace justibot_server.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    adminId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    staffMember = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.adminId);
                });

            migrationBuilder.CreateTable(
                name: "blacklists",
                columns: table => new
                {
                    blacklistID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bserver = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blacklists", x => x.blacklistID);
                });

            migrationBuilder.CreateTable(
                name: "Givaways",
                columns: table => new
                {
                    GiveawayId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Gchannel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GservID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    prize = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Givaways", x => x.GiveawayId);
                });

            migrationBuilder.CreateTable(
                name: "globalusersxp",
                columns: table => new
                {
                    globaluserxpID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Xp = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_globalusersxp", x => x.globaluserxpID);
                });

            migrationBuilder.CreateTable(
                name: "leavingMessages",
                columns: table => new
                {
                    LeavingMessageID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    leavingGuild = table.Column<ulong>(type: "INTEGER", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leavingMessages", x => x.LeavingMessageID);
                });

            migrationBuilder.CreateTable(
                name: "musics",
                columns: table => new
                {
                    musicId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tname = table.Column<string>(type: "TEXT", nullable: true),
                    Tserver = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musics", x => x.musicId);
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    NoteID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    User = table.Column<ulong>(type: "INTEGER", nullable: false),
                    nServId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.NoteID);
                });

            migrationBuilder.CreateTable(
                name: "prefixes",
                columns: table => new
                {
                    ServPrefixID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    prefix = table.Column<char>(type: "INTEGER", nullable: false),
                    prefixGuild = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prefixes", x => x.ServPrefixID);
                });

            migrationBuilder.CreateTable(
                name: "rewards",
                columns: table => new
                {
                    rewardID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rewardlvl = table.Column<int>(type: "INTEGER", nullable: false),
                    rewardrole = table.Column<ulong>(type: "INTEGER", nullable: false),
                    rserver = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rewards", x => x.rewardID);
                });

            migrationBuilder.CreateTable(
                name: "ServerPerms",
                columns: table => new
                {
                    ServerPermID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PServId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    PermActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    PermArg = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Pmode = table.Column<string>(type: "TEXT", nullable: true),
                    SPerm = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerPerms", x => x.ServerPermID);
                });

            migrationBuilder.CreateTable(
                name: "serversxp",
                columns: table => new
                {
                    serverxpID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xp = table.Column<int>(type: "INTEGER", nullable: false),
                    xServId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serversxp", x => x.serverxpID);
                });

            migrationBuilder.CreateTable(
                name: "staffRoles",
                columns: table => new
                {
                    staffRoleID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermArg = table.Column<ulong>(type: "INTEGER", nullable: false),
                    RServId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staffRoles", x => x.staffRoleID);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tactive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tchannel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Thost = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Tmax = table.Column<int>(type: "INTEGER", nullable: false),
                    TservID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    joinable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                });

            migrationBuilder.CreateTable(
                name: "versionChecks",
                columns: table => new
                {
                    versionControlID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    version = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_versionChecks", x => x.versionControlID);
                });

            migrationBuilder.CreateTable(
                name: "welcomeMessages",
                columns: table => new
                {
                    WelcomeMessageID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    message = table.Column<string>(type: "TEXT", nullable: true),
                    welcomeGuild = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_welcomeMessages", x => x.WelcomeMessageID);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    EntryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GiveawayId = table.Column<int>(type: "INTEGER", nullable: false),
                    entrentID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ishost = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryID);
                    table.ForeignKey(
                        name: "FK_Entries_Givaways_GiveawayId",
                        column: x => x.GiveawayId,
                        principalTable: "Givaways",
                        principalColumn: "GiveawayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serverusersxp",
                columns: table => new
                {
                    serveruserxpID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Xp = table.Column<int>(type: "INTEGER", nullable: false),
                    serverxpID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serverusersxp", x => x.serveruserxpID);
                    table.ForeignKey(
                        name: "FK_serverusersxp_serversxp_serverxpID",
                        column: x => x.serverxpID,
                        principalTable: "serversxp",
                        principalColumn: "serverxpID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TMatches",
                columns: table => new
                {
                    TmatchID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tmatchno = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: true),
                    Troundno = table.Column<int>(type: "INTEGER", nullable: false),
                    Tteam1ID = table.Column<int>(type: "INTEGER", nullable: false),
                    Tteam2ID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMatches", x => x.TmatchID);
                    table.ForeignKey(
                        name: "FK_TMatches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TPlayers",
                columns: table => new
                {
                    TPlayerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    isLeader = table.Column<bool>(type: "INTEGER", nullable: false),
                    lost = table.Column<bool>(type: "INTEGER", nullable: false),
                    playerID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    state = table.Column<int>(type: "INTEGER", nullable: false),
                    teamNo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPlayers", x => x.TPlayerID);
                    table.ForeignKey(
                        name: "FK_TPlayers_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_GiveawayId",
                table: "Entries",
                column: "GiveawayId");

            migrationBuilder.CreateIndex(
                name: "IX_serverusersxp_serverxpID",
                table: "serverusersxp",
                column: "serverxpID");

            migrationBuilder.CreateIndex(
                name: "IX_TMatches_TournamentId",
                table: "TMatches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TPlayers_TournamentId",
                table: "TPlayers",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "blacklists");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "globalusersxp");

            migrationBuilder.DropTable(
                name: "leavingMessages");

            migrationBuilder.DropTable(
                name: "musics");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "prefixes");

            migrationBuilder.DropTable(
                name: "rewards");

            migrationBuilder.DropTable(
                name: "ServerPerms");

            migrationBuilder.DropTable(
                name: "serverusersxp");

            migrationBuilder.DropTable(
                name: "staffRoles");

            migrationBuilder.DropTable(
                name: "TMatches");

            migrationBuilder.DropTable(
                name: "TPlayers");

            migrationBuilder.DropTable(
                name: "versionChecks");

            migrationBuilder.DropTable(
                name: "welcomeMessages");

            migrationBuilder.DropTable(
                name: "Givaways");

            migrationBuilder.DropTable(
                name: "serversxp");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
