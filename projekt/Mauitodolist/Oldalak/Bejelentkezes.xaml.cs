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
                        //DisplayAlert("Jelszó Hash ellenõrzés", ellJelszo + " -választó- " + encoder.Encode(passwordEntry.Text), "Ok");
                        if (encoder.Compare(passwordEntry.Text, ellJelszo))
                        {
                            id = felhasznaloIndex + 1;
                            return true;
                        }
                        else
                        {
                            DisplayAlert("Hiba", "Hibás e-mail cím vagy jelszó!", "Ok");
                            return false;
                        }
                    }
                    else
                    {
                        DisplayAlert("Információ", "Nincs még ezzel a címmel felhasználó.", "Ok");
                        return false;
                    }
                }
                else
                {
                    DisplayAlert("Információ", "Nem létezik ezzel a címmel felhasználó.", "Ok");
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
    // Esemény, amely akkor hívódik meg, amikor a felhasználó gépel valamelyik mezõbe
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Ellenõrizzük, hogy mindkét Entry mezõ ki van-e töltve
        button_Bejelentkezes.IsEnabled = !string.IsNullOrWhiteSpace(emailEntry.Text) && !string.IsNullOrWhiteSpace(passwordEntry.Text);
    }
    // Itt történik a bejelentkezés lebonyolítása
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