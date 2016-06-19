using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
using Managers;
using System;
using UnityEngine.SceneManagement;

public class FacebookAction : MonoBehaviour, IResponseHandler
{

    [SerializeField]
    private StartMenuUI startMenuUI;

    void Start()
    {
        RegisterEvents(IANTGame.ResponseManager);
    }
    void OnDestroy()
    {
        EraseEvents(IANTGame.ResponseManager);
    }

    public void RegisterEvents(ResponseManager responseManager)
    {
        responseManager.AuthenticationResponseManager.RegistrLoginResponseFunction(OnFacebookLoginResponse);
    }
    public void EraseEvents(ResponseManager responseManager)
    {
        responseManager.AuthenticationResponseManager.EraseLoginResponseFunction(OnFacebookLoginResponse);
    }
    public void InitialFacebook()
    {
        InitDelegate onInitialComplete = () =>
        {
            startMenuUI.resultText.text = "initial complete";
            if (FB.IsInitialized)
            {
                startMenuUI.resultText.text = "initial successiful waiting for login";
                LoginWithFacbook();
            }
            else
            {
                startMenuUI.resultText.text = "facebook 初始化失敗";
            }
        };
        startMenuUI.resultText.text = "waiting for initial response";
        FB.Init(onInitialComplete);
    }
    private void LoginWithFacbook()
    {
        FacebookDelegate<ILoginResult> loginCallBack = (result) =>
        {
            startMenuUI.resultText.text = "login complete";
            if (FB.IsLoggedIn)
            {
                startMenuUI.resultText.text = "login successiful";
                IANTGame.FacebookID = result.AccessToken.UserId;
                IANTGame.FacebookAccessToken = result.AccessToken.ToString();
                IANTGame.ActionManager.AuthenticateActionManager.Login(result.AccessToken.UserId, result.AccessToken.TokenString);
                FB.API("/me", HttpMethod.GET, ProfileCallBack);
                FB.API("/me/picture", HttpMethod.GET, this.ProfilePhotoCallback);
            }
        };
        startMenuUI.resultText.text = "waiting for login response";
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "user_friends" }, loginCallBack);
    }

    private void OnFacebookLoginResponse()
    {
        SceneManager.LoadScene("Main");
    }
    protected void ProfileCallBack(IResult result)
    {
        if(result != null)
        {
            IANTGame.ProfileResult = result.ResultDictionary;
        }
    }
    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
        {
            IANTGame.ProfilePhoto = result.Texture;
        }
    }
    public void GetFriendList(FacebookDelegate<IGraphResult> callBackFunction)
    {
        FB.API("/me/friends?fields=id,name", HttpMethod.GET, callBackFunction);
    }
    public void GetUserName(long facebooID, FacebookDelegate<IGraphResult> callBackFunction)
    {
        FB.API(string.Format("/{0}", facebooID), HttpMethod.GET, callBackFunction);
    }
    public void GetUserPhoto(long facebooID, FacebookDelegate<IGraphResult> callBackFunction)
    {
        FB.API(string.Format("/{0}/picture", facebooID), HttpMethod.GET, callBackFunction);
    }
}
