using Battleship_v2.Services;

namespace Battleship_v2.ViewModels;

public sealed class NavigationViewModel : BaseViewModel
{
	private object m_SelectedViewModel;

	public object SelectedViewModel
	{
		get
		{
			return m_SelectedViewModel;
		}
		set
		{
			SetProperty(ref m_SelectedViewModel, value, "SelectedViewModel");
		}
	}

	public NavigationViewModel()
	{
		WindowManagerService.Instance.RegisterNavigationViewModel(this);
	}
}
