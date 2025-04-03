using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladatPages;

public partial class Feladatok : ContentPage
{
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

        // Lek�rj�k az �sszes csoportot az adatb�zisb�l
        var csoportok = await connection.Table<Csoport>().ToListAsync();

        // List�ba mentj�k a csoportok neveit
        var csoportNevek = csoportok.Select(c => c.Csoportnev).ToList();

        // A csoportneveket hozz�rendelj�k a Picker ItemsSource-j�hoz
        CsoportPicker.ItemsSource = csoportNevek;
    }

    private void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Fooldal(FHO_id));
    }

    private void Ujfeladat_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private void csoport_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private void profil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profilok(FHO_id));
    }

    private async void Keszit_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CimEntry.Text) || string.IsNullOrWhiteSpace(feladatleiras.Text) || CsoportPicker.SelectedItem == null)
        {
            await DisplayAlert("Hiba", "Minden mez?t ki kell t�lteni �s ki kell v�lasztani egy csoportot!", "OK");
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

        await DisplayAlert("Siker", "A feladat sikeresen l�trej�tt!", "OK");

        // Navig�ci� a Feladataim oldalra
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }
}