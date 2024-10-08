using Battleship.Services;
using Battleship.ViewModels;

namespace Battleship.Models;

public class GameModel
{
    private GameViewModel m_ViewModel;

    public GameViewModel ViewModel { get; set; }

    public GameModel( GameViewModel theGameViewModel )
    {
        ViewModel = theGameViewModel;
        GameManagerService.Instance.InjectShipGridModel( ViewModel.OwnGrid.Model );
        GameManagerService.Instance.InjectShipGridModel( ViewModel.EnemyGrid.Model, isOwnGrid: false );
    }
}
