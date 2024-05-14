
using SQLite;
 
namespace sites;

public partial class MainPage : ContentPage
{
    string dbPath;//indica onde esta o banco de dados

    SQLiteConnection conn; //representa onde esta o conexao com o banco de dados

    public MainPage()
    {
        InitializeComponent();
    }

    public void ListarSites()
    {
        List<Site> lista = conn.Table<Site>().ToList();
        ListaClv.ItemsSource = lista;
    }

    private void CriaBancoBt_Clicked(object sender, EventArgs e)
    {
        dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "sites.db3");
        conn = new SQLiteConnection(dbPath);
        conn.CreateTable<Site>();
        itensvsl.IsVisible = true;

        ListarSites();
    }

    private void InserirBt_Clicked(object sender, EventArgs e)
    {
        Site dado = new Site();
        dado.Id = Convert.ToInt32(IdEt.Text);
        dado.Endereco = EnderecoEt.Text;

        try
        {
            conn.Insert(dado);

            DisplayAlert("Cadastro", "cadastro efetuado com sucesso", "OK");
            IdEt.Text = "";
            EnderecoEt.Text = "";
        }

        catch
        {
            DisplayAlert("Cadastro", "Site ja cadastrado", "OK");
        }

        ListarSites();
    }

    private void AlterarBt_Clicked(object sender, EventArgs e)
    {
        Site site = new Site();
        site.Id = Convert.ToInt32(IdEt.Text);
        site.Endereco = EnderecoEt.Text;

        conn.Update(site);
        IdEt.Text = "";
        EnderecoEt.Text = "";
        DisplayAlert("Update", 
            "Registro atualizado com sucesso", "OK");

        ListarSites();
    }

    private void ExcluirBt_Clicked(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(IdEt.Text);
        conn.Delete<Site>(id);
        IdEt.Text = "";
        EnderecoEt.Text = "";
        DisplayAlert("Delete",
            "Registro deletado com sucesso!!!", "OK");

        ListarSites();
    }

    private void LocalizarBt_Clicked(object sender, EventArgs e)
    {
            int id = Convert.ToInt32(IdEt.Text);
            string endereco = EnderecoEt.Text;

            var sites = from s in conn.Table<Site>()
                        where s.Id == id
                        select s;

            //var sites = from s in conn.Table<Site>()
            //            where s.Endereco == endereco
            //            select s;

        Site site = sites.First();
            IdEt.Text = site.Id.ToString();
            EnderecoEt.Text = site.Endereco;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var label = sender as Label;
        if (label != null && label.BindingContext is Site item)
        {
            IdEt.Text = item.Id.ToString();
            EnderecoEt.Text = item.Endereco;
        }
    }
}
