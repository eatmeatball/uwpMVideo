using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static SimpleM.MainPage;

namespace SimpleM
{
    static public class DataAccess
    {
        public async static void InitializeDatabase()
        {

            await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            /*await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.ReplaceExisting);*/
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "name NVARCHAR(2048) NULL," +
                    "path NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string name, string path)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand
                {
                    Connection = db,

                    // Use parameterized query to prevent SQL injection attacks
                    CommandText = "INSERT INTO MyTable VALUES (NULL, @name, @path);"
                };
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@path", path);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<VideoFileInfoData> GetData()
        {
            List<String> entries = new List<string>();
            List<VideoFileInfoData> videoFileInfoDataList = new List<VideoFileInfoData>();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT name,path from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                    videoFileInfoDataList.Add(new VideoFileInfoData(query.GetString(0), query.GetString(1)));
                }

                db.Close();
            }

            return videoFileInfoDataList;
        }

        public static bool DelAllData()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
   new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand delCommand = new SqliteCommand
                    ("Delete from MyTable", db);

                SqliteDataReader query = delCommand.ExecuteReader();

                db.Close();
            }
            return true;
        }
    }
}
