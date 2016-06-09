
namespace Managers
{
    public class ActionManager
    {
        public readonly AuthenticateActionManager AuthenticateActionManager = new AuthenticateActionManager();
        public readonly FetchDataActionManager FetchDataActionManager = new FetchDataActionManager();
        public readonly OperationManager OperationManager = new OperationManager();
    }
}
