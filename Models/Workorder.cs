using SQLite;

namespace MAUIAMPPUMPLITEPOC.Models
{
    // WorkOrder Table
    public class WorkOrder
    {
        [PrimaryKey, AutoIncrement]
        public int WorkOrderId { get; set; }
        public string? WorkOrderNumber { get; set; }
        public int ServiceCenterId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsSync { get; set; } = false;
    }
}
