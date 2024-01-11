// DB

using System;
using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore.Sqlite;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameManager
{
    public class ItemPrefix
    {
        /// <summary>
        /// name[0] = en;
        /// name[1] = de;
        /// name[2] = it;
        /// name[3] = fr;
        /// name[4] = es;
        /// </summary>
        public List<string> name = new List<string>();
        public float damageMod;
        public float knockBackMod;
        public float speedMod;
        public float sizeMod;
        public float velocityMod;
        public float manaCostMod;
        public int critStrikeMod;
        public float sellValueMod;
    }

    public class DB
    {
        private SqliteConnection connection;
        private SqliteDataReader reader;
        private const string folderName = "db";

        public static Dictionary<string, int> Tiles = new Dictionary<string, int>();
        public static Dictionary<string, int> Buffs = new Dictionary<string, int>();
        public static Dictionary<int, ItemPrefix> ItemPrefixes = new Dictionary<int, ItemPrefix>();

        public DB()
        {
            //TEMP
            return;
            
            SqliteDataReader tempReader = Load(
                "terraria", "SELECT name, id FROM tiles");

            try
            {
                while (tempReader.Read())
                    Tiles.Add(GetString(tempReader, 0), GetInt32(tempReader, 1));
            }
            catch
            {
                return;
            }

            tempReader = Load("terraria", "SELECT name, id FROM buffs");

            while (tempReader.Read())
            {
                Buffs.Add(GetString(tempReader, 0), GetInt32(tempReader, 1));
            }

            tempReader = Load("terraria", "SELECT id, name_en, name_de, name_it, name_fr, name_es, damageMod, knockbackMod, speedMod, " + 
                "sizeMod, velocityMod, manaCostMod, critStrikeMod, sellValueMod FROM item_prefix");
            while(tempReader.Read())
            {
                ItemPrefix prefix = new ItemPrefix();
                prefix.name.Add(GetString(tempReader, 1));
                prefix.name.Add(GetString(tempReader, 2));
                prefix.name.Add(GetString(tempReader, 3));
                prefix.name.Add(GetString(tempReader, 4));
                prefix.name.Add(GetString(tempReader, 5));
                prefix.damageMod = GetFloat(tempReader, 6);
                prefix.knockBackMod = GetFloat(tempReader, 7);
                prefix.speedMod = GetFloat(tempReader, 8);
                prefix.sizeMod = GetFloat(tempReader, 9);
                prefix.velocityMod = GetFloat(tempReader, 10);
                prefix.manaCostMod = GetFloat(tempReader, 11);
                prefix.critStrikeMod = GetInt32(tempReader, 12);
                prefix.sellValueMod = GetFloat(tempReader, 13);

                ItemPrefixes.Add(GetInt32(tempReader, 0), prefix);
            }

            if (!tempReader.IsClosed)
            {
                tempReader.Dispose();
               //tempReader.Close();
            }
            CloseConnections();
        }

        private SqliteDataReader Load(string file, string query)
        {
            //string str = string.Format(@"Data Source=.\Content\{0}\{1}.sqlite;", folderName, file);
            string str = string.Format(@"Data Source={0}.db", file);


            connection = new SqliteConnection(str);
            try
            {
                
                connection.Open();
                using (SqliteCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    reader = command.ExecuteReader();
                    return reader;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] DB bug: " + ex.Message);
                //reader.Close();

                if (reader != null)
                  reader.Dispose();

                if (connection != null)
                  connection.Close();

                return null;
            }
        }

        private float GetFloat(SqliteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? 0 : reader.GetFloat(col);
        }

        private string GetString(SqliteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? string.Empty : reader.GetString(col);
        }

        private Int32 GetInt32(SqliteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? 0 : reader.GetInt32(col);
        }

        private void CloseConnections()
        {
            if (!reader.IsClosed)
            {
                //reader.Close();
                reader.Dispose();
            }

            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}
