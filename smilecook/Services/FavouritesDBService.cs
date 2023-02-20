using CommunityToolkit.Maui.Core.Extensions;
using smilecook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Services
{
    public class FavouritesDBService : DBService
    {
        public FavouritesDBService(string dbpath) : base(dbpath) 
        {

        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Favourite>();
        }

        public void InsertFavourite(string url, string label)
        {
            int result = 0;
            try
            {
                Init();

                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(label))
                    throw new Exception("Valid url and label required");

                result = conn.Insert(new Favourite { Url = url, Label = label });

                Debug.WriteLine($"{result} record(s) added (Label: {label}, Url: {url})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add {url}. Error: {ex.Message}");
            }
        }

        public bool IsFavourite(string url)
        {
            var result = conn.Table<Favourite>().Where(i => i.Url == url);
            Debug.WriteLine(result);

            return true;
        }

        public int DeleteFavourite(int id)
        {
            try
            {
                Init();
                int rowsAffected = conn.Delete<Favourite>(id);

                Debug.WriteLine($"{rowsAffected} record(s) deleted (Id: {id})");
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to delete {id}. Error: {ex.Message}");
            }
            return 0;
        }

        public ObservableCollection<Favourite> GetAllFavourites()
        {
            try
            {
                Init();
                return conn.Table<Favourite>().ToObservableCollection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}");
            }

            return new ObservableCollection<Favourite>();
        }
    }
}
