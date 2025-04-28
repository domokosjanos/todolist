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
    [System.ComponentModel.DataAnnotations.Schema.Table("Tagsag")]
    public class Tag
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int FHO_id { get; set; }
        [NotNull]
        public int CSPT_id { get; set; }
        [NotNull]
        public Boolean Jogosultsag { get; set; }
        [NotNull]
        public string Csoportnev { get; set; } = "";
    }
}
