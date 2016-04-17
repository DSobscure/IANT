using UnityEngine;
using System.Collections;

public class AntManager : MonoBehaviour
{
    [SerializeField]
    private GameObject antPrefab;
	
    public GameObject InstantiateNewAnt(float positionX, float positionY)
    {
        return Instantiate(antPrefab, new Vector3(positionX, positionY, 3), Quaternion.identity) as GameObject;
    }
}
