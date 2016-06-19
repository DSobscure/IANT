using Facebook.Unity;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HarvestModeController : MonoBehaviour, IResponseHandler
{
    [SerializeField]
    private HarvestModePanelUI harvestModePanelUI;
    [SerializeField]
    private FacebookAction facebookAction;

    private Dictionary<long, HarvestPlayerInfo> playerInfoDictionary;

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
        playerInfoDictionary = new Dictionary<long, HarvestPlayerInfo>();
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
        IANTGame.ActionManager.FetchDataActionManager.GetHarvestPlayerList(friendFBIDs.ToArray());
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.FetchDataResponseManager.RegistrGetHarvestPlayerListResponseFunction(OnGetHarvestPlayerListResponseAction);
        responseManager.OperationResponseManager.RegistrStartHarvestGameResponseFunction(OnStartHarvestGameResponseAction);
    }

    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.FetchDataResponseManager.EraseGetHarvestPlayerListResponseFunction(OnGetHarvestPlayerListResponseAction);
        responseManager.OperationResponseManager.EraseStartHarvestGameResponseFunction(OnStartHarvestGameResponseAction);
    }

    private void OnGetHarvestPlayerListResponseAction(HarvestPlayerInfo[] infos)
    {
        for(int i = 0; i < infos.Length; i++)
        {
            if(playerInfoDictionary.ContainsKey(infos[i].facebookID))
            {
                playerInfoDictionary[infos[i].facebookID] = new HarvestPlayerInfo
                {
                    facebookID = infos[i].facebookID,
                    photo = playerInfoDictionary[infos[i].facebookID].photo,
                    name = playerInfoDictionary[infos[i].facebookID].name,
                    level = infos[i].level,
                    harvestableCakeNumber = infos[i].harvestableCakeNumber
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
                        HarvestPlayerInfo info = playerInfoDictionary[fbID];
                        if(result.ResultDictionary.ContainsKey("name"))
                        {
                            playerInfoDictionary[fbID] = new HarvestPlayerInfo
                            {
                                facebookID = fbID,
                                photo = info.photo,
                                name = (string)result.ResultDictionary["name"],
                                level = info.level,
                                harvestableCakeNumber = info.harvestableCakeNumber
                            };
                            harvestModePanelUI.UpdateHarvestPlayerList(playerInfoDictionary.Values.ToList());
                        }
                    }
                });
                facebookAction.GetUserPhoto(fbID, (result) =>
                {
                    if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
                    {
                        HarvestPlayerInfo info = playerInfoDictionary[fbID];
                        if (result.Texture != null)
                        {
                            playerInfoDictionary[fbID] = new HarvestPlayerInfo
                            {
                                facebookID = fbID,
                                photo = result.Texture,
                                name = info.name,
                                level = info.level,
                                harvestableCakeNumber = info.harvestableCakeNumber
                            };
                            harvestModePanelUI.UpdateHarvestPlayerList(playerInfoDictionary.Values.ToList());
                        }
                    }
                });
            }
        }
        harvestModePanelUI.UpdateHarvestPlayerList(playerInfoDictionary.Values.ToList());
    }
    public void StartHarvestGame()
    {
        IANTGame.ActionManager.OperationManager.StartHarvestGame(IANTGame.HarvestFacebookID, harvestModePanelUI.GetUsedCakeNumber(), IANTGame.HarvestableCakeNumber);
    }
    private void OnStartHarvestGameResponseAction(int usedCakeNumber, string harvestTargetDefenceDataString, int harvestableCakeNumber)
    {
        IANTGame.Player.UseCake(usedCakeNumber);
        IANTGame.UsedCakeNumber = usedCakeNumber;
        IANTGame.HarvestTargetDefenceDataString = harvestTargetDefenceDataString;
        IANTGame.HarvestableCakeNumber = harvestableCakeNumber;
        IANTGame.Game = new ClientGame();
        IANTGame.GameType = "harvest";
        SceneManager.LoadScene("HarvestGame");
    }
}
