using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormulaFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_CreatedByUserId",
                table: "NetworkCanvas");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_OwnerUserId",
                table: "NetworkCanvas");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_UpdatedByUserId",
                table: "NetworkCanvas");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCard_AspNetUsers_CreatedByUserId",
                table: "NetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCard_AspNetUsers_UpdatedByUserId",
                table: "NetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCard_NetworkCanvas_NetworkCanvasId",
                table: "NetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCardToNetworkCard_AspNetUsers_CreatedByUserId",
                table: "NetworkCardToNetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCardToNetworkCard_AspNetUsers_UpdatedByUserId",
                table: "NetworkCardToNetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCardToNetworkCard_NetworkCard_From",
                table: "NetworkCardToNetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkCardToNetworkCard_NetworkCard_To",
                table: "NetworkCardToNetworkCard");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkParameter_AspNetUsers_CreatedByUserId",
                table: "NetworkParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkParameter_AspNetUsers_UpdatedByUserId",
                table: "NetworkParameter");

            migrationBuilder.DropForeignKey(
                name: "FK_NetworkParameter_NetworkCard_NetworkCardId",
                table: "NetworkParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkParameter",
                table: "NetworkParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkCardToNetworkCard",
                table: "NetworkCardToNetworkCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkCard",
                table: "NetworkCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkCanvas",
                table: "NetworkCanvas");

            migrationBuilder.RenameTable(
                name: "NetworkParameter",
                newName: "Parameters");

            migrationBuilder.RenameTable(
                name: "NetworkCardToNetworkCard",
                newName: "CardsToCards");

            migrationBuilder.RenameTable(
                name: "NetworkCard",
                newName: "Cards");

            migrationBuilder.RenameTable(
                name: "NetworkCanvas",
                newName: "Canvases");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkParameter_UpdatedByUserId",
                table: "Parameters",
                newName: "IX_Parameters_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkParameter_NetworkCardId",
                table: "Parameters",
                newName: "IX_Parameters_NetworkCardId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkParameter_CreatedByUserId",
                table: "Parameters",
                newName: "IX_Parameters_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCardToNetworkCard_UpdatedByUserId",
                table: "CardsToCards",
                newName: "IX_CardsToCards_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCardToNetworkCard_To",
                table: "CardsToCards",
                newName: "IX_CardsToCards_To");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCardToNetworkCard_From_To",
                table: "CardsToCards",
                newName: "IX_CardsToCards_From_To");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCardToNetworkCard_CreatedByUserId",
                table: "CardsToCards",
                newName: "IX_CardsToCards_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCard_UpdatedByUserId",
                table: "Cards",
                newName: "IX_Cards_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCard_NetworkCanvasId",
                table: "Cards",
                newName: "IX_Cards_NetworkCanvasId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCard_CreatedByUserId",
                table: "Cards",
                newName: "IX_Cards_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCanvas_UpdatedByUserId",
                table: "Canvases",
                newName: "IX_Canvases_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCanvas_OwnerUserId",
                table: "Canvases",
                newName: "IX_Canvases_OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkCanvas_CreatedByUserId",
                table: "Canvases",
                newName: "IX_Canvases_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parameters",
                table: "Parameters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardsToCards",
                table: "CardsToCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canvases",
                table: "Canvases",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Canvases_AspNetUsers_CreatedByUserId",
                table: "Canvases",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Canvases_AspNetUsers_OwnerUserId",
                table: "Canvases",
                column: "OwnerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Canvases_AspNetUsers_UpdatedByUserId",
                table: "Canvases",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_CreatedByUserId",
                table: "Cards",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_UpdatedByUserId",
                table: "Cards",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Canvases_NetworkCanvasId",
                table: "Cards",
                column: "NetworkCanvasId",
                principalTable: "Canvases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardsToCards_AspNetUsers_CreatedByUserId",
                table: "CardsToCards",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardsToCards_AspNetUsers_UpdatedByUserId",
                table: "CardsToCards",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardsToCards_Cards_From",
                table: "CardsToCards",
                column: "From",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardsToCards_Cards_To",
                table: "CardsToCards",
                column: "To",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_AspNetUsers_CreatedByUserId",
                table: "Parameters",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_AspNetUsers_UpdatedByUserId",
                table: "Parameters",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Cards_NetworkCardId",
                table: "Parameters",
                column: "NetworkCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canvases_AspNetUsers_CreatedByUserId",
                table: "Canvases");

            migrationBuilder.DropForeignKey(
                name: "FK_Canvases_AspNetUsers_OwnerUserId",
                table: "Canvases");

            migrationBuilder.DropForeignKey(
                name: "FK_Canvases_AspNetUsers_UpdatedByUserId",
                table: "Canvases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_CreatedByUserId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_UpdatedByUserId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Canvases_NetworkCanvasId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardsToCards_AspNetUsers_CreatedByUserId",
                table: "CardsToCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardsToCards_AspNetUsers_UpdatedByUserId",
                table: "CardsToCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardsToCards_Cards_From",
                table: "CardsToCards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardsToCards_Cards_To",
                table: "CardsToCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_AspNetUsers_CreatedByUserId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_AspNetUsers_UpdatedByUserId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Cards_NetworkCardId",
                table: "Parameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parameters",
                table: "Parameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardsToCards",
                table: "CardsToCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Canvases",
                table: "Canvases");

            migrationBuilder.RenameTable(
                name: "Parameters",
                newName: "NetworkParameter");

            migrationBuilder.RenameTable(
                name: "CardsToCards",
                newName: "NetworkCardToNetworkCard");

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "NetworkCard");

            migrationBuilder.RenameTable(
                name: "Canvases",
                newName: "NetworkCanvas");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_UpdatedByUserId",
                table: "NetworkParameter",
                newName: "IX_NetworkParameter_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_NetworkCardId",
                table: "NetworkParameter",
                newName: "IX_NetworkParameter_NetworkCardId");

            migrationBuilder.RenameIndex(
                name: "IX_Parameters_CreatedByUserId",
                table: "NetworkParameter",
                newName: "IX_NetworkParameter_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CardsToCards_UpdatedByUserId",
                table: "NetworkCardToNetworkCard",
                newName: "IX_NetworkCardToNetworkCard_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CardsToCards_To",
                table: "NetworkCardToNetworkCard",
                newName: "IX_NetworkCardToNetworkCard_To");

            migrationBuilder.RenameIndex(
                name: "IX_CardsToCards_From_To",
                table: "NetworkCardToNetworkCard",
                newName: "IX_NetworkCardToNetworkCard_From_To");

            migrationBuilder.RenameIndex(
                name: "IX_CardsToCards_CreatedByUserId",
                table: "NetworkCardToNetworkCard",
                newName: "IX_NetworkCardToNetworkCard_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_UpdatedByUserId",
                table: "NetworkCard",
                newName: "IX_NetworkCard_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_NetworkCanvasId",
                table: "NetworkCard",
                newName: "IX_NetworkCard_NetworkCanvasId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_CreatedByUserId",
                table: "NetworkCard",
                newName: "IX_NetworkCard_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Canvases_UpdatedByUserId",
                table: "NetworkCanvas",
                newName: "IX_NetworkCanvas_UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Canvases_OwnerUserId",
                table: "NetworkCanvas",
                newName: "IX_NetworkCanvas_OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Canvases_CreatedByUserId",
                table: "NetworkCanvas",
                newName: "IX_NetworkCanvas_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkParameter",
                table: "NetworkParameter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkCardToNetworkCard",
                table: "NetworkCardToNetworkCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkCard",
                table: "NetworkCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkCanvas",
                table: "NetworkCanvas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_CreatedByUserId",
                table: "NetworkCanvas",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_OwnerUserId",
                table: "NetworkCanvas",
                column: "OwnerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCanvas_AspNetUsers_UpdatedByUserId",
                table: "NetworkCanvas",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCard_AspNetUsers_CreatedByUserId",
                table: "NetworkCard",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCard_AspNetUsers_UpdatedByUserId",
                table: "NetworkCard",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCard_NetworkCanvas_NetworkCanvasId",
                table: "NetworkCard",
                column: "NetworkCanvasId",
                principalTable: "NetworkCanvas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCardToNetworkCard_AspNetUsers_CreatedByUserId",
                table: "NetworkCardToNetworkCard",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCardToNetworkCard_AspNetUsers_UpdatedByUserId",
                table: "NetworkCardToNetworkCard",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCardToNetworkCard_NetworkCard_From",
                table: "NetworkCardToNetworkCard",
                column: "From",
                principalTable: "NetworkCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkCardToNetworkCard_NetworkCard_To",
                table: "NetworkCardToNetworkCard",
                column: "To",
                principalTable: "NetworkCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkParameter_AspNetUsers_CreatedByUserId",
                table: "NetworkParameter",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkParameter_AspNetUsers_UpdatedByUserId",
                table: "NetworkParameter",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkParameter_NetworkCard_NetworkCardId",
                table: "NetworkParameter",
                column: "NetworkCardId",
                principalTable: "NetworkCard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
