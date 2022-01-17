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
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TK>().Wait();
            //get data from file db
            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream embeddedDatabaseStream = assembly.GetManifestResourceStream("Antalaktiko.Antalaktiko.db");
            if (File.Exists(dbPath))
            {
                FileStream fileStreamToWrite = File.Create(dbPath);
                embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
                embeddedDatabaseStream.CopyTo(fileStreamToWrite);
                fileStreamToWrite.Close();
            }
            //
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
        
    }
}
