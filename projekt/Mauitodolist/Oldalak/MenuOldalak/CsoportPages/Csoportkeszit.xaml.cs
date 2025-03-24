using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportkeszit : ContentPage
{
    private int FHO_id;
    public Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO();
    //public Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();

    public Csoportkeszit(int id)
	{
		InitializeComponent();
        var felhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(x=>x.Id == id);
        if (felhasznalo != null)
        {
            // Ha l�tezik a felhaszn�l�, be�ll�tjuk az Aktfelhasznalo-t
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
        // Az Entry mez� �rt�ke
        var formazottNev = csoportentry.Text.Replace(" ", "");
        var csoportNev = formazottNev;

        // Ellen�rizd, hogy van-e sz�veg
        if (string.IsNullOrEmpty(csoportNev))
        {
            // Ha nincs sz�veg, jelezd a felhaszn�l�nak
            await DisplayAlert("Hiba", "A csoport neve nem lehet �res.", "OK");
            return;
        }

        // �j Csoport objektum l�trehoz�sa
        var ujCsoport = new Csoport
        {
            Csoportnev = csoportNev,
            Csoportkeszito = viewmodelFHO.Aktfelhasznalo.Fnev, // Ha sz�ks�ges, itt �ll�tsd be a felhaszn�l�t
            Letszam = 1 // Kezdeti l�tsz�m, ezt k�s�bb m�dos�thatod
        };
        var ujTag = new Tag
        {
            FHO_id = FHO_id,
            CSPT_nev = ,
            Jogosultsag = true
        }

        // Aszinkron kapcsolat l�trehoz�sa �s adatb�zisba ment�s
        var connection =  DBcsatlakozas.CreateConnection();

        // Csoport t�bla l�trehoz�sa, ha nem l�tezik
        await connection.CreateTableAsync<Csoport>();

        // �j csoport ment�se
        await connection.InsertAsync(ujCsoport);

        await connection.CreateTableAsync<Tag>();

        await connection.InsertAsync(ujTag);

        // Navig�lj a Csoportok oldalra
        await Navigation.PushAsync(new Csoportok());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}