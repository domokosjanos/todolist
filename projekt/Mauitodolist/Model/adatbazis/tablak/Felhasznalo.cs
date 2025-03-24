using PropertyChanged;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiToDoList.Model.adatbazis.tablak
{
    [AddINotifyPropertyChangedInterface]
    [Table("felhasznalok")]
    public class Felhasznalo
    {
        [PrimaryKey, AutoIncrement]
        public int Id {  get; set; }
        [NotNull]
        public string Vnev { get; set; }
        [NotNull]
        public string Knev { get; set; }
        public string? Hnev { get; set; }
        [NotNull]
        public DateTime Szul_ido { get; set; } = DateTime.Now.AddYears(-7);
        [NotNull, Unique]
        public string Fnev { get; set; }
        [NotNull]
        public string Jelszo { get; set; }
        [NotNull, Unique]
        public string Email { get; set; }
        
        public string? Tszam { get; set; }
    }
}
