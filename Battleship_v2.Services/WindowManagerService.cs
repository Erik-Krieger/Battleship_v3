using Battleship_v2.ViewModels;

namespace Battleship_v2.Services;

public class WindowManagerService
{
	public static WindowManagerService Instance { get; private set; } = new WindowManagerService();


	public NavigationViewModel NavigationViewModel { get; private set; }

	private WindowManagerService()
	{
	}

	public void RegisterNavigationViewModel(NavigationViewModel theViewModel)
	{
		NavigationViewModel = theViewModel;
	}

	public static void ChangeView(BaseViewModel theViewModel)
	{
		if (theViewModel != null)
		{
			Instance.NavigationViewModel.SelectedViewModel = theViewModel;
		}
	}

	public static void OpenMainMenu()
	{
		NetworkService.Instance.Close();
		ChangeView(new MainMenuViewModel());
	}

	public static void OpenMultiplayerMenu()
	{
		NetworkService.Instance.Close();
		ChangeView(new MultiplayerSetupViewModel());
	}
}
