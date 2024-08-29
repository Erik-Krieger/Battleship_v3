using System.Windows.Input;
using Battleship.Models;
using Battleship.Services;
using Battleship.Utility;

namespace Battleship.ViewModels;

public sealed class TargetInputViewModel : BaseViewModel
{
    private string m_TargetString = string.Empty;

    private ICommand m_CmdShoot;

    public string TargetString
    {
        get
        {
            return m_TargetString;
        }
        set
        {
            SetProperty( ref m_TargetString, value, "TargetString" );
        }
    }

    public TargetInputModel Model { get; set; }

    public ICommand CmdShoot => m_CmdShoot ?? ( m_CmdShoot = new CommandHandler( delegate
    {
        Model.ShootButtonPressed();
    }, () => GameManagerService.Instance.YourTurn ) );

    public TargetInputViewModel()
    {
        Model = new TargetInputModel( this );
    }
}
