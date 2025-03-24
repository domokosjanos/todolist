using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using SQLite;

namespace MauiToDoList.Model.adatbazis.tablak
{
    [AddINotifyPropertyChangedInterface]
    [System.ComponentModel.DataAnnotations.Schema.Table("csoportok")]
    public class Csoport
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Csoportnev { get; set; }
        [NotNull]
        public string Csoportkeszito { get; set; }
        [NotNull]
        public int Letszam { get; set; } //törlésnél játszik szerepet
    }
}
