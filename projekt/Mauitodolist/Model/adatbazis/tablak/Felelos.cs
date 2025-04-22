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
    [System.ComponentModel.DataAnnotations.Schema.Table("Felelosok")]
    public class Felelos
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public int CSPT_id { get; set; }
        [NotNull]
        public int FAT_id { get; set; }
        [NotNull]
        public string FeladatNev { get; set; } = ""; // 👈 alapértelmezett érték
    }
}
