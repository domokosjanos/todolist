using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladatPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.FeladataimPages;

public partial class Feladataim : ContentPage
{
    
    private SQLiteAsyncConnection _connection;
    private Felhasznalo Aktfelhasznalo;
    private Csoport Aktcsoport;
    private Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO();
    private Viewmodel_CSPT ViewmodelCSPT = new Viewmodel_CSPT();
    private readonly int FHO_id;
    public Feladataim(int id)
	{
		InitializeComponent();
        FHO_id = id;
        Aktfelhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(f=>f.Id == id);
        BetoltesFeladatok();
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

    public async Task BetoltesFeladatok()
    {
        _connection = DBcsatlakozas.CreateConnection();

        // 1. Lekérjük a felhasználó csoporttagságait
        var tagsagok = await _connection.Table<Tag>()
            .Where(t => t.FHO_id == FHO_id)
            .ToListAsync();

        var felhasznaloCsoportIds = tagsagok.Select(t => t.CSPT_id).ToList();

        // 2. Lekérjük a Felelos rekordokat
        var felelosok = await _connection.Table<Felelos>().ToListAsync();

        // 3. Lekérjük a feladatokat
        var feladatok = await _connection.Table<Feladat>().ToListAsync();

        // 4. Kiválasztjuk azokat, amik:
        //    - saját maga hozta létre (FHO_id == FHO_id)
        //    - vagy olyan feladathoz tartozik, ahol a felelos táblában CSPT_id = olyan csoport, aminek tagja
        var relevansFeladatIds = felelosok
            .Where(f => felhasznaloCsoportIds.Contains(f.CSPT_id))
            .Select(f => f.FAT_id)
            .Distinct()
            .ToList();

        var sajatEsCsoportFeladatok = feladatok
            .Where(f => f.FHO_id == FHO_id || relevansFeladatIds.Contains(f.Id))
            .ToList();

        // 5. Megjelenítés
        FeladatokListView.ItemsSource = sajatEsCsoportFeladatok;

        await _connection.CloseAsync();
    }

    private async void FeladatAllapot_Clicked(object sender, EventArgs e)
    {
        var gomb = sender as Button;
        var feladat = gomb?.BindingContext as Feladat;

        var connection = DBcsatlakozas.CreateConnection();
        var csoporttagsagok = await connection.Table<Tag>()
            .Where(t => t.FHO_id == FHO_id && t.Csoportnev == feladat.CSPT_nev)
            .ToListAsync();

        if (csoporttagsagok.Any() && feladat.FHO_id != FHO_id)
        {
            feladat.Allapot = !feladat.Allapot; // Állapot váltás (pl. készre jelölés)
            await connection.UpdateAsync(feladat);

            // Frissítjük a ListView-t
            await BetoltesFeladatok();
        }
        else
        {
            await DisplayAlert("Hiba", "Nem módosíthatja ezt a feladatot!", "OK");
        }
    }

    private async void KeszFeladat_Clicked(object sender, EventArgs e)
    {
        var gomb = sender as Button;
        var feladat = gomb?.BindingContext as Feladat;

        if (feladat != null)
        {
            // Tiltsuk le a gombot a többszöri kattintás elkerülése érdekében
            gomb.IsEnabled = false;

            try
            {
                var connection = DBcsatlakozas.CreateConnection();
                var csoporttagsagok = await connection.Table<Tag>()
                    .Where(t => t.FHO_id == FHO_id && t.Csoportnev == feladat.CSPT_nev)
                    .ToListAsync();

                if (csoporttagsagok.Any() || feladat.FHO_id == FHO_id) // Ha tag vagy a létrehozó
                {
                    feladat.Allapot = true; // Feladat állapotának beállítása "kész"-re
                    await connection.UpdateAsync(feladat);
                    await BetoltesFeladatok(); // Frissítjük a listát
                }
                else
                {
                    await DisplayAlert("Hiba", "Nem módosíthatja ezt a feladatot!", "OK");
                }
            }
            finally
            {
                // A lista frissítése után újra engedélyezhetjük a gombot
                gomb.IsEnabled = true;
            }
        }
    }

    private async void NemKeszFeladat_Clicked(object sender, EventArgs e)
    {
        var gomb = sender as Button;
        var feladat = gomb?.BindingContext as Feladat;

        if (feladat != null)
        {
            // Tiltsuk le a gombot a többszöri kattintás elkerülése érdekében
            gomb.IsEnabled = false;

            try
            {
                var connection = DBcsatlakozas.CreateConnection();
                var csoporttagsagok = await connection.Table<Tag>()
                    .Where(t => t.FHO_id == FHO_id && t.Csoportnev == feladat.CSPT_nev)
                    .ToListAsync();

                if (csoporttagsagok.Any() || feladat.FHO_id == feladat.FHO_id) // Ha tag vagy a létrehozó
                {
                    feladat.Allapot = false; // Feladat állapotának beállítása "nem kész"-re
                    await connection.UpdateAsync(feladat);
                    await BetoltesFeladatok(); // Frissítjük a listát
                }
                else
                {
                    await DisplayAlert("Hiba", "Nem módosíthatja ezt a feladatot!", "OK");
                }
            }
            finally
            {
                // A lista frissítése után újra engedélyezhetjük a gombot
                gomb.IsEnabled = true;
            }
        }
    }



    private async void FeladatTorles_Clicked(object sender, EventArgs e)
    {
        var gomb = sender as Button;
        var feladat = gomb?.BindingContext as Feladat;

        if (feladat != null)
        {
            // Ellenõrizzük, hogy a bejelentkezett felhasználó hozta-e létre a feladatot
            if (feladat.FHO_id == FHO_id)
            {
                bool valasz = await DisplayAlert("Törlés megerõsítése", $"Biztosan törölni szeretnéd a következõ feladatot: '{feladat.Cim}'?", "Igen", "Nem");

                if (valasz)
                {
                    var connection = DBcsatlakozas.CreateConnection();
                    await connection.DeleteAsync(feladat);

                    // Frissítjük a ListView-t
                    await BetoltesFeladatok();
                }
                // Ha a felhasználó a "Nem" gombra kattintott, semmi nem történik
            }
            else
            {
                await DisplayAlert("Hozzáférés megtagadva", "Csak a feladat létrehozója törölheti azt.", "OK");
            }
        }
    }



}