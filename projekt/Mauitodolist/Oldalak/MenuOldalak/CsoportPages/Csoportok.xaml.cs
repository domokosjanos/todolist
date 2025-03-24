using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportok : ContentPage
{
    private SQLiteAsyncConnection _connection;
    private int FHO_id;

    public Csoportok(int id)
    {
        InitializeComponent();
        FHO_id = id;
        _connection = DBcsatlakozas.CreateConnection(); // Adatbáziskapcsolat inicializálása
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

            // Csoportok lekérése az adatbázisból
            var csoportLista = await _connection.Table<Csoport>().ToListAsync();

            // CollectionView frissítése
            collectionCsoportok.ItemsSource = csoportLista;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem sikerült betölteni a csoportokat: {ex.Message}", "OK");
        }
    }

    private void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Fooldal(FHO_id));
    }

    private void feladataim_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private void csoportkeszit_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportkeszit(FHO_id));
    }

    private void profil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profilok(FHO_id));
    }


    private async void Torles_Clicked(object sender, EventArgs e)
    {
        // Megnézzük, hogy melyik gomb lett megnyomva
        if (sender is Button button && button.BindingContext is Csoport torlendoCsoport)
        {
            bool valasz = await DisplayAlert("Törlés", "Biztosan törölni szeretnéd?", "Igen", "Nem");

            if (valasz)
            {
                try
                {
                    // Csoport törlése az adatbázisból
                    await _connection.DeleteAsync(torlendoCsoport);

                    // Frissítjük a csoportok listáját
                    await BetoltCsoportok();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hiba", $"Hiba történt a törlés során: {ex.Message}", "OK");
                }
            }
        }
    }

}
