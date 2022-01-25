using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Antalaktiko.Models;
using SQLite;

namespace Antalaktiko.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            //get data from file db
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream embeddedDatabaseStream = assembly.GetManifestResourceStream("Antalaktiko.Antalaktiko.db");
            if (!File.Exists(dbPath))
            {
                FileStream fileStreamToWrite = File.Create(dbPath);
                embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
                embeddedDatabaseStream.CopyTo(fileStreamToWrite);
                fileStreamToWrite.Close();
            }
            //
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TK>().Wait();
            database.CreateTableAsync<Brand>().Wait();
            database.CreateTableAsync<Model>().Wait();
            database.CreateTableAsync<Part>().Wait();
            
        }

        public Task<List<TK>> GetNotesAsync()
        {
            //Get all notes.
            return database.Table<TK>().ToListAsync();
        }

        public Task<TK> GetNoteAsync(int id)
        {
            // Get a specific note.
            return database.Table<TK>()
                            .Where(i => i.OID == id)
                            .FirstOrDefaultAsync();
        }
        public Task<List<Brand>> GetBrandsAsync()
        {
            //Get all notes.
            return database.Table<Brand>().ToListAsync();
        }
        public Task<List<Model>> GetModelsAsync()
        {
            //Get all notes.
            return database.Table<Model>().ToListAsync();
        }
        public Task<List<Part>> GetPartsAsync()
        {
            //Get all notes.
            return database.Table<Part>().ToListAsync();
        }

    }
}
