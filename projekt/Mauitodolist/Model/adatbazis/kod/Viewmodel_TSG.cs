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
    class Viewmodel_TSG
    {
        public List<Tag> Tagok { get; set; } = new List<Tag>();
        private Szolgaltatas Szolgaltatas = new Szolgaltatas();
        public Tag AktTag { get; set; } = new Tag();

        public Viewmodel_TSG()
        {
            Tagok = new List<Tag>(Szolgaltatas.GetTableData<Tag>().ToList());
        }

    }
}
