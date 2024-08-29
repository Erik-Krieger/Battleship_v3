using System.ComponentModel;
using System.Windows;
using Battleship.Services;

namespace Battleship.Views;

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

    protected override void OnClosing( CancelEventArgs e )
    {
        NetworkService.Instance.Close();
        base.OnClosing( e );
    }
}
