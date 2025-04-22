using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladatPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladataimPages;

public partial class Feladataim : ContentPage
{
    readonly int FHO_id;
    public int BejelentkezettFelhasznaloId { get; set; }

    public Feladataim(int id)
	{
		InitializeComponent();
        FHO_id = id;

        BejelentkezettFelhasznaloId = id;
        BetoltesFeladatok();
    }

    private void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Fooldal(FHO_id));
    }

    private void feladat_keszit_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private void csoport_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private void profil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profilok(FHO_id));
    }

    public async Task BetoltesFeladatok()
    {
        var connection = DBcsatlakozas.CreateConnection();

        // Lekérjük a felhasználó csoporttagságait
        var csoporttagsagok = await connection.Table<Tag>()
            .Where(ct => ct.FHO_id == FHO_id)
            .ToListAsync();

        var csoportnevek = csoporttagsagok.Select(ct => ct.Csoportnev).ToList();

        // Lekérjük azokat a feladatokat, amiket õ hozott létre vagy a csoportjainak lettek kiosztva
        var feladatok = await connection.Table<Feladat>().ToListAsync();

        var sajatFeladatok = feladatok
            .Where(f => f.FHO_id == FHO_id || csoportnevek.Contains(f.CSPT_nev))
            .ToList();

        // Itt pl. ListView vagy CollectionView-hoz adhatod hozzá
        FeladatokListView.ItemsSource = sajatFeladatok;
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
    }



}