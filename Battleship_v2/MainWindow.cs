using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Battleship_v2.Services;

namespace Battleship_v2;

public partial class MainWindow : Window, IComponentConnector
{
	public MainWindow()
	{
		InitializeComponent();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		NetworkService.Instance.Close();
		base.OnClosing(e);
	}
}
