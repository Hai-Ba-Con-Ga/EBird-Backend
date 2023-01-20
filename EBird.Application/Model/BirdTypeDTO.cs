namespace EBird.Application.Model
{
    public class BirdTypeDTO
    {
        public Guid? Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public DateTime CreatedDatetime { get; set; }
    }
}
