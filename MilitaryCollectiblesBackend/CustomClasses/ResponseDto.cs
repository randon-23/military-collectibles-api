namespace MilitaryCollectiblesBackend.CustomClasses
{
    public class ResponseDto<T>
    {
        
        public required T CreatedObject { get; set; }
        public required string entityType { get; set; }
    }
}
