using MAUIAMPPUMPLITEPOC.Data;
using MAUIAMPPUMPLITEPOC.Services;
using MAUIAMPPUMPLITEPOC.Views;

namespace MAUIAMPPUMPLITEPOC
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PersonPage), typeof(PersonPage));
            Routing.RegisterRoute(nameof(WorkorderPage), typeof(WorkorderPage));
            Routing.RegisterRoute(nameof(AssetPage), typeof(AssetPage));
        }

        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            // Disable the Sync button
            SyncButton.IsEnabled = false;

            try
            {

                // await SyncModifiedDataAsync(string.Empty);
                await UploadSqlFiletoServer(Constants.ApiPath);

                // Optionally, show a success message
                await DisplayAlert("Sync", "Data synced successfully!", "OK");
            }
            catch (Exception ex)
            {
                // Handle any errors during sync
                await DisplayAlert("Error", $"Sync failed: {ex.Message}", "OK");
            }
            finally
            {
                // Optionally, re-enable the button after the operation
                 SyncButton.IsEnabled = true;
            }
        }

        public async Task SyncModifiedDataAsync(string apiUrl)
        {
            var syncService = new SyncService(new DatabaseService());
            var apiService = new ApiService();

            // Fetch modified data as JSON
            string jsonData = await syncService.GetModifiedDataAsJsonAsync();

            // Send JSON data to the API
            await apiService.SendModifiedDataToApiAsync(apiUrl, jsonData);
        }

        private async Task UploadSqlFiletoServer(string apiUrl)
        {
            var apiService = new ApiService();
            
            try
            {
                var syncService = new SyncService(new DatabaseService());
                // Close the SQLite connection
                await syncService.CloseConnectionAsync();
                await apiService.UploadDatabaseFileAsync(apiUrl);
                await DisplayAlert("Success", "Database file uploaded successfully.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to upload database file: {ex.Message}", "OK");
            }
        }
    }
}
