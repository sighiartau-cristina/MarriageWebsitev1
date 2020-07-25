namespace BusinessModel.Entities
{
    public class ReligionEntity
    {
        public int ReligionId { get; set; }

        public string ReligionName { get; set; }

        override
        public string ToString()
        {
            return this.ReligionName;
        }
    }
}
