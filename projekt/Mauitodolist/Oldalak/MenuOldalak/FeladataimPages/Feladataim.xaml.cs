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
        var connection = DBcsatlakozas.CreateConnection();

        // Lek�rj�k a felhaszn�l� csoporttags�gait
        var csoporttagsagok = await connection.Table<Tag>()
            .Where(ct => ct.FHO_id == FHO_id)
            .ToListAsync();

        var csoportnevek = csoporttagsagok.Select(ct => ct.Csoportnev).ToList();

        // Lek�rj�k azokat a feladatokat, amiket � hozott l�tre vagy a csoportjainak lettek kiosztva
        var feladatok = await connection.Table<Feladat>().ToListAsync();

        var sajatFeladatok = feladatok
            .Where(f => f.FHO_id == FHO_id || csoportnevek.Contains(f.CSPT_nev))
            .ToList();

        // ListView vagy CollectionView-hoz adhatod hozz�
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
            feladat.Allapot = !feladat.Allapot; // �llapot v�lt�s (pl. k�szre jel�l�s)
            await connection.UpdateAsync(feladat);

            // Friss�tj�k a ListView-t
            await BetoltesFeladatok();
        }
        else
        {
            await DisplayAlert("Hiba", "Nem m�dos�thatja ezt a feladatot!", "OK");
        }
    }

    private async void KeszFeladat_Clicked(object sender, EventArgs e)
    {
        var gomb = sender as Button;
        var feladat = gomb?.BindingContext as Feladat;

        if (feladat != null)
        {
            // Tiltsuk le a gombot a t�bbsz�ri kattint�s elker�l�se �rdek�ben
            gomb.IsEnabled = false;

            try
            {
                var connection = DBcsatlakozas.CreateConnection();
                var csoporttagsagok = await connection.Table<Tag>()
                    .Where(t => t.FHO_id == FHO_id && t.Csoportnev == feladat.CSPT_nev)
                    .ToListAsync();

                if (csoporttagsagok.Any() || feladat.FHO_id == FHO_id) // Ha tag vagy a l�trehoz�
                {
                    feladat.Allapot = true; // Feladat �llapot�nak be�ll�t�sa "k�sz"-re
                    await connection.UpdateAsync(feladat);
                    await BetoltesFeladatok(); // Friss�tj�k a list�t
                }
                else
                {
                    await DisplayAlert("Hiba", "Nem m�dos�thatja ezt a feladatot!", "OK");
                }
            }
            finally
            {
                // A lista friss�t�se ut�n �jra enged�lyezhetj�k a gombot
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
            // Tiltsuk le a gombot a t�bbsz�ri kattint�s elker�l�se �rdek�ben
            gomb.IsEnabled = false;

            try
            {
                var connection = DBcsatlakozas.CreateConnection();
                var csoporttagsagok = await connection.Table<Tag>()
                    .Where(t => t.FHO_id == FHO_id && t.Csoportnev == feladat.CSPT_nev)
                    .ToListAsync();

                if (csoporttagsagok.Any() || feladat.FHO_id == feladat.FHO_id) // Ha tag vagy a l�trehoz�
                {
                    feladat.Allapot = false; // Feladat �llapot�nak be�ll�t�sa "nem k�sz"-re
                    await connection.UpdateAsync(feladat);
                    await BetoltesFeladatok(); // Friss�tj�k a list�t
                }
                else
                {
                    await DisplayAlert("Hiba", "Nem m�dos�thatja ezt a feladatot!", "OK");
                }
            }
            finally
            {
                // A lista friss�t�se ut�n �jra enged�lyezhetj�k a gombot
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
            // Ellen�rizz�k, hogy a bejelentkezett felhaszn�l� hozta-e l�tre a feladatot
            if (feladat.FHO_id == FHO_id)
            {
                bool valasz = await DisplayAlert("T�rl�s meger�s�t�se", $"Biztosan t�r�lni szeretn�d a k�vetkez� feladatot: '{feladat.Cim}'?", "Igen", "Nem");

                if (valasz)
                {
                    var connection = DBcsatlakozas.CreateConnection();
                    await connection.DeleteAsync(feladat);

                    // Friss�tj�k a ListView-t
                    await BetoltesFeladatok();
                }
                // Ha a felhaszn�l� a "Nem" gombra kattintott, semmi nem t�rt�nik
            }
            else
            {
                await DisplayAlert("Hozz�f�r�s megtagadva", "Csak a feladat l�trehoz�ja t�r�lheti azt.", "OK");
            }
        }
    }
    public async void BetoltesFeladatok()
    {
        var connection = DBcsatlakozas.CreateConnection();

        var feladatok = await connection.Table<Feladat>()
                                       .Where(f => f.FHO_id == FHO_id)
                                       .ToListAsync();

        FeladatokListView.ItemsSource = feladatok;
    }

}