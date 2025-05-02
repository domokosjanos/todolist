using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiToDoList.Model.adatbazis.tablak;
using SQLite;

namespace MauiToDoList
{

    public class Szolgaltatas
    {
        private SQLiteConnection _adatbazis;
        public Szolgaltatas()
        {
            var eleresiUt = DBcsatlakozas.Utvonal;
            _adatbazis = new SQLiteConnection(eleresiUt, DBcsatlakozas.Flags);

            _adatbazis.CreateTable<Felhasznalo>();
            _adatbazis.CreateTable<Csoport>();
            _adatbazis.CreateTable<Feladat>();
            _adatbazis.CreateTable<Tag>(); //elsődleges kulcsokra panaszkodik itt.
            _adatbazis.CreateTable<Felelos>(); //Itt is.
        }
        public string StatusMsg { get; set; } = "";
        public List<T> GetTableData<T>() where T : new()
        {
            StatusMsg = typeof(T).Name.ToLower();
            return _adatbazis.Table<T>().ToList();
        }
        public int AddTableData<T>(T item)
        {
            return _adatbazis.Insert(item);
        }
        public int UpdateTableData<T>(T item)
        {
            return _adatbazis.Update(item);
        }
        public int DeleteTableData<T>(int id)
        {
            return _adatbazis.Delete<T>(id);
        }
    }
}
