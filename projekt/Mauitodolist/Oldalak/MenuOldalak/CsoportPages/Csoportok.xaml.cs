using MauiToDoList.Converters;
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
    private Csoport AktCsoport;
    private int FHO_id;
    private Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO(); // Hozz�adtuk �s Inicializ�ltuk a Viewmodel_FHO-t
    private Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();
    public Csoportok(int id)
    {
        InitializeComponent();
        FHO_id = id;
        _connection = DBcsatlakozas.CreateConnection(); // Adatb�ziskapcsolat inicializ�l�sa
        

        // Lek�rj�k az aktu�lis felhaszn�l�t
        AktFelhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(f => f.Id == FHO_id);
        // Csoportok bet�lt�se, stb.
        BetoltCsoportok();
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

            // Csoportok lek�r�se
            var Tagsagok = await _connection.Table<Tag>().Where(i => i.FHO_id == AktFelhasznalo.Id).ToListAsync();
            
            var csoportLista = await _connection.Table<Csoport>().ToListAsync();
            

            List<Csoport> megjelenoCsoportok = new List<Csoport>();
            // Minden csoporthoz hozz�rendelj�k, hogy t�r�lhet�-e
            foreach (var csoport in csoportLista)
            {
                // Jav�tva: az AktFelhasznalo mez�t haszn�ljuk
                csoport.IsTorolheto = csoport.Csoportkeszito == AktFelhasznalo?.Fnev;
                
                //Csak azok a Csoportok, ahol tag a felhaszn�l�
                foreach (var tag in Tagsagok)
                {
                    if (csoport.Id == tag.CSPT_id)
                    {
                        megjelenoCsoportok.Add(csoport);
                    }
                }
                
            }

            // CollectionView friss�t�se
            collectionCsoportok.ItemsSource = megjelenoCsoportok;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Nem siker�lt bet�lteni a csoportokat: {ex.Message}", "OK");
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
        // Megn�zz�k, hogy melyik gomb lett megnyomva
        if (sender is Button button && button.BindingContext is Csoport torlendoCsoport)
        {
            // Jogosults�g ellen�rz�s
            // Jav�tva: az AktFelhasznalo mez�t haszn�ljuk �s null-biztos el�r�st alkalmazunk
            if (AktFelhasznalo?.Fnev != torlendoCsoport.Csoportkeszito)
            {
                //ha valahogy m�gsem lenne �nakt�v a gomb 
                await DisplayAlert("Hozz�f�r�s megtagadva", "Csak a csoport k�sz�t�je t�r�lheti a csoportot.", "OK");
                return;
            }

            bool valasz = await DisplayAlert("T�rl�s", "Biztosan t�r�lni szeretn�d?", "Igen", "Nem");

            if (valasz)
            {
                try
                {
                    await _connection.DeleteAsync(torlendoCsoport);
                    await BetoltCsoportok();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Hiba", $"Hiba t�rt�nt: {ex.Message}", "OK");
                }
            }
        }
    }

}
