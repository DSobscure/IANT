using UnityEngine;
using UnityEngine.UI;

public class AntManager : MonoBehaviour
{
    [SerializeField]
    private GameObject antPrefab;
    [SerializeField]
    private GameObject canvus;
    [SerializeField]
    private AntInfoController antInfoController;

    private float timeDelta = 0;

    public GameObject InstantiateNewAnt(float positionX, float positionY, float rotation)
    {
        GameObject ant = Instantiate(antPrefab, new Vector3(positionX, positionY, 3), Quaternion.AngleAxis(rotation, new Vector3(0,0,1))) as GameObject;
        ant.transform.SetParent(canvus.transform);
        ant.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => antInfoController.ShowInfo(ant.GetComponent<AntController>().ant));
        return ant;
    }

    void Update()
    {
        timeDelta += Time.deltaTime;
        if(timeDelta > 0.5f)
        {
            IANTGame.Game.AntFactory.FormationTraning(timeDelta);
            timeDelta -= 1;
        }
    }
}
