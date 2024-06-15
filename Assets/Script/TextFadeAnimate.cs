using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    /// <summary>
    /// 切緩場景的文字提示動畫
    /// </summary>
    public class TextFadeAnimate : MonoBehaviour
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
            this.transform.position = new Vector3(0, 0, 0);
        }
        void FixedUpdate()
        {
            if (count < 256)
            {
                Vector3 p = r.transform.position;
                p.z -= 0.5f;
                r.transform.position = p;
                Color c = r.color;
                c.a = count / 125f;
                r.color = c;
                count++;
            }
            else if((count>=256)&&(count<384))
            {
                Vector3 p = r.transform.position;
                p.z -= 0.5f;
                r.transform.position = p;
                Color c = r.color;
                c.a = 1 - ((count - 256) / 128f);
                r.color = c;
                count++;
            }
            else
            {
                this.gameObject.SetActive(false);
            }

        }


    }
}

