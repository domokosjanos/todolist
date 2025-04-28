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
    private Dictionary<int, Felhasznalo> Felhasznalok = new Dictionary<int, Felhasznalo>();
    /*
    <CollectionView x:Name="FelhasznaloDataGrid" ItemsSource="{Binding}">
        <CollectionView.ItemTemplate>
            <DataTemplate>
                <Border StrokeShape="RoundRectangle 10,10,10,10">
                    <Grid ColumnDefinitions="*">
                        <Label Text="{Binding Fnev}" VerticalOptions="Center"/>
                        <Label Text="{Binding Vnev}" VerticalOptions="Center"/>
                        <Label Text="{Binding Email}" VerticalOptions="Center"/>
                    </Grid>
                </Border>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
    */
    public Fooldal(int id)
	{
        var felhasznalok = szolgaltatas.GetTableData<Felhasznalo>().ToList();
        Felhasznalok = felhasznalok.ToDictionary(x => x.Id);
        InitializeComponent();
        FHO_id = id;
        FnevUdvozlo.Text = $"Üdvözöllek, {felhasznalok[id - 1].Fnev}!";
        if (Felhasznalok.ContainsKey(id))
        {
            var felhasznalo = new List<Felhasznalo> { Felhasznalok[id] };
            BindingContext = felhasznalo;
        }
    }

    private async void Button_feladataim_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Feladataim(FHO_id));
    }

    private async void Button_csoportok_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Csoportok(FHO_id));
    }

    private async void Button_feladatkeszit_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Feladatok(FHO_id));
    }

    private async void Button_profilom_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;

        if (button != null)
        {
            await button.ScaleTo(0.95, 100, Easing.CubicIn);
            await button.ScaleTo(1, 100, Easing.CubicOut);
        }
        await Navigation.PushAsync(new Profilok(FHO_id));
    }

    
}