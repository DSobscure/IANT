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
    private int time;

	void Start ()
    {
        nest = IANTGame.Player.Nests[0];
        if(IANTGame.GameType == "challenge")
        {
            nest = IANTGame.BattleNest;
        }
        blocks = new SpriteRenderer[48, 30];
        for(int x=0;x<48;x++)
        {
            for(int y=0;y<30;y++)
            {
                blocks[x, y] = Instantiate(blockPrefab, new Vector3(x * 20 - 480, y * 20 - 300, 1), Quaternion.identity) as SpriteRenderer;
                blocks[x, y].transform.SetParent(weightAnchor.transform);
            }
        }
    }

    void Update()
    {
        time++;
        if (time % 600 == 0)
        {
            float sum = 0;
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    blocks[x+24, y+15].color = new Color(1, 1, 1, 0.3f + 50 * (float)nest.distributionMap.distributionWeight[x, y]);
                }
            }
            //for (int x = 0; x < 48; x++)
            //{
            //    for (int y = 0; y < 30; y++)
            //    {
            //        blocks[x, y].color = new Color(1, 1, 1, 0.6f + nest.weightsForSurvive[x, y]);
            //    }
            //}
        }
        //else 
        //if (time % 60 == 0)
        //{
        //    float sum = 0;
        
        //}
    }
}
