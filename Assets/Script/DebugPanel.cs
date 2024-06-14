using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class DebugPanel : MonoBehaviour
    {
        public GameObject mainPanel;
        public Text tStage, tcFrame, tCount, tlCount, tMove, connected;
   
        public video video;
        
        void Update()
        {
            tMove.text = video.Move.ToString();
            tStage.text = video.Stage.ToString();
            tcFrame.text = video.cFrame.ToString();
            tCount.text = video.count.ToString();
            tlCount.text = video.lCount.ToString();
            connected.text = SocketServer.IsConnected() ? "連線成功" : "尚未連線";

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainPanel.SetActive(!mainPanel.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitGame();
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}