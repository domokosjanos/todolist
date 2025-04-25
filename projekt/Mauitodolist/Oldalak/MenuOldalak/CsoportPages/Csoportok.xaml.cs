using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportok : ContentPage
{
    private SQLiteAsyncConnection _connection;
    private Felhasznalo AktFelhasznalo;
    private int FHO_id;
    private Viewmodel_FHO viewmodelFHO; // Hozzáadtuk a Viewmodel_FHO példányt
    public Csoportok(int id)
    {
        InitializeComponent();
        FHO_id = id;
        _connection = DBcsatlakozas.CreateConnection(); // Adatbáziskapcsolat inicializálása
        viewmodelFHO = new Viewmodel_FHO(); // Inicializáljuk a Viewmodel_FHO-t

        // Lekérjük az aktuális felhasználót
        AktFelhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(f => f.Id == FHO_id);

        // Csoportok betöltése, stb.
        BetoltCsoportok();
    }

    public Csoportok()
    {
        InitializeComponent();
        _connection = DBcsatlakozas.CreateConnection(); // Adatbáziskapcsolat inicializálása

        // ViewModel példányosítása
        Viewmodel_CSPT viewModel = new Viewmodel_CSPT();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await BetoltCsoportok();
    }

    private async Task BetoltCsoportok()
    {
        try
        {
            // Ellenõrizzük, hogy létezik-e a tábla, ha nem, hozzuk létre
            await _connection.CreateTableAsync<Csoport>();

            // Csoportok lekérése
            var csoportLista = await _connection.Table<Csoport>().ToListAsync();

            // Minden csoporthoz hozzárendeljük, hogy törölhetõ-e
            foreach (var csoport in csoportLista)
            {
                // Javítva: az AktFelhasznalo mezõt használjuk
                csoport.IsTorolheto = csoport.Csoportkeszito == AktFelhasznalo?.Fnev;
            }

            // CollectionView frissítése
            collectionCsoportok.ItemsSource = csoportLista;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem sikerült betölteni a csoportokat: {ex.Message}", "OK");
        }
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

    private async void feladataim_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private async void csoportkeszit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Csoportkeszit(FHO_id));
    }

    private async void profil_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Profilok(FHO_id));
    }


    private async void Torles_Clicked(object sender, EventArgs e)
    {
        // Megnézzük, hogy melyik gomb lett megnyomva
        if (sender is Button button && button.BindingContext is Csoport torlendoCsoport)
        {
            // Jogosultság ellenõrzés
            // Javítva: az AktFelhasznalo mezõt használjuk és null-biztos elérést alkalmazunk
            if (AktFelhasznalo?.Fnev != torlendoCsoport.Csoportkeszito)
            {
                //ha valahogy mégsem lenne ínaktív a gomb 
                await DisplayAlert("Hozzáférés megtagadva", "Csak a csoport készítõje törölheti a csoportot.", "OK");
                return;
            }

            bool valasz = await DisplayAlert("Törlés", "Biztosan törölni szeretnéd?", "Igen", "Nem");

            if (valasz)
            {
                try
                {
                    await _connection.DeleteAsync(torlendoCsoport);
                    await BetoltCsoportok();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hiba", $"Hiba történt: {ex.Message}", "OK");
                }
            }
        }
    }

}
