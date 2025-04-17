using SQLite;
using MAUIAMPPUMPLITEPOC.Models;

namespace MAUIAMPPUMPLITEPOC.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            if (_database is not null)
                return;

            // Initialize the SQLite connection
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

            // Create tables
            _database.CreateTableAsync<Person>().Wait();
            _database.CreateTableAsync<WorkOrder>().Wait();
            _database.CreateTableAsync<Asset>().Wait();
        }

        // Create or Update a Person
        // Generic Save Method
        public Task<int> SaveItemAsync<T>(T item) where T : new()
        { 
            return _database.InsertAsync(item);
        }

        public Task<int> UpdateItemAsync<T>(T item) where T : new()
        {
            return _database.UpdateAsync(item);
        }

        // Generic Delete Method
        public Task<int> DeleteItemAsync<T>(T item) where T : new()
        {
            return _database.DeleteAsync(item);
        }

        // Generic Get All Items Method
        public async Task<List<T>> GetItemsAsync<T>() where T : new()
        {
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<List<Person>> GetModifiedPersonsAsync()
        {
            return await _database.Table<Person>()
                                  .Where(p => p.IsSync == false)
                                  .ToListAsync();
        }

        public async Task<List<WorkOrder>> GetModifiedWorkOrdersAsync()
        {
            return await _database.Table<WorkOrder>()
                                  .Where(w => w.IsSync == false)
                                  .ToListAsync();
        }

        public async Task<List<Asset>> GetModifiedAssetsAsync()
        {
            return await _database.Table<Asset>()
                                  .Where(a => a.IsSync == false)
                                  .ToListAsync();
        }

        public async Task CloseConnectionAsync()
        {
            if (_database != null)
            {
                await _database.CloseAsync();
                _database = null;
            }
        }
    }
}
