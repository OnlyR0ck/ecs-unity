using R3;
using VS.Runtime.Core.UI;

namespace VS.Runtime.Meta.UI
{
    public class LobbyViewModel : ViewModel
    {
        public ReactiveCommand EnterGame { get; } =  new();
    }
}