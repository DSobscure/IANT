using UnityEngine;
using System.Xml.Serialization;
using System.Linq;
using System.IO;
using System.Text;

public class SerializationTest : MonoBehaviour
{
    public UnityEngine.UI.Text t;
	void Start ()
    {
        double[] ds = new double[11*11];
        ds[1*11 + 1] = 0.000000000000000000000000000000000000000001;
        ds[2*11 + 2] = double.Epsilon;
        StringBuilder sb = new StringBuilder();
        foreach(double d in ds)
        {
            sb.Append(d);
            sb.Append(",");
        }
        t.text = (sb.ToString());
        Debug.Log(t.text.Length);
        Debug.Log(sb.ToString().Split(',').Length);
        double[] ds2 = new double[11 * 11];
        string[] sbs = sb.ToString().Split(',');
        for(int i = 0; i < 121; i++)
        {
            ds2[i] = System.Convert.ToDouble(sbs[i]);
        }
        Debug.Log(ds2[1 * 11 + 1] == 0.000000000000000000000000000000000000000001);
        //XmlSerializer serializer = new XmlSerializer(typeof(double[]));
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    serializer.Serialize(ms, ds);
        //    t.text = Encoding.ASCII.GetString(ms.ToArray());
        //    Debug.Log(t.text.Length);
        //}
        //double[,] ds2 = JsonUtility.FromJson<double[,]>(t.text);
        //t.text = (ds2[2, 2] == 2).ToString();

    }
}
