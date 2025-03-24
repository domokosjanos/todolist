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
}