using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class step : MonoBehaviour
{
    RawImage r;
    int count;
    int wait;
    void OnEnable()
    {
        r = this.GetComponent<RawImage>();
        Color c = r.color;
        c.a = 0;
        r.color = c;
        count = 0;
        this.transform.position = new Vector3(0, -529, -1000);
    }
    void FixedUpdate()
    {
        if (count < 256)
        {
            Color c = r.color;
            c.a = count / 256f;
            r.color = c;
            count++;
        }
        else if ((count >= 256) && (count < 512))
        {
            Vector3 p = r.transform.position;
            p.z -= 0.1f;
            r.transform.position = p;
            count++;
        }
        else if ((count >= 512) && (count < 768))
        {
            Vector3 p = r.transform.position;
            p.z += 0.1f;
            r.transform.position = p;
            count++;
        }
        else
        {
            count = 256;
        }

    }
}
