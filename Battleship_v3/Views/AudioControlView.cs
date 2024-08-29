using System.Windows.Controls;
using System.Windows.Markup;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views;

public partial class AudioControlView : UserControl, IComponentConnector
{
	public AudioControlView()
	{
		InitializeComponent();
		base.DataContext = new AudioControlViewModel();
	}
}
