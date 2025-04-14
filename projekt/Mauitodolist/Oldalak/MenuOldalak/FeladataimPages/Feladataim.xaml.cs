using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladatPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak.MenuPages.FeladataimPages;

public partial class Feladataim : ContentPage
{
    int FHO_id;
	public Feladataim(int id)
	{
		InitializeComponent();
        FHO_id = id;
	}

    private void Buttonfooldal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Fooldal(FHO_id));
    }

    private void feladat_keszit_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private void csoport_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private void profil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profilok(FHO_id));
    }

    public async void BetoltesFeladatok()
    {
        var connection = DBcsatlakozas.CreateConnection();

        var feladatok = await connection.Table<Feladat>()
                                       .Where(f => f.FHO_id == FHO_id)
                                       .ToListAsync();

        FeladatokListView.ItemsSource = feladatok;
    }

}