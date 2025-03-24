using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak;

namespace MauiToDoList
{
    public partial class App : Application
    {
        //public static Adatszerkesztes<Felhasznalo> Adatszerkesztes {  get; private set; }
        //Adatszerkesztes<Felhasznalo> szerkesztes
        public App()
        {
            InitializeComponent();
            //Adatszerkesztes = szerkesztes;
            MainPage = new NavigationPage(new Bejelentkezes());
        }
    }
}
