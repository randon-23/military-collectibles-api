using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilitaryCollectiblesBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAndAlteringSchema : Migration
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
                name: "ArtifactTypes",
                columns: table => new
                {
                    ArtifactTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtifactTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtifactTypes", x => x.ArtifactTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "BindingTypes",
                columns: table => new
                {
                    BindingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BindingTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BindingTypes", x => x.BindingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CaliberSpecs",
                columns: table => new
                {
                    CaliberSpecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaliberSpecName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaliberSpecs", x => x.CaliberSpecId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.EquipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Eras",
                columns: table => new
                {
                    EraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EraName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eras", x => x.EraId);
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
                name: "InsigniaTypes",
                columns: table => new
                {
                    InsigniaTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsigniaTypeName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsigniaTypes", x => x.InsigniaTypeId);
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
                name: "LiteratureTypes",
                columns: table => new
                {
                    LiteratureTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiteratureTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiteratureTypes", x => x.LiteratureTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.ManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialId);
                });

            migrationBuilder.CreateTable(
                name: "MechanicalEquipmentTypes",
                columns: table => new
                {
                    MechanicalEquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MechanicalEquipmentTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicalEquipmentTypes", x => x.MechanicalEquipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Origins",
                columns: table => new
                {
                    OriginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.OriginId);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublisherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.PublisherId);
                });

            migrationBuilder.CreateTable(
                name: "StorageArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorageAreaName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StorageAreaNotes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageArea", x => x.Id);
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
                    ArtifactTypeId = table.Column<int>(type: "int", nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: true),
                    EraId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorageArea = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artifacts_ArtifactSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "ArtifactSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Artifacts_ArtifactTypes_ArtifactTypeId",
                        column: x => x.ArtifactTypeId,
                        principalTable: "ArtifactTypes",
                        principalColumn: "ArtifactTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artifacts_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "EraId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Artifacts_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "OriginId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Artifacts_StorageArea_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "StorageArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: true),
                    EraId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorageArea = table.Column<int>(type: "int", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "EquipmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "EraId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Equipment_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Equipment_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "OriginId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Equipment_StorageArea_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "StorageArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    InsigniaTypeId = table.Column<int>(type: "int", nullable: false),
                    PartOfSet = table.Column<bool>(type: "bit", nullable: false),
                    OriginId = table.Column<int>(type: "int", nullable: true),
                    EraId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorageArea = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insignias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Insignias_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "EraId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Insignias_InsigniaSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "InsigniaSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Insignias_InsigniaTypes_InsigniaTypeId",
                        column: x => x.InsigniaTypeId,
                        principalTable: "InsigniaTypes",
                        principalColumn: "InsigniaTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Insignias_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Insignias_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "OriginId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Insignias_StorageArea_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "StorageArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    PublicationYear = table.Column<int>(type: "int", nullable: true),
                    PublisherId = table.Column<int>(type: "int", nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LiteratureTypeId = table.Column<int>(type: "int", nullable: false),
                    BindingTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorageArea = table.Column<int>(type: "int", maxLength: 200, nullable: true),
                    SeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Literatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Literatures_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Literatures_BindingTypes_BindingTypeId",
                        column: x => x.BindingTypeId,
                        principalTable: "BindingTypes",
                        principalColumn: "BindingTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Literatures_LiteratureSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "LiteratureSeries",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Literatures_LiteratureTypes_LiteratureTypeId",
                        column: x => x.LiteratureTypeId,
                        principalTable: "LiteratureTypes",
                        principalColumn: "LiteratureTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Literatures_Publishers_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Publishers",
                        principalColumn: "PublisherId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Literatures_StorageArea_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "StorageArea",
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
                    MechanicalEquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    CaliberSpecId = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    VehicleModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true),
                    EraId = table.Column<int>(type: "int", nullable: true),
                    OriginId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StorageArea = table.Column<int>(type: "int", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicalEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_CaliberSpecs_CaliberSpecId",
                        column: x => x.CaliberSpecId,
                        principalTable: "CaliberSpecs",
                        principalColumn: "CaliberSpecId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_Eras_EraId",
                        column: x => x.EraId,
                        principalTable: "Eras",
                        principalColumn: "EraId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "ManufacturerId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_MechanicalEquipmentTypes_MechanicalEquipmentTypeId",
                        column: x => x.MechanicalEquipmentTypeId,
                        principalTable: "MechanicalEquipmentTypes",
                        principalColumn: "MechanicalEquipmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "OriginId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MechanicalEquipment_StorageArea_StorageArea",
                        column: x => x.StorageArea,
                        principalTable: "StorageArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_ArtifactTypeId",
                table: "Artifacts",
                column: "ArtifactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_EraId",
                table: "Artifacts",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_OriginId",
                table: "Artifacts",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_SeriesId",
                table: "Artifacts",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_StorageArea",
                table: "Artifacts",
                column: "StorageArea");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentTypeId",
                table: "Equipment",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EraId",
                table: "Equipment",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_MaterialId",
                table: "Equipment",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_OriginId",
                table: "Equipment",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_StorageArea",
                table: "Equipment",
                column: "StorageArea");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_EraId",
                table: "Insignias",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_InsigniaTypeId",
                table: "Insignias",
                column: "InsigniaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_MaterialId",
                table: "Insignias",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_OriginId",
                table: "Insignias",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_SeriesId",
                table: "Insignias",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Insignias_StorageArea",
                table: "Insignias",
                column: "StorageArea");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_AuthorId",
                table: "Literatures",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_BindingTypeId",
                table: "Literatures",
                column: "BindingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_ISBN",
                table: "Literatures",
                column: "ISBN",
                unique: true,
                filter: "[ISBN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_LiteratureTypeId",
                table: "Literatures",
                column: "LiteratureTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_PublisherId",
                table: "Literatures",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_SeriesId",
                table: "Literatures",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Literatures_StorageArea",
                table: "Literatures",
                column: "StorageArea");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_CaliberSpecId",
                table: "MechanicalEquipment",
                column: "CaliberSpecId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_EraId",
                table: "MechanicalEquipment",
                column: "EraId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_ManufacturerId",
                table: "MechanicalEquipment",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_MaterialId",
                table: "MechanicalEquipment",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_MechanicalEquipmentTypeId",
                table: "MechanicalEquipment",
                column: "MechanicalEquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_OriginId",
                table: "MechanicalEquipment",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalEquipment_StorageArea",
                table: "MechanicalEquipment",
                column: "StorageArea");
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
                name: "ArtifactTypes");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "InsigniaSeries");

            migrationBuilder.DropTable(
                name: "InsigniaTypes");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "BindingTypes");

            migrationBuilder.DropTable(
                name: "LiteratureSeries");

            migrationBuilder.DropTable(
                name: "LiteratureTypes");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "CaliberSpecs");

            migrationBuilder.DropTable(
                name: "Eras");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MechanicalEquipmentTypes");

            migrationBuilder.DropTable(
                name: "Origins");

            migrationBuilder.DropTable(
                name: "StorageArea");
        }
    }
}
