using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using IANTLibrary;
using System.Collections.Generic;
using Managers;
using System;

public class PlayerInfoController : MonoBehaviour, IInformHandler
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Slider experiencePointsSlider;
    [SerializeField]
    private Text cakeCountText;
    [SerializeField]
    private Image profilePhoto;
    [SerializeField]
    private Text expText;

    void Start()
    {
        UpdatePlayerInfo(IANTGame.Player);
        IANTGame.OnProfileChange += OnProfileChangeAction;
        IANTGame.OnProfilePhotoChange += OnProfilePhotoChangeAction;
        IANTGame.Player.OnCakeCountChange += OnCakeCountChangeAction;
        IANTGame.Player.OnLevelChange += OnLevelChangeAction;
        IANTGame.Player.OnEXPChange += OnEXPChangeAction;
        experiencePointsSlider.onValueChanged.AddListener((value) => 
        {
            expText.text = string.Format("{0}/{1}", value, experiencePointsSlider.maxValue);
        });
        RegisterEvents(IANTGame.InformManager);
    }
    void OnDestroy()
    {
        IANTGame.OnProfileChange -= OnProfileChangeAction;
        IANTGame.OnProfilePhotoChange -= OnProfilePhotoChangeAction;
        IANTGame.Player.OnCakeCountChange -= OnCakeCountChangeAction;
        IANTGame.Player.OnLevelChange -= OnLevelChangeAction;
        IANTGame.Player.OnEXPChange -= OnEXPChangeAction;
        EraseEvents(IANTGame.InformManager);
    }

    public void UpdatePlayerInfo(Player player)
    {
        if(player != null)
        {
            int remainedEXP;
            int level = LevelEXPTable.GetLevel(player.EXP, out remainedEXP);
            int maxEXP = LevelEXPTable.EXPForUpgrade(player.Level);
            levelText.text = level.ToString();
            experiencePointsSlider.maxValue = maxEXP;
            experiencePointsSlider.value = remainedEXP;
            expText.text = string.Format("{0}/{1}", remainedEXP, maxEXP);
            cakeCountText.text = player.CakeCount.ToString();
            OnProfilePhotoChangeAction(IANTGame.ProfilePhoto);
            OnProfileChangeAction(IANTGame.ProfileResult);
        }
    }
    private void OnProfilePhotoChangeAction(Texture2D photo)
    {
        if(photo != null)
            profilePhoto.sprite = Sprite.Create(photo, new Rect(0, 0, 50f, 50f), new Vector2(0, 0));
    }
    private void OnProfileChangeAction(IDictionary<string,object> profile)
    {
        if(profile != null)
            nameText.text = (string)profile["name"];
    }
    private void OnCakeCountChangeAction(int number)
    {
        cakeCountText.text = number.ToString();
    }
    private void OnLevelChangeAction(int level)
    {
        levelText.text = level.ToString();
        experiencePointsSlider.maxValue = LevelEXPTable.EXPForUpgrade(level);
    }
    private void OnEXPChangeAction(int exp)
    {
        experiencePointsSlider.value = exp;
    }

    public void RegisterEvents(InformManager informManager)
    {
        informManager.GameInformManager.RegistrCakeNumberChangeInformFunction(OnCakeCountChangeAction);
    }

    public void EraseEvents(InformManager informManager)
    {
        informManager.GameInformManager.EraseCakeNumberChangeInformFunction(OnCakeCountChangeAction);
    }
}
