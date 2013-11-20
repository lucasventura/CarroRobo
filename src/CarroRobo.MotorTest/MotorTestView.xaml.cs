using System.Windows;

namespace CarroRobo.MotorTest
{
	/// <summary>
	/// Interaction logic for MotorTestView.xaml
	/// </summary>
	public partial class MotorTestView : Window
	{
		public MotorTestView()
		{
			InitializeComponent();

			DataContext = new MotorTestViewModel(this);
		}
	}
}
