using UnityEngine;
using UnityEngine.UI;
using IANTLibrary;

public class BuildTowerController : MonoBehaviour
{
    [SerializeField]
    private CanvasScaler canvasScaler;
    [SerializeField]
    private RectTransform towerBlockPrefab;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private RectTransform panel;
    private RectTransform towerBlock;
    private bool isSelectingPosition = false;

    public void SelectBuildTowerPosition()
    {
        isSelectingPosition = true;
        towerBlock = Instantiate(towerBlockPrefab);
        towerBlock.transform.SetParent(panel);
        towerBlock.localScale = Vector3.one;
        towerBlock.GetComponent<Button>().onClick.AddListener(() => BuildTower());
    }
    public void BuildTower()
    {
        Tower tower;
        string errorMessage;

        isSelectingPosition = false;
        if(IANTGame.Game.TowerFactory.BuildTower(towerBlock.localPosition.x, towerBlock.localPosition.y, IANTGame.Game, out tower, out errorMessage))
        {
            RectTransform towerButton = (tower as ClientTower).TowerButton.GetComponent<RectTransform>();
            towerButton.transform.SetParent(panel);
            towerButton.localScale = Vector3.one;
            towerButton.localPosition = towerBlock.localPosition;
        }
        else
        {
            Debug.Log(errorMessage);
            IANTGame.InformManager.SystemInformManager.CallAlert(errorMessage);
        }
        Destroy(towerBlock.gameObject);
    }

    void Update()
    {
        if(isSelectingPosition)
        {
            towerBlock.localPosition = Input.mousePosition * canvasScaler.referenceResolution.y / Screen.height - new Vector3(canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y) / 2;
            if (Input.GetMouseButtonUp(1))
            {
                isSelectingPosition = false;
                if (towerBlock != null)
                {
                    Destroy(towerBlock.gameObject);
                }
            }
        }
    }
}
