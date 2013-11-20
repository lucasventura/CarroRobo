namespace CarroRobo.UI.RemoteControl.View
{
	using System.Windows;
	using ViewModel;

	/// <summary>
	/// Interaction logic for SelecionarDispositivoView.xaml
	/// </summary>
	public partial class SelecionarDispositivoView : Window
	{
		/// <summary>
		/// Construtor Padrão
		/// </summary>
		public SelecionarDispositivoView()
		{
			InitializeComponent();

			DataContext = new SelecionarDispositivoViewModel(this);
		}
	}
}
