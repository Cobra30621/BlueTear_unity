using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class DebugPanel : MonoBehaviour
    {
        public GameObject mainPanel;
        public Text tStage, tcFrame, tCount, tlCount, tMove;
        [FormerlySerializedAs("tGetCamera")] public Text tCaptureCamera;
        [FormerlySerializedAs("connected")] public Text tconnected;
        public Text tinitCount;

        public video video;
        
        void Update()
        {
            tMove.text = $"Move: {video.Move}";
            tStage.text = $"Stage: {video.Stage}";
            tcFrame.text = $"cFrame: {video.cFrame}";
            tCount.text = $"Count: {video.count}";
            tlCount.text = $"lCount: {video.lCount}";
            tinitCount.text = $"initCount: {video.initCounter}";
            tconnected.text = $"connected: {(SocketServer.IsConnected() ? "連線成功" : "尚未連線")}";
            string iscaptureCamera = SocketServer.IsCaptureCamera() && SocketServer.IsConnected() ? "有" : "無";
            tCaptureCamera.text = $"偵測到攝影機: {iscaptureCamera}";


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