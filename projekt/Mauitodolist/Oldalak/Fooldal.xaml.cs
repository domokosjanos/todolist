using MauiToDoList.Model.adatbazis.kod;
using MauiToDoList.Model.adatbazis.tablak;
using MauiToDoList.Oldalak.MenuPages.CsoportPages;
using MauiToDoList.Oldalak.MenuPages.FeladataimPages;
using MauiToDoList.Oldalak.MenuPages.FeladatPages;
using MauiToDoList.Oldalak.MenuPages.ProfilPages;

namespace MauiToDoList.Oldalak;

public partial class Fooldal : ContentPage
{
    static int FHO_id;
    static Szolgaltatas szolgaltatas = new Szolgaltatas();
    private Viewmodel_FHO Viewmodel_FHO = new Viewmodel_FHO();
    
    public Fooldal(int id)
	{
        var felhasznalok = szolgaltatas.GetTableData<Felhasznalo>().ToList();
        Viewmodel_FHO.Aktfelhasznalo = felhasznalok.Find(x=>x.Id == id);
        InitializeComponent();
        FHO_id = id;
        FnevUdvozlo.Text = $"Üdvözöllek, {Viewmodel_FHO.Aktfelhasznalo.Fnev}!";
    }

    private void Button_feladataim_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private void Button_csoportok_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private void Button_feladatkeszit_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private void Button_profilom_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profilok(FHO_id));
    }

    
}