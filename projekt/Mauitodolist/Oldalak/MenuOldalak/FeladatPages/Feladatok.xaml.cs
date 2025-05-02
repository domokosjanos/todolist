using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladatPages;

public partial class Feladatok : ContentPage
{
    private Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();
    private List<Csoport> CSPTLista =new List<Csoport>();
    private Csoport AktCsoport;
    private Viewmodel_FAT viewmodelFAT = new Viewmodel_FAT();
    private Feladat AktFeladat;
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

        
        await connection.CreateTableAsync<Tag>();
        await connection.CreateTableAsync<Csoport>();

        var tagsagok = await connection.Table<Tag>()
            .Where(t => t.FHO_id == FHO_id)
            .ToListAsync();

        var csoportIDs = tagsagok.Select(t => t.CSPT_id).Distinct().ToList();

        var csoportok = await connection.Table<Csoport>()
            .Where(c => csoportIDs.Contains(c.Id))
            .ToListAsync();
        foreach (var csoport in csoportok)
        {
            CSPTLista.Add(csoport);
        }
        var csoportNevek = csoportok.Select(c => c.Csoportnev).ToList();

        CsoportPicker.ItemsSource = csoportNevek;
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
            await DisplayAlert("Hiba", "Minden k�telez� mez�t ki kell t�lteni �s ki kell v�lasztani egy csoportot!", "OK");
            return;
        }

        var connection = DBcsatlakozas.CreateConnection();
        await connection.CreateTableAsync<Feladat>();

        var ujFeladat = new Feladat
        {
            FHO_id = FHO_id,
            CSPT_nev = CsoportPicker.SelectedItem.ToString(),
            Cim = CimEntry.Text,
            //Hatarido = hatarDatePicker.Date.ToString(),
            Leiras = feladatleiras.Text,
            Feladat_letrejotte = DateTime.Now.ToString()
        };

        await connection.InsertAsync(ujFeladat);
        await connection.CreateTableAsync<Felelos>();

        if (CSPTLista == null)
        {
            await DisplayAlert("Hiba", "CSPTLista", "OK");
            return;
        }

        if ((ujFeladat == null))
        {
            await DisplayAlert("Hiba", "ujFeladat", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(ujFeladat.CSPT_nev))
        {
            await DisplayAlert("Hiba", "ujFeladat.CSPT_nev", "OK");
            return;
        }

        var talalat = CSPTLista.Find(x => x.Csoportnev == ujFeladat.CSPT_nev);
        if (talalat == null)
        {
            await DisplayAlert("Hiba", "A kiv�lasztott csoport nem tal�lhat� a list�ban!", "OK");
            return;
        }

        var letezik = await connection.Table<Felelos>()
    .Where(f => f.FAT_id == ujFeladat.Id && f.CSPT_id == talalat.Id)
    .FirstOrDefaultAsync();

        if (letezik == null)
        {
            var ujFelelos = new Felelos
            {
                FAT_id = ujFeladat.Id,
                CSPT_id = talalat.Id
            };

            await connection.InsertAsync(ujFelelos);
        }
        else
        {
            await DisplayAlert("Figyelem", "Ez a felel�s m�r hozz� van rendelve a feladathoz.", "OK");
        }
        var tempList = await connection.Table<Felelos>().ToListAsync();
        await DisplayAlert("Eredm�ny:", $"Hozz�adott sorok sz�ma: {tempList.Count}", "Wilco!");
        await DisplayAlert("Siker", "A feladat sikeresen l�trej�tt!", "OK");

        // Navig�ci� a Feladataim oldalra
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }
    /*
    private void hatarDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;
        DateTime today = DateTime.Now.Date;  // Mai nap d�tuma (id� n�lk�li)

        // Ellen�rizz�k, hogy a d�tum j�v�beli, mint a mai nap �s nem t�lt�tte be a 6 �ves kort.
        if (selectedDate < today)
        {
            // Ha nem megfelel� d�tumot v�lasztottak, akkor �ll�tsuk vissza egy �rv�nyes d�tumra
            DisplayAlert("Hiba", "A kiv�lasztott d�tum nem lehet kor�bbi, mint a mai nap d�tuma. V�lasszon egy k�s�bbi d�tumot!", "Rendben");
        }
    }
    */
}