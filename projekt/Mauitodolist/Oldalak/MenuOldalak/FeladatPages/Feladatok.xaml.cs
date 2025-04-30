using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladatPages;

public partial class Feladatok : ContentPage
{
    private Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();
    private Csoport AktCsoport;
    readonly int FHO_id;
	public Feladatok(int id)
	{
		InitializeComponent();
        FHO_id = id;

        BetoltesCsoportok();
    }


    private async void BetoltesCsoportok()
    {
        var connection = DBcsatlakozas.CreateConnection();

        // Ellenõrizzük, hogy létezik-e a Tag tábla
        await connection.CreateTableAsync<Tag>();

        // Lekérjük az aktuális felhasználó tagságait
        var tagsagok = await connection.Table<Tag>()
            .Where(t => t.FHO_id == FHO_id) // Feltételezzük, hogy FHO_id az aktuális felhasználó azonosítója
            .ToListAsync();

        if (!tagsagok.Any())
        {
            // Ha a felhasználó nem tag egyetlen csoportban sem, üres listát állítunk be
            CsoportPicker.ItemsSource = new List<string>();
            return;
        }

        // Lekérjük az összes csoportot
        var osszesCsoport = await connection.Table<Csoport>().ToListAsync();

        // Létrehozunk egy listát a megjelenítendõ csoportnevekhez
        var megjelenoCsoportNevek = new List<string>();

        // Iterálunk az összes csoporton
        foreach (var csoport in osszesCsoport)
        {
            // Ellenõrizzük, hogy a csoport azonosítója szerepel-e a felhasználó tagságai között
            if (tagsagok.Any(tag => tag.CSPT_id == csoport.Id))
            {
                megjelenoCsoportNevek.Add(csoport.Csoportnev);
            }
        }

        // A megjelenítendõ csoportneveket hozzárendeljük a Picker ItemsSource-jához
        CsoportPicker.ItemsSource = megjelenoCsoportNevek;
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

    private async void Keszit_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CimEntry.Text) || string.IsNullOrWhiteSpace(feladatleiras.Text) || CsoportPicker.SelectedItem == null)
        {
            await DisplayAlert("Hiba", "Minden mezõt ki kell tölteni és ki kell választani egy csoportot!", "OK");
            return;
        }

        var connection = DBcsatlakozas.CreateConnection();
        await connection.CreateTableAsync<Feladat>();

        var ujFeladat = new Feladat
        {
            FHO_id = FHO_id,
            CSPT_nev = CsoportPicker.SelectedItem.ToString(),
            Cim = CimEntry.Text, 
            Leiras = feladatleiras.Text,
            Feladat_letrejotte = DateTime.Now.ToString()
        };

        await connection.InsertAsync(ujFeladat);






        await DisplayAlert("Siker", "A feladat sikeresen létrejött!", "OK");

        // Navigáció a Feladataim oldalra
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }
}