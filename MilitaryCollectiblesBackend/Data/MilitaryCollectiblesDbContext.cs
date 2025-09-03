using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MilitaryCollectiblesBackend.Data.Configurations;
using MilitaryCollectiblesBackend.Models;

namespace MilitaryCollectiblesBackend.Data
{
    // This class represents the database context for the Military Collectibles application. Here we define the entities that will be mapped to the database tables, as well as the field constraints, mappings and relationships between them
    // This acts as a double-layered data integrity check, ensuring that the data being stored in the database adheres to the defined structure and relationships, before it is even saved to the database, ensuring that the data in the database is consistent and valid.
    // Application throws exceptions if the data does not meet the defined constraints, preventing invalid data from being saved to SQL Server.
    // Also, by mirroring constraints in the code, emsure that application code, database schema and API all work together seamlessly, reducing the risk of errors and inconsistencies. 
    public class MilitaryCollectiblesDbContext : DbContext
    {
        public MilitaryCollectiblesDbContext(DbContextOptions<MilitaryCollectiblesDbContext> options)
            : base(options)
        { }
        public DbSet<Literature> Literatures { get; set; } //DbSet is used to represent a collection of entities in the context of Entity Framework. What is returned (Literatures) is a collection of Literature objects that can be queried or modified.
        public DbSet<Insignia> Insignias { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Equipment> Equipments  { get; set; }
        public DbSet<MechanicalEquipment> MechanicalEquipments  { get; set; }
        public DbSet<LiteratureSeries> QueriedLiteratureSeries { get; set; }
        public DbSet<ArtifactSeries> QueriedArtifactSeries  { get; set; }
        public DbSet<InsigniaSeries> QueriedInsigniaSeries { get; set; }
        public DbSet<StorageArea> StorageAreas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Best practice to use separate configuration classes for each entity. This below is alternative way to automatically apply all
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(MilitaryCollectiblesDbContext).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LiteratureConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsigniaConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArtifactConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EquipmentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArtifactConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LiteratureSeriesConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsigniaSeriesConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArtifactSeriesConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorageAreaConfiguration).Assembly);
        }
    }
}

//What "Manipulation and Deduction Logic" Do API Endpoints Handle?
//In a RESTful API, endpoints do much more than database CRUD:

//1.Input Validation and Sanitization
//API endpoints receive data from clients/users, so they must validate required fields, length, correct formats, allowlists, etc.—often using model constraints described above.

//Prevent injection(SQL, XSS) or malformed requests.

//2. Business Rules and Deduction
//API methods often implement application-level rules that go beyond simple table constraints. Examples:

//Deduct inventory when an item is purchased.

//Auto-assign a default group or status based on logic.

//Calculate derived fields before saving.

//Prevent duplicate artifacts in a collection based on business-specific uniqueness (not just database PKs).

//Enforce military-era or authenticity rules for collectibles.

//3. Computed Responses and Data Transformation
//API endpoints may combine multiple tables, shape/transform responses for the client, or hide internal -only fields.

//Example: Combine Literature and LiteratureSeries info into a single DTO for the frontend.

//4. Authorization and Security
//Control who can manipulate or read certain data, applying permission checks before passing queries to the database.

//5. Error Handling and Feedback
//Catch database errors and provide user-friendly, actionable messages rather than raw SQL exceptions.