using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views;

public partial class NavigationView : UserControl, IComponentConnector
{
	public NavigationView()
	{
		InitializeComponent();
		base.DataContext = new NavigationViewModel
		{
			SelectedViewModel = new MainMenuViewModel()
		};
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
