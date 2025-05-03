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
            Message = (Emailellen == null) ? "a felhaszn�l�n�v" : "az email";
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
        // Ellen�rizz�k, hogy a sz�let�si d�tum ki van-e v�lasztva
        DateTime selectedDate = szuletesDatePicker.Date;
        DateTime today = DateTime.Now.Date;  // Mai nap d�tuma (id� n�lk�li)

        // Ha a sz�let�si d�tum j�v�beli, mint a mai nap van, vagy 6 �vvel kor�bbi d�tum van megadva, akkor hiba�zenet
        if (selectedDate >= today || selectedDate >= today.AddYears(-6))
        {
            DisplayAlert("Hiba", "A kiv�lasztott d�tum nem lehet j�v�beli. V�lasszon egy r�gebbi d�tumot!", "OK");
            return; // Ha nem megfelel� a d�tum, kil�p�nk a regisztr�ci�s folyamatb�l
        }

        // SQLite kapcsolat l�trehoz�sa
        using (var connection = new SQLiteConnection(DBcsatlakozas.Utvonal, DBcsatlakozas.Flags))
        {
            connection.CreateTable<Felhasznalo>(); //Felhasznalo t�bl�hoz csatlakoz�s
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
                DisplayAlert("Inform�ci�", "Sikeres regisztr�ci�", "OK");

                // Ellen�rizz�k, hogy t�nyleg beker�lt-e az �j felhaszn�l�
                var felhasznalo = connection.Table<Felhasznalo>().FirstOrDefault(x => x.Fnev == ujFelhasznalo.Fnev);

                if (felhasznalo != null)
                {
                    // Ki�rjuk az adatokat, hogy t�nyleg elmentett�k �ket
                    string userDetails = $"Felhaszn�l�n�v: {felhasznalo.Fnev}\n" +
                                         $"N�v: {felhasznalo.Vnev} {felhasznalo.Knev}\n" +
                                         $"Email: {felhasznalo.Email}\n" +
                                         $"Sz�let�si d�tum: {felhasznalo.Szul_ido.ToShortDateString()}\n" +
                                         $"Telefonsz�m: {felhasznalo.Tszam}";
                    //DisplayAlert("Sikeres regisztr�ci�", $"A k�vetkez� adatokat mentett�k el:\n\n{userDetails}", "OK");
                }

                // Ha minden rendben van, �tir�ny�tjuk a felhaszn�l�t a Bejelentkez�s oldalra
                Navigation.PushAsync(new Bejelentkezes());
            }
            else
            {
                DisplayAlert("Hiba", $"Ez {Message} m�r foglalt. Adj meg egy m�sikat!", "Ok");
            }
        }
    }


    private void Telefon_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry != null)
        {
            // Csak sz�mok maradjanak
            entry.Text = new string(entry.Text.Where(c => char.IsDigit(c)).Take(15).ToArray());
        }
    }

    private void szuletesDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;
        DateTime today = DateTime.Now.Date;  // Mai nap d�tuma (id� n�lk�li)

        // Ellen�rizz�k, hogy a d�tum j�v�beli, mint a mai nap �s nem t�lt�tte be a 6 �ves kort.
        if (selectedDate >= today || selectedDate >= today.AddYears(-6))
        {
            // Ha nem megfelel� d�tumot v�lasztottak, akkor �ll�tsuk vissza egy �rv�nyes d�tumra
            szuletesDatePicker.Date = today.AddYears(-7);  // P�lda: vissza�ll�tjuk 7 �vvel kor�bbra
            DisplayAlert("Hiba", "A kiv�lasztott d�tum nem lehet k�s�bbi, mint az �rv�nyes. V�lasszon egy r�gebbi d�tumot!", "Rendben");
        }
    }
}