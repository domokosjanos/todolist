using PropertyChanged;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiToDoList.Model.adatbazis.tablak
{
    [AddINotifyPropertyChangedInterface]
    [System.ComponentModel.DataAnnotations.Schema.Table("feladatok")]
    public class Feladat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Leiras { get; set; }
        [NotNull]
        public string Cim { get; set; }
        public string? Hatarido { get; set; }
        [NotNull]
        public Boolean Allapot { get; set; }
        [NotNull]
        public string Feladat_letrejotte { get; set; }
        [NotNull]
        public int FHO_id { get; set; }
    }
}
