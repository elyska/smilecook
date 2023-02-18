using CommunityToolkit.Maui.Core.Extensions;
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
    public class DBService
    {
        protected string _dbPath;

        protected SQLiteConnection conn;
        public DBService(string dbPath)
        {
            _dbPath = dbPath;
        }

    }
}
