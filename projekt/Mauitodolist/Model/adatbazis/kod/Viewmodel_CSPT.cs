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
    class Viewmodel_CSPT
    {
        public List<Csoport> Csoportok { get; set; } = new List<Csoport>();
        private Szolgaltatas Szolgaltatas = new Szolgaltatas();
        public Csoport Aktcsoport { get; set; } = new Csoport();

        public Viewmodel_CSPT()
        {
            Csoportok = new List<Csoport>(Szolgaltatas.GetTableData<Csoport>().ToList());
        }

    }
}
