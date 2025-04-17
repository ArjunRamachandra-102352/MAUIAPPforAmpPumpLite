using MAUIAMPPUMPLITEPOC.Data;
using System.Text.Json;

namespace MAUIAMPPUMPLITEPOC.Services
{
    public class SyncService
    {
        private readonly DatabaseService _databaseService;

        public SyncService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task CloseConnectionAsync() {
            await _databaseService.CloseConnectionAsync();
        }

        public async Task<string> GetModifiedDataAsJsonAsync()
        {
            // Fetch modified data
            var modifiedPersons = await _databaseService.GetModifiedPersonsAsync();
            var modifiedWorkOrders = await _databaseService.GetModifiedWorkOrdersAsync();
            var modifiedAssets = await _databaseService.GetModifiedAssetsAsync();

            // Structure the data
            var recentlyUpdated = new List<object>
        {
            new
            {
                table = "Person",
                data = modifiedPersons.Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Address,
                    p.State,
                    p.City,
                    p.Pincode
                }).ToList()
            },
            new
            {
                table = "WorkOrder",
                data = modifiedWorkOrders.Select(w => new
                {
                    w.WorkOrderId,
                    w.WorkOrderNumber,
                    w.ServiceCenterId
                }).ToList()
            },
            new
            {
                table = "Asset",
                data = modifiedAssets.Select(a => new
                {
                    a.AssetId,
                    a.SerialNumber
                }).ToList()
            }
        };

            // Serialize to JSON
            var jsonData = new
            {
                recentlyUpdated
            };

            return JsonSerializer.Serialize(jsonData, new JsonSerializerOptions
            {
                WriteIndented = true // For pretty printing (optional)
            });
        }
    }
}
