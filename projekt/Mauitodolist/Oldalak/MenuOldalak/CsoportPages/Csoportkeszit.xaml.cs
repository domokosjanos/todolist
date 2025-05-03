using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportkeszit : ContentPage
{
    private int FHO_id;
    public Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO();
    private List<Felhasznalo> felhasznalok = new List<Felhasznalo>();
    

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

        BetoltesFelhasznalok();

	}

    private async void BetoltesFelhasznalok()
    {
        var connection = DBcsatlakozas.CreateConnection();
        var felhasznalokLista = await connection.Table<Felhasznalo>().ToListAsync();

        // Sz�rj�k a list�t, hogy ne tartalmazza az aktu�lisan bejelentkezett felhaszn�l�t
        var szurtFelhasznalokLista = felhasznalokLista.Where(f => f.Id != FHO_id).ToList();

        // Az IsSelected property be�ll�t�sa a sz�rt list�ban
        szurtFelhasznalokLista.ForEach(f => f.IsSelected = false); // Alap�rtelmezett �llapot: nem kiv�lasztott

        felhasznalok = szurtFelhasznalokLista; // Felhaszn�l�k sz�rt lista ment�se
        FelhasznaloCollectionView.ItemsSource = szurtFelhasznalokLista; // A CollectionView friss�t�se a sz�rt list�val
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
            await DisplayAlert("Hiba", "A csoport neve nem lehet �res.", "OK");
            return;
        }

        var ujCsoport = new Csoport
        {
            Csoportnev = csoportNev,
            Csoportkeszito = viewmodelFHO.Aktfelhasznalo.Fnev,
            Letszam = 0 // Kezdeti l�tsz�m a k�sz�t�vel
        };

        var connection = DBcsatlakozas.CreateConnection();
        var path = DBcsatlakozas.CreateConnection().DatabasePath;
        //await DisplayAlert("DB �tvonal", path, "OK");

        await connection.CreateTableAsync<Csoport>();
        await connection.InsertAsync(ujCsoport);

        var csoport = await connection.Table<Csoport>().FirstOrDefaultAsync(c => c.Csoportnev == csoportNev);

        if (csoport == null)
        {
            await DisplayAlert("Hiba", "A csoportot nem siker�lt l�trehozni.", "OK");
            return;
        }

        await connection.CreateTableAsync<Tag>();

        int letszam = 0; // Kezdj�k a l�tsz�mot a csoport k�sz�t�j�vel (1)

        // A csoport k�sz�t�j�nek hozz�ad�sa a Tag t�bl�hoz
        var keszitoTag = new Tag
        {
            FHO_id = FHO_id,
            CSPT_id = csoport.Id,
            Jogosultsag = true
        };
        await connection.InsertAsync(keszitoTag);
        letszam++; // N�velj�k a l�tsz�mot a k�sz�t�vel

        // A kiv�lasztott felhaszn�l�k beilleszt�se a Tag t�bl�ba
        var kijeloltFelhasznalok = felhasznalok.Where(f => f.IsSelected).ToList();
        letszam += kijeloltFelhasznalok.Count; // A kiv�lasztott felhaszn�l�k sz�m�nak hozz�ad�sa

        foreach (var felhasznalo in kijeloltFelhasznalok)
        {
            var ujTag = new Tag
            {
                FHO_id = felhasznalo.Id,
                CSPT_id = csoport.Id,
                Jogosultsag = false // Az alap�rtelmezett jogosults�g
            };
            await connection.InsertAsync(ujTag);
        }
        var mindenTag = await connection.Table<Tag>().ToListAsync();
        //await DisplayAlert("Debug", $"Besz�rt tag rekordok sz�ma: {mindenTag.Count}", "OK");
        // Friss�tj�k a csoport l�tsz�m�t
        csoport.Letszam = letszam;
        await connection.UpdateAsync(csoport); // A csoport l�tsz�m�nak friss�t�se

        await connection.CloseAsync();

        await DisplayAlert("Siker", "A csoport l�trehoz�sa sikeres volt.", "OK");
        await Navigation.PushAsync(new Csoportok(FHO_id));

    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}