using Battleship.ViewModels;

namespace Battleship.Models;

public class MainMenuModel
{
    public MainMenuViewModel ViewModel { get; set; }

    public MainMenuModel( MainMenuViewModel theViewModel )
    {
        ViewModel = theViewModel;
    }

    public void PlaySinglePlayer()
    {
    }
}
