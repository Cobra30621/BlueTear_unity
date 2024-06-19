using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    /// <summary>
    /// This class represents the Debug Panel in the game.
    /// It displays various game statistics and allows the user to interact with the game.
    /// </summary>
    public class DebugPanel : MonoBehaviour
    {
        public GameObject mainPanel;
        public Text tStage, tcFrame, tCount, tlCount, tMove;
        [FormerlySerializedAs("tGetCamera")] public Text tCaptureCamera;
        [FormerlySerializedAs("connected")] public Text tconnected;
        public Text tinitCount;

        [FormerlySerializedAs("video")] public GameProcess gameProcess;
        
        void Update()
        {
            tMove.text = $"move: {GameProcess.HandPose}";
            tStage.text = $"stage: {gameProcess.Stage}";
            tcFrame.text = $"sunFrame: {gameProcess.sunFrame}";
            tCount.text = $"count: {gameProcess.count}";
            tlCount.text = $"lightCount: {gameProcess.lightCount}";
            tinitCount.text = $"reloadCount: {gameProcess.reloadCount}";
            tconnected.text = $"與姿態偵測程式連線: {(SocketServer.IsConnected() ? "Success" : "None")}";
            string isCapture = SocketServer.IsCaptureCamera() && SocketServer.IsConnected() ? "Yes" : "No";
            tCaptureCamera.text = $"偵測到攝影機: {isCapture}";


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
        
        
        private void OnApplicationQuit()
        {
            Debug.Log($"App quit : {Time.time}, {GameProcess.HandPose}, {gameProcess.Stage}");
        }
    }
}