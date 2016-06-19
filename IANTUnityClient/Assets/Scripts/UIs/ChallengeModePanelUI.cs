using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ChallengeModePanelUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform challengePlayerPanelPrefab;
    [SerializeField]
    private RectTransform playerPanelContent;
    [SerializeField]
    private Text usedCakeNumberText;
    [SerializeField]
    private Slider usedCakeSlider;
    [SerializeField]
    private GameObject selectCakePanel;
    [SerializeField]
    private Text pageNumberText;

    private List<ChallengePlayerInfo> challengePlayerInfos;

    private int pageNumber;
    void Start()
    {
        challengePlayerInfos = new List<ChallengePlayerInfo>();
        pageNumber = 1;
        pageNumberText.text = string.Format("{0}/{1}", pageNumber, (int)Math.Ceiling((challengePlayerInfos.Count) / 5.0));
        usedCakeSlider.maxValue = Math.Min(IANTGame.Player.CakeCount, 10);
        usedCakeSlider.minValue = 1;
        usedCakeSlider.onValueChanged.AddListener((value) => { usedCakeNumberText.text = ((int)value).ToString(); });
        usedCakeSlider.value = usedCakeSlider.maxValue;
    }

    public void UpdateChallengePlayerList(List<ChallengePlayerInfo> infos)
    {
        challengePlayerInfos = infos;

        if (playerPanelContent == null)
            return;
        pageNumberText.text = string.Format("{0}/{1}", pageNumber, (int)Math.Ceiling((challengePlayerInfos.Count) / 5.0));
        int childs = playerPanelContent.gameObject.transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            Destroy(playerPanelContent.gameObject.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5 && i < infos.Count - (pageNumber-1)*5; i++)
        {
            RectTransform playerBar = Instantiate(challengePlayerPanelPrefab);
            playerBar.transform.SetParent(playerPanelContent);
            playerBar.localScale = Vector3.one;
            playerBar.localPosition = new Vector2(0, 180 - 90 * i);
            playerBar.GetChild(0).GetComponent<RawImage>().texture = infos[(pageNumber - 1) * 5 + i].photo;
            playerBar.GetChild(1).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].name;
            playerBar.GetChild(2).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].level.ToString();
            playerBar.GetChild(3).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].nestLevel.ToString();
            long fbID = infos[(pageNumber - 1) * 5 + i].facebookID;
            playerBar.GetChild(4).GetComponent<Button>().onClick.AddListener(() => 
            {
                IANTGame.ChallengeFacebookID = fbID;
                selectCakePanel.SetActive(true);
            });
        }
    }
    public int GetUsedCakeNumber()
    {
        return (int)usedCakeSlider.value;
    }
    public void NextPage()
    {
        pageNumber = Math.Min(pageNumber+1, (int)Math.Ceiling((challengePlayerInfos.Count) / 5.0));
        UpdateChallengePlayerList(challengePlayerInfos);
    }
    public void PrevoiusPage()
    {
        pageNumber = Math.Max(pageNumber-1, 1);
        UpdateChallengePlayerList(challengePlayerInfos);
    }
}
