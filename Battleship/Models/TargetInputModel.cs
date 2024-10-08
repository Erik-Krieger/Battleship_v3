using Battleship.Services;
using Battleship.ViewModels;

namespace Battleship.Models;

public class TargetInputModel
{
    public TargetInputViewModel ViewModel { get; set; }

    public GameManagerService GameManager { get; set; }

    public TargetInputModel( TargetInputViewModel theViewModel )
    {
        ViewModel = theViewModel;
        GameManager = GameManagerService.Instance.InjectTargetInputModel( this );
    }

    public void ShootButtonPressed()
    {
        if ( GameManagerService.Instance.YourTurn )
        {
            GameManagerService.Instance.ProcessShot( ViewModel.TargetString );
            ViewModel.TargetString = string.Empty;
        }
    }
}
