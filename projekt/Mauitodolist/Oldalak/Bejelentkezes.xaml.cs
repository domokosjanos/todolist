using MauiToDoList.Model.adatbazis.tablak;
using Scrypt;
using SQLite;

namespace MauiToDoList.Oldalak;

public partial class Bejelentkezes : ContentPage
{
    ScryptEncoder encoder = new ScryptEncoder();
    Szolgaltatas szolgaltatas = new Szolgaltatas();
    int id;
    public Bejelentkezes()
    {
        InitializeComponent();
    }
    private bool Ellenorzes()
    {
        using (var connection = new SQLiteConnection(DBcsatlakozas.Utvonal, DBcsatlakozas.Flags))
        {
            var felhasznalok = szolgaltatas.GetTableData<Felhasznalo>();
            try
            {
                var felhasznaloIndex = felhasznalok.FindIndex(adat => adat.Email == emailEntry.Text);
                if (felhasznaloIndex != -1)
                {
                    //DisplayAlert("Valami", felhasznalok[felhasznaloIndex].Email, "Ok");
                    var felhasznalo = felhasznalok[felhasznaloIndex];

                    if (felhasznalo != null)
                    {
                        var ellJelszo = felhasznalo.Jelszo;
                        //DisplayAlert("Jelsz� Hash ellen�rz�s", ellJelszo + " -v�laszt�- " + encoder.Encode(passwordEntry.Text), "Ok");
                        if (encoder.Compare(passwordEntry.Text, ellJelszo))
                        {
                            id = felhasznaloIndex + 1;
                            return true;
                        }
                        else
                        {
                            DisplayAlert("Hiba", "Hib�s e-mail c�m vagy jelsz�!", "Ok");
                            return false;
                        }
                    }
                    else
                    {
                        DisplayAlert("Inform�ci�", "Nincs m�g ezzel a c�mmel felhaszn�l�.", "Ok");
                        return false;
                    }
                }
                else
                {
                    DisplayAlert("Inform�ci�", "Nem l�tezik ezzel a c�mmel felhaszn�l�.", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Hiba", ex.Message, "Ok");
                return false;
            }
        }
    }
    // Esem�ny, amely akkor h�v�dik meg, amikor a felhaszn�l� g�pel valamelyik mez�be
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Ellen�rizz�k, hogy mindk�t Entry mez� ki van-e t�ltve
        button_Bejelentkezes.IsEnabled = !string.IsNullOrWhiteSpace(emailEntry.Text) && !string.IsNullOrWhiteSpace(passwordEntry.Text);
    }
    // Itt t�rt�nik a bejelentkez�s lebonyol�t�sa
    private void button_Bejelentkezes_Clicked(object sender, EventArgs e)
    {
        if (Ellenorzes())
        {
            Navigation.PushAsync(new Fooldal(id));
        }
    }

    private void button_ElfelejtJelszo_Clicked(object sender, EventArgs e)
    {

    }

    private void button_Register_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Regisztracio());
    }

    
}