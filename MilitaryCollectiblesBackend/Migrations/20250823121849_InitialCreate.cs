using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilitaryCollectiblesBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtifactSeries",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactSeries", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "InsigniaSeries",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsigniaSeries", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "LiteratureSeries",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureSeries", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LocationNotes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ArtifactType = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Era = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                    table.CheckConstraint("chk_ArtifactType", "ArtifactType IN ('Photograph', 'Poster', 'Document')");
                    table.ForeignKey(
                        name: "FK_Artifacts_ArtifactSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "ArtifactSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Artifacts_Locations_Location",
                        column: x => x.Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EquipmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Era = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<int>(type: "int", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.CheckConstraint("chk_EquipmentType", "EquipmentType IN ('Uniform', 'Armour', 'Inventory')");
                    table.ForeignKey(
                        name: "FK_Equipment_Locations_Location",
                        column: x => x.Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Insignias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InsigniaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PartOfSet = table.Column<bool>(type: "bit", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Era = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insignias", x => x.Id);
                    table.CheckConstraint("chk_InsigniaType", "InsigniaType IN  ('Badge', 'Regimental Badge', 'Lapel Badge', 'Ribbon')");
                    table.ForeignKey(
                        name: "FK_Insignias_InsigniaSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "InsigniaSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Insignias_Locations_Location",
                        column: x => x.Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Literatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublicationYear = table.Column<int>(type: "int", nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LiteratureType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BindingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literatures", x => x.Id);
                    table.CheckConstraint("chk_BindingType", "BindingType IN ('Paperback', 'Hardback')");
                    table.CheckConstraint("chk_LiteratureType", "LiteratureType IN ('Book', 'Magazine')");
                    table.ForeignKey(
                        name: "FK_Literatures_LiteratureSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "LiteratureSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Literatures_Locations_Location",
                        column: x => x.Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MechanicalEquipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MechanicalEquipmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CaliberSpec = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Era = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<int>(type: "int", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicalEquipment", x => x.Id);
                    table.CheckConstraint("chk_MechanicalEquipmentType", "MechanicalEquipmentType IN ('Ordinance', 'Weapon', 'Projectile', 'Vehicular')");
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_Locations_Location",
                        column: x => x.Location,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_Location",
                table: "Artifacts",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_SeriesId",
                table: "Artifacts",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Location",
                table: "Equipment",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_Location",
                table: "Insignias",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_SeriesId",
                table: "Insignias",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_ISBN",
                table: "Literatures",
                column: "ISBN",
                unique: true,
                filter: "[ISBN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_Location",
                table: "Literatures",
                column: "Location");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_SeriesId",
                table: "Literatures",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_Location",
                table: "MechanicalEquipment",
                column: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Insignias");

            migrationBuilder.DropTable(
                name: "Literatures");

            migrationBuilder.DropTable(
                name: "MechanicalEquipment");

            migrationBuilder.DropTable(
                name: "ArtifactSeries");

            migrationBuilder.DropTable(
                name: "InsigniaSeries");

            migrationBuilder.DropTable(
                name: "LiteratureSeries");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
