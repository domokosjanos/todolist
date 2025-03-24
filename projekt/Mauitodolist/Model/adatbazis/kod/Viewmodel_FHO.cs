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
    public class Viewmodel_FHO
    {
        public List<Felhasznalo> Felhasznalok { get; set; } = new List<Felhasznalo>();
        private Szolgaltatas Szolgaltatas = new Szolgaltatas();
        public Felhasznalo Aktfelhasznalo { get; set; } = new Felhasznalo();
        public bool Modositas { get; set; } = false;

        public Viewmodel_FHO()
        {
            // Felhasználók betöltése az adatbázisból
            Felhasznalok = new List<Felhasznalo>(Szolgaltatas.GetTableData<Felhasznalo>().ToList());
        }
    }
    /*
    public void Lekeres_FHO(Felhasznalo felhasz)
    {
        Szolgaltatas.AddTableData<Felhasznalo>(felhasz);
        Felhasznalok.Add(felhasz);
    }

    public void Modosit_FHO(Felhasznalo felhasz)
    {
        try
        {
            Szolgaltatas.UpdateTableData<Felhasznalo>(felhasz);
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Hiba", ex.Message, "Ok");
        }
    }
    */
}

