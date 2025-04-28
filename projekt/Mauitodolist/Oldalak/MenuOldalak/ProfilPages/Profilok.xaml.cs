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

        // Létrehozod a ViewModel példányt
        Viewmodel_FHO viewModel = new Viewmodel_FHO();

        // Kiválasztod az adott felhasználót a listából
        var felhasznalo = viewModel.Felhasznalok.FirstOrDefault(x => x.Id == id);

        if (felhasznalo != null)
        {
            // Ha létezik a felhasználó, beállítjuk az Aktfelhasznalo-t
            viewModel.Aktfelhasznalo = felhasznalo;
        }

        // Beállítod a BindingContext-et a ViewModel-re
        BindingContext = viewModel;
        FHO_id = id;
    }


    private async void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Fooldal(FHO_id));
    }

    

    private async void csoport_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private async void feladat_keszit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private async void Ujfeladat_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private async void kijelentkezes_Clicked(object sender, EventArgs e)
    {

        bool valasz = await DisplayAlert("Kijelentkezés", "Biztosan ki szeretnél jelentkezni?", "igen", "Nem");
        if (valasz == true)
        {
            // Távolítsuk el az összes elõzõ oldalt a navigációs verembõl, így nem tudunk visszalépni
            await Navigation.PopToRootAsync();
            // Navigálj a Bejelentkezés oldalra
            await Navigation.PushAsync(new Bejelentkezes());
        }
        else 
        {
           
        }
       

        
    }
}