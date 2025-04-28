using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using SQLite;

namespace MauiToDoList.Oldalak.MenuPages.CsoportPages;

public partial class Csoportkeszit : ContentPage
{
    private int FHO_id;
    public Viewmodel_FHO viewmodelFHO = new Viewmodel_FHO();
    //public Viewmodel_CSPT viewmodelCSPT = new Viewmodel_CSPT();
    private List<Felhasznalo> felhasznalok = new List<Felhasznalo>();


    public Csoportkeszit(int id)
    {
        InitializeComponent();
        var felhasznalo = viewmodelFHO.Felhasznalok.FirstOrDefault(x => x.Id == id);
        if (felhasznalo != null)
        {
            // Ha létezik a felhasználó, beállítjuk az Aktfelhasznalo-t
            viewmodelFHO.Aktfelhasznalo = felhasznalo;
        }
        FHO_id = id;

        BetoltesFelhasznalok();

    }

    private async void BetoltesFelhasznalok()
    {
        var connection = DBcsatlakozas.CreateConnection();
        var felhasznalokLista = await connection.Table<Felhasznalo>().ToListAsync();

        // Szûrjük a listát, hogy ne tartalmazza az aktuálisan bejelentkezett felhasználót
        var szurtFelhasznalokLista = felhasznalokLista.Where(f => f.Id != FHO_id).ToList();

        // Az IsSelected property beállítása a szûrt listában
        szurtFelhasznalokLista.ForEach(f => f.IsSelected = false); // Alapértelmezett állapot: nem kiválasztott

        felhasznalok = szurtFelhasznalokLista; // Felhasználók szûrt lista mentése
        FelhasznaloCollectionView.ItemsSource = szurtFelhasznalokLista; // A CollectionView frissítése a szûrt listával
    }

    private void csoportentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        button_Letrehoz.IsEnabled = !string.IsNullOrWhiteSpace(csoportentry.Text);
    }

    private async void button_Letrehoz_Clicked_1(object sender, EventArgs e)
    {
        // Az Entry mezõ értéke
        var formazottNev = csoportentry.Text.Replace(" ", "");
        var csoportNev = formazottNev;

        // Ellenõrizd, hogy van-e szöveg
        if (string.IsNullOrEmpty(csoportNev))
        {
            await DisplayAlert("Hiba", "A csoport neve nem lehet üres.", "OK");
            return;
        }

        var ujCsoport = new Csoport
        {
            Csoportnev = csoportNev,
            Csoportkeszito = viewmodelFHO.Aktfelhasznalo.Fnev,
            Letszam = 0 // Kezdeti létszám a készítõvel
        };

        var connection = DBcsatlakozas.CreateConnection();
        await connection.CreateTableAsync<Csoport>();
        await connection.InsertAsync(ujCsoport);

        var csoport = await connection.Table<Csoport>().FirstOrDefaultAsync(c => c.Csoportnev == csoportNev);

        if (csoport == null)
        {
            await DisplayAlert("Hiba", "A csoportot nem sikerült létrehozni.", "OK");
            return;
        }

        await connection.CreateTableAsync<Tag>();

        int letszam = 0; // Kezdjük a létszámot a csoport készítõjével (1)

        // A csoport készítõjének hozzáadása a Tag táblához
        var keszitoTag = new Tag
        {
            FHO_id = FHO_id,
            CSPT_id = csoport.Id,
            Jogosultsag = true
        };
        await connection.InsertAsync(keszitoTag);
        letszam++; // Növeljük a létszámot a készítõvel

        // A kiválasztott felhasználók beillesztése a Tag táblába
        var kijeloltFelhasznalok = felhasznalok.Where(f => f.IsSelected).ToList();
        letszam += kijeloltFelhasznalok.Count; // A kiválasztott felhasználók számának hozzáadása

        foreach (var felhasznalo in kijeloltFelhasznalok)
        {
            var ujTag = new Tag
            {
                FHO_id = felhasznalo.Id,
                CSPT_id = csoport.Id,
                Jogosultsag = false // Az alapértelmezett jogosultság
            };
            await connection.InsertAsync(ujTag);
        }

        // Frissítjük a csoport létszámát
        csoport.Letszam = letszam;
        await connection.UpdateAsync(csoport); // A csoport létszámának frissítése

        await DisplayAlert("Siker", "A csoport létrehozása sikeres volt.", "OK");
        await Navigation.PushAsync(new Csoportok(FHO_id));

    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}