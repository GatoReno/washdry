using System;
using System.Collections.Generic;
using System.Text;

namespace WashDry.Interface
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
