using System.Windows.Controls;
using Battleship.ViewModels;

namespace Battleship.Views;

public partial class NavigationView : UserControl
{
    public NavigationView()
    {
        InitializeComponent();
        base.DataContext = new NavigationViewModel
        {
            SelectedViewModel = new MainMenuViewModel()
        };
    }

    /*[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}*/
}
