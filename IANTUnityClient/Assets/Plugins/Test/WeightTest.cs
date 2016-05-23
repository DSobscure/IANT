using UnityEngine;
using IANTLibrary;

public class WeightTest : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer blockPrefab;
    [SerializeField]
    private SpriteRenderer smallBlockPrefab;
    [SerializeField]
    private GameObject weightAnchor;
    [SerializeField]
    private GameObject formationAnchor;

    private SpriteRenderer[,] blocks;
    private SpriteRenderer[,] formationBlocks;

    public Nest nest;

	void Start ()
    {
        blocks = new SpriteRenderer[96, 60];
        for(int x=0;x<96;x++)
        {
            for(int y=0;y<60;y++)
            {
                blocks[x, y] = Instantiate(blockPrefab, new Vector3(x * 10 - 480, y * 10 - 300, 1), Quaternion.identity) as SpriteRenderer;
                blocks[x, y].transform.SetParent(weightAnchor.transform);
            }
        }
        formationBlocks = new SpriteRenderer[96, 60];
        for (int x = 0; x < 96; x++)
        {
            for (int y = 0; y < 60; y++)
            {
                formationBlocks[x, y] = Instantiate(smallBlockPrefab, new Vector3(x * 2, y * 2, 1), Quaternion.identity) as SpriteRenderer;
                formationBlocks[x, y].transform.SetParent(formationAnchor.transform);
            }
        }
    }

	void Update ()
    {
        for (int x = 0; x < 96; x++)
        {
            for (int y = 0; y < 60; y++)
            {
                blocks[x, y].color = new Color(1, 1, 1, 128f/255 + (nest.weightsForSurvive[x, y] + nest.weightsForCake[x, y])/2);
                formationBlocks[x, y].color = new Color(1, 1, 1, 128f / 255 + (nest.formationWeight[x, y]));
            }
        }
    }
}
