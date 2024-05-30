using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class InfoDisplay : MonoBehaviour
    {
        public Text infoText;

        [SerializeField] int count;
        [SerializeField] int wait;

        
        [SerializeField] private string testInfo;
        
        [ContextMenu("Test")]
        public void Test()
        {
            Show(testInfo);
        }
        

        public void Show(string info)
        {
            infoText.text = info;
            infoText.gameObject.SetActive(true);

            count = 0;
            
            var color = infoText.color;
            color.a = 0;
            infoText.color = color;
            infoText.transform.localPosition = new Vector3(0, 0, 0);
        }

        public void Hide()
        {
            infoText.gameObject.SetActive(false);
        }


        void FixedUpdate()
        {
            if (count < 256)
            {
                Color c = infoText.color;
                c.a = count / 256f;
                infoText.color = c;
                count++;
            }
            else if ((count >= 256) && (count < 512))
            {
                Vector3 p = infoText.transform.localPosition;
                p.z -= 0.1f;
                infoText.transform.localPosition = p;
                count++;
            }
            else if ((count >= 512) && (count < 768))
            {
                Vector3 p = infoText.transform.localPosition;
                p.z += 0.1f;
                infoText.transform.localPosition = p;
                count++;
            }
            else
            {
                count = 256;
            }

        }
    }
}