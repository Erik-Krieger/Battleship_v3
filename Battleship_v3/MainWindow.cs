using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using Battleship_v2.Services;

namespace Battleship_v2;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

    public void Connect( int connectionId, object target )
    {
        throw new System.NotImplementedException();
    }

    public void InitializeComponent()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnClosing(CancelEventArgs e)
	{
		NetworkService.Instance.Close();
		base.OnClosing(e);
	}
}
