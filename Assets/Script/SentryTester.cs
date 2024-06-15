using UnityEngine;

namespace Script
{
    /// <summary>
    /// 用來測試使用 Sentry 插件時，是否能順利報錯
    /// </summary>
    public class SentryTester : MonoBehaviour
    {
        public GameObject nullObject;
        
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log(nullObject.name);
            }
        }
    }
}