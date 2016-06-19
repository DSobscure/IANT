using Facebook.Unity;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeModeController : MonoBehaviour, IResponseHandler
{
    [SerializeField]
    private ChallengeModePanelUI challengeModePanelUI;
    [SerializeField]
    private FacebookAction facebookAction;

    private Dictionary<long, ChallengePlayerInfo> playerInfoDictionary;

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
        playerInfoDictionary = new Dictionary<long, ChallengePlayerInfo>();
        facebookAction.GetFriendList(GetFriendListCallBack);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }
    public void Refresh()
    {
        facebookAction.GetFriendList(GetFriendListCallBack);
    }
    private void GetFriendListCallBack(IResult result)
    {
        List<long> friendFBIDs = new List<long>();
        if (result != null)
        {
            foreach (var data in result.ResultDictionary["data"] as List<object>)
            {
                friendFBIDs.Add(long.Parse((data as Dictionary<string, object>)["id"].ToString()));
            }
        }
        IANTGame.ActionManager.FetchDataActionManager.GetChallengePlayerList(friendFBIDs.ToArray());
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.FetchDataResponseManager.RegistrGetChallengePlayerListResponseFunction(OnGetChallengePlayerListResponseAction);
        responseManager.OperationResponseManager.RegistrStartChallengeGameResponseFunction(OnStartChallengeGameResponseAction);
    }

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.FetchDataResponseManager.EraseGetChallengePlayerListResponseFunction(OnGetChallengePlayerListResponseAction);
        responseManager.OperationResponseManager.EraseStartChallengeGameResponseFunction(OnStartChallengeGameResponseAction);
    }

    private void OnGetChallengePlayerListResponseAction(ChallengePlayerInfo[] infos)
    {
        for(int i = 0; i < infos.Length; i++)
        {
            if(playerInfoDictionary.ContainsKey(infos[i].facebookID))
            {
                playerInfoDictionary[infos[i].facebookID] = new ChallengePlayerInfo
                {
                    facebookID = infos[i].facebookID,
                    photo = playerInfoDictionary[infos[i].facebookID].photo,
                    name = playerInfoDictionary[infos[i].facebookID].name,
                    level = infos[i].level,
                    nestLevel = infos[i].nestLevel
                };
            }
            else
            {
                long fbID = infos[i].facebookID;
                playerInfoDictionary.Add(fbID, infos[i]);
                facebookAction.GetUserName(fbID, (result) => 
                {
                    if (result != null)
                    {
                        ChallengePlayerInfo info = playerInfoDictionary[fbID];
                        if(result.ResultDictionary.ContainsKey("name"))
                        {
                            playerInfoDictionary[fbID] = new ChallengePlayerInfo
                            {
                                facebookID = fbID,
                                photo = info.photo,
                                name = (string)result.ResultDictionary["name"],
                                level = info.level,
                                nestLevel = info.nestLevel
                            };
                            challengeModePanelUI.UpdateChallengePlayerList(playerInfoDictionary.Values.ToList());
                        }
                    }
                });
                facebookAction.GetUserPhoto(fbID, (result) =>
                {
                    if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
                    {
                        ChallengePlayerInfo info = playerInfoDictionary[fbID];
                        if (result.Texture != null)
                        {
                            playerInfoDictionary[fbID] = new ChallengePlayerInfo
                            {
                                facebookID = fbID,
                                photo = result.Texture,
                                name = info.name,
                                level = info.level,
                                nestLevel = info.nestLevel
                            };
                            challengeModePanelUI.UpdateChallengePlayerList(playerInfoDictionary.Values.ToList());
                        }
                    }
                });
            }
        }
        challengeModePanelUI.UpdateChallengePlayerList(playerInfoDictionary.Values.ToList());
    }
    public void StartChallengeGame()
    {
        IANTGame.ActionManager.OperationManager.StartChallengeGame(IANTGame.ChallengeFacebookID, challengeModePanelUI.GetUsedCakeNumber());
    }
    private void OnStartChallengeGameResponseAction(int usedCakeNumber)
    {
        IANTGame.Player.UseCake(usedCakeNumber);
        IANTGame.UsedCakeNumber = usedCakeNumber;
        IANTGame.Game = new ClientGame();
        IANTGame.GameType = "challenge";
        SceneManager.LoadScene("Game");
    }
}
