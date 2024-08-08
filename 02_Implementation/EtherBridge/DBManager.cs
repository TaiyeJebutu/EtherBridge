using EtherXMLReader;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherBridge
{
    public class DBManager
    {
        private const string _database = "database.db";
        public DBManager(){
            
        }
        
        private void RemoveOldDatabase()
        {
            if (File.Exists(_database))
            {
                Console.WriteLine("Removing Old Database");
                File.Delete(_database);
            }
        }

        public void CreateTable(List<String> tables)
        {

            // Remove old file if it exists
            RemoveOldDatabase();
            
            using (var conn = new SqliteConnection($"Data Source={DBManager._database}"))
            {
                conn.Open();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();

                foreach (String table in tables)
                {
                    try
                    {
                        sqlite_cmd.CommandText = table;
                        sqlite_cmd.ExecuteNonQuery();
                    }catch (SqliteException ex)
                    {
                        //
                    }
                    
                }
                conn.Close();
                
            }
            Console.WriteLine("Database Tables created");
        }

        public void AddTranslatedMessage(TranslatedMessage message)
        {
            using (var conn = new SqliteConnection($"Data Source={DBManager._database}"))
            {
                // Get table name
                string tableName = message.name;
                // Get field names
                string fieldNames = $"(";
                string fieldValues = $"(";
                foreach (KeyValuePair<string, string> field in message.fields)
                {
                    fieldNames += $"{field.Key},";
                    fieldValues += $"{field.Value},";
                }

                fieldNames = fieldNames.Remove(fieldNames.Length - 1);
                fieldValues = fieldValues.Remove(fieldValues.Length - 1);

                fieldNames += ")";
                fieldValues += ");";


                conn.Open();
                SqliteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = $"INSERT INTO {tableName} {fieldNames} VALUES{fieldValues}";
                sqlite_cmd.ExecuteNonQuery();
                conn.Close();
                
            }
        }

        public string CreateDatabaseTableString(XMLICDMessage icdMessage)
        {
            string tableName = icdMessage.Name;
            List<string> fields = new List<string>();
            foreach (XMLFields field in icdMessage.Fields)
            {
                if (field._name != null) fields.Add(field._name);

            }

            string table = $"CREATE TABLE {tableName} (";
            foreach (string field in fields)
            {
                table += $"{field} DOUBLE NULL DEFAULT NULL,";
            }

            table = table.Remove(table.Length - 1);
            table += ");";

            return table;

        }


        public bool Query(string query)
        {
            using (var conn = new SqliteConnection($"Data Source={DBManager._database}"))
            {                
                conn.Open();
                SqliteCommand sqlite_cmd =  new SqliteCommand(query, conn);              
                
                long count = (long)sqlite_cmd.ExecuteScalar();
                conn.Close();

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
    }
}
