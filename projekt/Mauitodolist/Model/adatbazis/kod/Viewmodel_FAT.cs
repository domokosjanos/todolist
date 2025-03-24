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
    class Viewmodel_FAT
    {
        public List<Feladat> Feladatok { get; set; } = new List<Feladat>();
        private Szolgaltatas Szolgaltatas = new Szolgaltatas();
        public Feladat Aktfeladat { get; set; } = new Feladat();

        public Viewmodel_FAT()
        {
            Feladatok = new List<Feladat>(Szolgaltatas.GetTableData<Feladat>().ToList());
        }

    }
}
