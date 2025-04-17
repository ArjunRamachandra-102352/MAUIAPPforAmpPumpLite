using System.Net.Http.Headers;
using System.Text;

namespace MAUIAMPPUMPLITEPOC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendModifiedDataToApiAsync(string apiUrl, string jsonData)
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Data successfully sent to the API.");
            }
            else
            {
                Console.WriteLine($"Failed to send data. Status Code: {response.StatusCode}");
            }
        }

        public async Task UploadDatabaseFileAsync(string apiUrl)
        {
            // Get the SQLite file path
            string dbPath = Constants.DatabasePath;

            // Ensure the file exists
            if (!File.Exists(dbPath))
            {
                throw new FileNotFoundException("Database file not found.", dbPath);
            }

            // Add the SQLite file as a StreamContent
            using var fileStream = new FileStream(dbPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // Create the multipart form-data content
            using var formData = new MultipartFormDataContent();

            // Add the file to the form-data with a key (e.g., "file")
            formData.Add(fileContent, "file", "app.db");

            // Send the POST request to the API
            var response = await _httpClient.PostAsync(apiUrl, formData);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Database file uploaded successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to upload database file. Status Code: {response.StatusCode}");
            }
        }
    }
}


