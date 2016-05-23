using UnityEngine;
using UnityEngine.UI;

public class BuildTowerButtonController : MonoBehaviour
{
	void Update ()
    {
	    if(IANTGame.Game.TowerFactory.IsPositionLegal(transform.position.x, transform.position.y))
        {
            GetComponent<Image>().color = new Color(0, 1, 0, 88f/255f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 0, 0, 88f / 255f);
        }
	}
}
