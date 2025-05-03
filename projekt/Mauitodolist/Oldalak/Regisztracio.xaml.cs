using MauiToDoList.Model.adatbazis.tablak;
using SQLite;
using System.Text;
using Scrypt;

namespace MauiToDoList.Oldalak;

public partial class Regisztracio : ContentPage
{
    ScryptEncoder encoder = new ScryptEncoder();
    private string Message = "";
    private bool Ellenorzes()
    {
        using (var connection = new SQLiteConnection(DBcsatlakozas.Utvonal, DBcsatlakozas.Flags))
        {
            var felhasznalok = connection.Table<Felhasznalo>().ToList();
            var Fnevellen = felhasznalok.Find(x => x.Fnev == felhasznaloentry.Text);
            //DisplayAlert("asd", Fnevellen, "ok");
            var Emailellen = felhasznalok.Find(x => x.Email == emailentry.Text);
            //DisplayAlert("asd", Emailellen, "ok");
            if(Fnevellen == null && Emailellen == null)
            {
                return true;
            }
            Message = (Emailellen == null) ? "a felhasználónév" : "az email";
            return false;
        }
    }
    public Regisztracio()
    {
        InitializeComponent();
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        button_Register.IsEnabled = !string.IsNullOrWhiteSpace(vezetekneventry.Text) && !string.IsNullOrWhiteSpace(keresztneventry.Text) && !string.IsNullOrWhiteSpace(felhasznaloentry.Text) && !string.IsNullOrWhiteSpace(jelszoentry.Text) && !string.IsNullOrWhiteSpace(emailentry.Text);
    }

    private void button_Register_Clicked(object sender, EventArgs e)
    {
        // Ellenõrizzük, hogy a születési dátum ki van-e választva
        DateTime selectedDate = szuletesDatePicker.Date;
        DateTime today = DateTime.Now.Date;  // Mai nap dátuma (idõ nélküli)

        // Ha a születési dátum jövöbeli, mint a mai nap van, vagy 6 évvel korábbi dátum van megadva, akkor hibaüzenet
        if (selectedDate >= today || selectedDate >= today.AddYears(-6))
        {
            DisplayAlert("Hiba", "A kiválasztott dátum nem lehet jövõbeli. Válasszon egy régebbi dátumot!", "OK");
            return; // Ha nem megfelelõ a dátum, kilépünk a regisztrációs folyamatból
        }

        // SQLite kapcsolat létrehozása
        using (var connection = new SQLiteConnection(DBcsatlakozas.Utvonal, DBcsatlakozas.Flags))
        {
            connection.CreateTable<Felhasznalo>(); //Felhasznalo táblához csatlakozás
            if (Ellenorzes())
            {
                var ujFelhasznalo = new Felhasznalo
                {
                    Vnev = vezetekneventry.Text,
                    Knev = keresztneventry.Text,
                    Hnev = harmadikNevEntry.Text,
                    Fnev = felhasznaloentry.Text,
                    Jelszo = encoder.Encode(jelszoentry.Text),
                    Email = emailentry.Text,
                    Szul_ido = szuletesDatePicker.Date,
                    Tszam = Telefon.Text
                };

                connection.Insert(ujFelhasznalo);
                DisplayAlert("Információ", "Sikeres regisztráció", "OK");

                // Ellenõrizzük, hogy tényleg bekerült-e az új felhasználó
                var felhasznalo = connection.Table<Felhasznalo>().FirstOrDefault(x => x.Fnev == ujFelhasznalo.Fnev);

                if (felhasznalo != null)
                {
                    // Kiírjuk az adatokat, hogy tényleg elmentettük õket
                    string userDetails = $"Felhasználónév: {felhasznalo.Fnev}\n" +
                                         $"Név: {felhasznalo.Vnev} {felhasznalo.Knev}\n" +
                                         $"Email: {felhasznalo.Email}\n" +
                                         $"Születési dátum: {felhasznalo.Szul_ido.ToShortDateString()}\n" +
                                         $"Telefonszám: {felhasznalo.Tszam}";
                    //DisplayAlert("Sikeres regisztráció", $"A következõ adatokat mentettük el:\n\n{userDetails}", "OK");
                }

                // Ha minden rendben van, átirányítjuk a felhasználót a Bejelentkezés oldalra
                Navigation.PushAsync(new Bejelentkezes());
            }
            else
            {
                DisplayAlert("Hiba", $"Ez {Message} már foglalt. Adj meg egy másikat!", "Ok");
            }
        }
    }


    private void Telefon_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry != null)
        {
            // Csak számok maradjanak
            entry.Text = new string(entry.Text.Where(c => char.IsDigit(c)).Take(15).ToArray());
        }
    }

    private void szuletesDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;
        DateTime today = DateTime.Now.Date;  // Mai nap dátuma (idõ nélküli)

        // Ellenõrizzük, hogy a dátum jövõbeli, mint a mai nap és nem töltötte be a 6 éves kort.
        if (selectedDate >= today || selectedDate >= today.AddYears(-6))
        {
            // Ha nem megfelelõ dátumot választottak, akkor állítsuk vissza egy érvényes dátumra
            szuletesDatePicker.Date = today.AddYears(-7);  // Példa: visszaállítjuk 7 évvel korábbra
            DisplayAlert("Hiba", "A kiválasztott dátum nem lehet késõbbi, mint az érvényes. Válasszon egy régebbi dátumot!", "Rendben");
        }
    }
}