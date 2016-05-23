using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;

public class StartMenuAction : MonoBehaviour {

    [SerializeField]
    private StartMenuUI startMenuUI;
    // Use this for initialization
    void Start ()
    {
        //Debug.Log("initialing facebook ....");
        //InitialFacebook();
    }
	
	public void InitialFacebook()
    {
        InitDelegate onInitialComplete = () =>
        {
            startMenuUI.resultText.text = "initial complete\n";
            if (FB.IsInitialized)
            {
                startMenuUI.resultText.text = "initial successiful waiting for login\n";
                LoginWithFacbook();
            }
            else
            {
                startMenuUI.resultText.text = "facebook 初始化失敗";
            }
        };
        startMenuUI.resultText.text = "waiting for initial response\n";
        FB.Init(onInitialComplete);
    }

    private void LoginWithFacbook()
    {
        FacebookDelegate<ILoginResult> loginCallBack = (result) =>
        {
        startMenuUI.resultText.text = "login complete\n";
            if (FB.IsLoggedIn)
            {
                startMenuUI.resultText.text = "login successiful\n";
                IANTGame.ActionManager.AuthenticateActionManager.Login(result.AccessToken.UserId, result.AccessToken.TokenString);
            }
        };
        startMenuUI.resultText.text = "waiting for login response\n";
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, loginCallBack);
    }
}
