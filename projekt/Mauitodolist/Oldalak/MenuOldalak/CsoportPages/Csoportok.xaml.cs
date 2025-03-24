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
        _connection = DBcsatlakozas.CreateConnection(); // Adatb�ziskapcsolat inicializ�l�sa
    }

    public Csoportok()
    {
        InitializeComponent();
        _connection = DBcsatlakozas.CreateConnection(); // Adatb�ziskapcsolat inicializ�l�sa

        // ViewModel p�ld�nyos�t�sa
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
            // Ellen�rizz�k, hogy l�tezik-e a t�bla, ha nem, hozzuk l�tre
            await _connection.CreateTableAsync<Csoport>();

            // Csoportok lek�r�se az adatb�zisb�l
            var csoportLista = await _connection.Table<Csoport>().ToListAsync();

            // CollectionView friss�t�se
            collectionCsoportok.ItemsSource = csoportLista;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem siker�lt bet�lteni a csoportokat: {ex.Message}", "OK");
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
        // Megn�zz�k, hogy melyik gomb lett megnyomva
        if (sender is Button button && button.BindingContext is Csoport torlendoCsoport)
        {
            bool valasz = await DisplayAlert("T�rl�s", "Biztosan t�r�lni szeretn�d?", "Igen", "Nem");

            if (valasz)
            {
                try
                {
                    // Csoport t�rl�se az adatb�zisb�l
                    await _connection.DeleteAsync(torlendoCsoport);

                    // Friss�tj�k a csoportok list�j�t
                    await BetoltCsoportok();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hiba", $"Hiba t�rt�nt a t�rl�s sor�n: {ex.Message}", "OK");
                }
            }
        }
    }

}
