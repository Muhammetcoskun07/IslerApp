namespace IslerApp.Models
{
    public class IslerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsBaslik { get; set; }
        public int StatusId { get; set; }
        public string Detay { get; set; }
        public string Status { get; set; }
        public string? Active { get; set; }
        public string? Tamamlandi { get; set; }
        public string? Total { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
