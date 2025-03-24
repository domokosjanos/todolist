using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.FeladatPages;

namespace MauiToDoList.Oldalak.MenuPages.ProfilPages;

public partial class Profilok : ContentPage
{
    private int FHO_id;
    public Profilok(int id)
    {
        InitializeComponent();

        // L�trehozod a ViewModel p�ld�nyt
        Viewmodel_FHO viewModel = new Viewmodel_FHO();

        // Kiv�lasztod az adott felhaszn�l�t a list�b�l
        var felhasznalo = viewModel.Felhasznalok.FirstOrDefault(x => x.Id == id);

        if (felhasznalo != null)
        {
            // Ha l�tezik a felhaszn�l�, be�ll�tjuk az Aktfelhasznalo-t
            viewModel.Aktfelhasznalo = felhasznalo;
        }

        // Be�ll�tod a BindingContext-et a ViewModel-re
        BindingContext = viewModel;
        FHO_id = id;
    }


    private void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Fooldal(FHO_id));
    }

    

    private void csoport_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private void feladat_keszit_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private void Ujfeladat_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private async void kijelentkezes_Clicked(object sender, EventArgs e)
    {

        bool valasz = await DisplayAlert("Kijelentkez�s", "Biztosan ki szeretn�l jelentkezni?", "igen", "Nem");
        if (valasz == true)
        {
            // T�vol�tsuk el az �sszes el�z� oldalt a navig�ci�s veremb�l, �gy nem tudunk visszal�pni
            await Navigation.PopToRootAsync();
            // Navig�lj a Bejelentkez�s oldalra
            await Navigation.PushAsync(new Bejelentkezes());
        }
        else 
        {
           
        }
       

        
    }
}