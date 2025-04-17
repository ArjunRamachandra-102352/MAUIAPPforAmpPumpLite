using SQLite;

namespace MAUIAMPPUMPLITEPOC.Models
{
    // Asset Table
    public class Asset
    {
        [PrimaryKey, AutoIncrement]
        public int AssetId { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsSync { get; set; } = false;
    }
}
