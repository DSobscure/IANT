using Managers;

public interface IResponseHandler
{
    void RegisterEvents(ResponseManager responseManager);
    void EraseEvents(ResponseManager responseManager);
}