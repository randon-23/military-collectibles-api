namespace MilitaryCollectiblesBackend.CustomClasses
{
    public class SearchFilterDtos
    {
    }

    public class LiteratureSearchFilterDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? YearPublished { get; set; }
        public int? PublicationYearFrom { get; set; }
        public int? PublicationYearTo { get; set; }
        public string? Publisher { get; set; }
        public string? ISBN { get; set; }
        public string? LiteratureType { get; set; }
        public string? BindingType { get; set; }
    }

    public class ArtifactSearchFilterDto
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? ArtifactType { get; set; }
        public string? Origin { get; set; }
        public string? Era { get; set; }
    }

    public class InsigniaSearchFilterDto
    {
        public string? Name{ get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //public string? Branch { get; set; }
        //public string? Rank { get; set; }
        public string? InsigniaType { get; set; }
        public bool? PartOfSet { get; set; }
        public string? Origin { get; set; }
        public string? Era { get; set; }
        public string? Material { get; set; }
    }

    public class EquipmentSearchFilterDto
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? EquipmentType { get; set; }
        public string? Origin { get; set; }
        public string? Era { get; set; }
        public string? Material { get; set; }
    }

    public class MechanicalEquipmentSearchFilterDto
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? MechanicalEquipmentType { get; set; }
        public string? CaliberSpec { get; set; }
        public string? VehicleModel { get; set; }
        public string? SerialNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? Origin { get; set; }
        public string? Era { get; set; }
        public string? Material { get; set; }
    }
}
