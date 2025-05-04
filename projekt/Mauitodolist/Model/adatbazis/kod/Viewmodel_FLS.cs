using MauiToDoList.Model.adatbazis.tablak;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiToDoList.Model.adatbazis.kod
{
    [AddINotifyPropertyChangedInterface]
    public class Viewmodel_FLS
    {
        public List<Felelos> Felelosok { get; set; } = new List<Felelos>();
        private Szolgaltatas Szolgaltatas = new Szolgaltatas();
        public Felelos Aktfelelos { get; set; } = new Felelos();

        public Viewmodel_FLS()
        {
            // Felelösök betöltése az adatbázisból
            Felelosok = new List<Felelos>(Szolgaltatas.GetTableData<Felelos>().ToList());
        }

    }
}
