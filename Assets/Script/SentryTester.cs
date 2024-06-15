using UnityEngine;

namespace Script
{
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