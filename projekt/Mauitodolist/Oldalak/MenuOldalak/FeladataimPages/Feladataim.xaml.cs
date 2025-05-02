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
        /*   
           var connection = DBcsatlakozas.CreateConnection();

           // Lekérjük a felhasználó csoporttagságait


           // Lekérjük azokat a feladatokat, amiket õ hozott létre vagy a csoportjainak lettek kiosztva
           var feladatok = await connection.Table<Feladat>().ToListAsync();

           var sajatFeladatok = feladatok
               .Where(f => (f.FHO_id == FHO_id ) || csoportnevek.Contains(f.CSPT_nev))
               .ToList();

           // ListView vagy CollectionView-hoz adhatod hozzá
           FeladatokListView.ItemsSource = sajatFeladatok;
          */
        /*
          _connection = DBcsatlakozas.CreateConnection();
          //Tagságok lekérése
          var tagsagok = await _connection.Table<Tag>()
              .Where(ct => ct.FHO_id == FHO_id)
              .ToListAsync();

          //Felelõs tábla lekérdezés
          await _connection.CreateTableAsync<Felelos>();
          var Felelosok = await _connection.Table<Felelos>().ToListAsync();
          //Ellenõrzés, hogy nem üres.
          if (Felelosok == null || !Felelosok.Any())
          {
              await DisplayAlert("Hiba", "Üres tábla: Felelosok", "OK");
          }
          //Csoportok lekérdezése
          await _connection.CreateTableAsync<Csoport>();
          var Csoportok = await _connection.Table<Csoport>().ToListAsync();

          //Feladatok lekérdezése
          await _connection.CreateTableAsync<Feladat>();
          var Feladatok = await _connection.Table<Feladat>().ToListAsync();

          List<Feladat> CSPTesFHOFeladatok = new List<Feladat>();
          foreach (var csoport in Csoportok)
          {
              Aktcsoport = ViewmodelCSPT.Csoportok.FirstOrDefault(f => f.Id == csoport.Id);
              foreach (var felad in Feladatok)
              {

                  foreach (var felel in Felelosok)
                  {
                      if ((felad.Id == felel.FAT_id && Aktcsoport.Id == felel.CSPT_id)) //VAGY ág kellene
                      {
                          CSPTesFHOFeladatok.Add(felad);
                      }
                  }
              }
          }
          //CSPTesFHOFeladatok.AddRange(sajatFeladatok);
          FeladatokListView.ItemsSource = CSPTesFHOFeladatok;
          await _connection.CloseAsync();
        */

        _connection = DBcsatlakozas.CreateConnection();

        await _connection.CreateTableAsync<Feladat>();
        var feladatok = await _connection.Table<Feladat>().ToListAsync();

        FeladatokListView.ItemsSource = feladatok;

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