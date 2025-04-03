using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportkeszit : ContentPage
{
    private int FHO_id;
    public Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO();
    public Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();

    public Csoportkeszit(int id)
	{
		InitializeComponent();
        var felhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(x=>x.Id == id);
        if (felhasznalo != null)
        {
            // Ha létezik a felhasználó, beállítjuk az Aktfelhasznalo-t
            viewmodelFHO.Aktfelhasznalo = felhasznalo;
        }
        FHO_id = id;

	}

    private void csoportentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        button_Letrehoz.IsEnabled = !string.IsNullOrWhiteSpace(csoportentry.Text);
    }

    private async void button_Letrehoz_Clicked_1(object sender, EventArgs e)
    {
        
        // Az Entry mezõ értéke
        NevVisszaJelez.IsVisible = false;
        var formazottNev = "";
        if (csoportentry.Text.Any(char.IsWhiteSpace) == false)
        {
            formazottNev = csoportentry.Text.Replace(" ", "_");
            var csoportNev = formazottNev;

            // Ellenõrizd, hogy van-e szöveg
            if (string.IsNullOrEmpty(csoportNev))
            {
                // Ha nincs szöveg, jelezd a felhasználónak
                await DisplayAlert("Visszajelzés", "A csoport neve nem lehet üres.", "OK");
                return;
            }

            // Új Csoport objektum létrehozása
            var ujCsoport = new Csoport
            {
                Csoportnev = csoportNev,
                Csoportkeszito = viewmodelFHO.Aktfelhasznalo.Fnev, // Ha szükséges, itt állítsd be a felhasználót
                Letszam = 1 // Kezdeti létszám, ezt késõbb módosíthatod
            };
            await DisplayAlert("Infó", csoportNev, "OK");

            // Aszinkron kapcsolat létrehozása és adatbázisba mentés
            var connection = DBcsatlakozas.CreateConnection();

            // Csoport tábla létrehozása, ha nem létezik
            await connection.CreateTableAsync<Csoport>();

            // Új csoport mentése
            await connection.InsertAsync(ujCsoport);
            viewmodelCSPT.Aktcsoport = await connection.Table<Csoport>().Where(x => x.Csoportnev == csoportNev).FirstOrDefaultAsync();
            
            await DisplayAlert("Infó2", viewmodelCSPT.Aktcsoport.Csoportnev, "OK");
            
            if(viewmodelCSPT.Aktcsoport == null)
            {
                await DisplayAlert("Hiba!", "Null érték", "A kenyérbe ezzel.");
                return;
            }
            var ujTag = new Tag
            {
                FHO_id = FHO_id,
                CSPT_id = viewmodelCSPT.Aktcsoport.Id,
                Jogosultsag = true
            };

            await connection.CreateTableAsync<Tag>();

            await connection.InsertAsync(ujTag);

            // Navigálj a Csoportok oldalra
            await Navigation.PushAsync(new Csoportok());

            
        }
        else
        {
            NevVisszaJelez.IsVisible = true;
            return;
        }
        
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}