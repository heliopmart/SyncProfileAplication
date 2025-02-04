using WindowsApp.Views;
namespace WindowsApp
{
    public class TestApp : Form
    {
        [STAThread] // Necessário para o Windows Forms
        public static void Main(string[] args)
        {
            /*
                TODO:
                Criar uma função que atualize os dados do metadata.yaml com os dados do firebase

                Verificar a funcionalidade metatada.yaml quando ele próprio não existe
            */
            try
            {
                Application.Run(new SetApp()); 
            }
            catch(Exception error)
            {
                MessageBox.Show($"Erro ao inicializar, error: {error}");
            }
        }
    }
}
