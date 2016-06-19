using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HarvestModePanelUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform harvestPlayerPanelPrefab;
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

    private List<HarvestPlayerInfo> harvestPlayerInfoa;

    private int pageNumber;
    void Start()
    {
        harvestPlayerInfoa = new List<HarvestPlayerInfo>();
        pageNumber = 1;
        usedCakeSlider.maxValue = Math.Min(IANTGame.Player.CakeCount, 10 * IANTGame.Player.Level);
        usedCakeSlider.minValue = 1;
        usedCakeSlider.onValueChanged.AddListener((value) => { usedCakeNumberText.text = ((int)value).ToString(); });
        usedCakeSlider.value = usedCakeSlider.maxValue;
    }

    public void UpdateHarvestPlayerList(List<HarvestPlayerInfo> infos)
    {
        harvestPlayerInfoa = infos;
        if (playerPanelContent == null)
            return;
        pageNumberText.text = string.Format("{0}/{1}", pageNumber, (int)Math.Ceiling((harvestPlayerInfoa.Count) / 5.0));
        int childs = playerPanelContent.gameObject.transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            Destroy(playerPanelContent.gameObject.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 5 && i < infos.Count - (pageNumber-1)*5; i++)
        {
            RectTransform playerBar = Instantiate(harvestPlayerPanelPrefab);
            playerBar.transform.SetParent(playerPanelContent);
            playerBar.localScale = Vector3.one;
            playerBar.localPosition = new Vector2(0, 180 - 90 * i);
            playerBar.GetChild(0).GetComponent<RawImage>().texture = infos[(pageNumber - 1) * 5 + i].photo;
            playerBar.GetChild(1).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].name;
            playerBar.GetChild(2).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].level.ToString();
            playerBar.GetChild(3).GetComponent<Text>().text = infos[(pageNumber - 1) * 5 + i].harvestableCakeNumber.ToString();
            long fbID = infos[(pageNumber - 1) * 5 + i].facebookID;
            int cakeNumber = infos[(pageNumber - 1) * 5 + i].harvestableCakeNumber;
            playerBar.GetChild(4).GetComponent<Button>().onClick.AddListener(() => 
            {
                IANTGame.HarvestFacebookID = fbID;
                IANTGame.HarvestableCakeNumber = cakeNumber;
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
        pageNumber = Math.Min(pageNumber + 1, (int)Math.Ceiling((harvestPlayerInfoa.Count) / 5.0));
        UpdateHarvestPlayerList(harvestPlayerInfoa);
    }
    public void PrevoiusPage()
    {
        pageNumber = Math.Max(pageNumber - 1, 1);
        UpdateHarvestPlayerList(harvestPlayerInfoa);
    }
}
