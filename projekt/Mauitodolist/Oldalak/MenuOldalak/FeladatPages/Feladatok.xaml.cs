using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladatPages;

public partial class Feladatok : ContentPage
{
    private Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();
    private List<Csoport> CSPTLista;
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

        // Ellen�rizz�k, hogy l�tezik-e a Tag t�bla
        await connection.CreateTableAsync<Tag>();

        // Lek�rj�k az aktu�lis felhaszn�l� tags�gait
        var tagsagok = await connection.Table<Tag>()
            .Where(t => t.FHO_id == FHO_id) // Felt�telezz�k, hogy FHO_id az aktu�lis felhaszn�l� azonos�t�ja
            .ToListAsync();

        if (!tagsagok.Any())
        {
            // Ha a felhaszn�l� nem tag egyetlen csoportban sem, �res list�t �ll�tunk be
            CsoportPicker.ItemsSource = new List<string>();
            return;
        }

        // Lek�rj�k az �sszes csoportot
        var osszesCsoport = await connection.Table<Csoport>().ToListAsync();
        // L�trehozunk egy list�t a megjelen�tend� csoportnevekhez
        var megjelenoCsoportNevek = new List<string>();

        // Iter�lunk az �sszes csoporton
        foreach (var csoport in osszesCsoport)
        {
            CSPTLista.Add(csoport);
            // Ellen�rizz�k, hogy a csoport azonos�t�ja szerepel-e a felhaszn�l� tags�gai k�z�tt
            if (tagsagok.Any(tag => tag.CSPT_id == csoport.Id))
            {
                megjelenoCsoportNevek.Add(csoport.Csoportnev);
            }
        }

        // A megjelen�tend� csoportneveket hozz�rendelj�k a Picker ItemsSource-j�hoz
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
            Hatarido = hatarDatePicker.Date.ToString(),
            Leiras = feladatleiras.Text,
            Feladat_letrejotte = DateTime.Now.ToString()
        };

        await connection.InsertAsync(ujFeladat);
        await connection.CreateTableAsync<Felelos>();

        var ujFelelos = new Felelos
        {
            FAT_id = ujFeladat.Id,
            CSPT_id = CSPTLista.Find(x => x.Csoportnev == ujFeladat.CSPT_nev).Id
        };

        await connection.InsertAsync(ujFelelos);

        await DisplayAlert("Siker", "A feladat sikeresen l�trej�tt!", "OK");

        // Navig�ci� a Feladataim oldalra
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }

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
}