using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class XmlConfigTest : MonoBehaviour
{
    StringBuilder sb;
    void Start()
    {
        TextAsset  ta = Resources.Load("LevelEXP") as TextAsset;
        if(ta != null)
        {
            Debug.Log("ok");
        }
        XmlSerializer serializer = new XmlSerializer(typeof(LevelEXPTable));
        Stream s = new MemoryStream(ta.bytes);
        LevelEXPTable table = serializer.Deserialize(s) as LevelEXPTable;
        s.Close();
        sb = new StringBuilder();
        foreach(int i in table.upgradeEXP)
        {
            sb.AppendLine(i.ToString());
        }
    }

	void OnGUI ()
    {
        if(sb != null)
            GUI.TextArea(new Rect(100, 100, 300, 200),sb.ToString());
    }
}
public class LevelEXPTable
{
    public List<int> upgradeEXP = new List<int>();
}
