using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Script
{
    /// <summary>
    /// 嘗試解決 Unity 閃退 bug
    /// 來自: https://discussions.unity.com/t/bizarre-videoplayer-errors-0x8007000e-unexpected-error-code-10-cannot-read-file-ran-out-of-memory/250612/2
    /// </summary>
    public class VideoPlayerSetting : MonoBehaviour
    {
        public VideoPlayer[] videoPlayers;

        public Dictionary<VideoName, VideoPlayer> videoPlayerDict;
        
        void Awake()
        {
            foreach (var videoPlayer in videoPlayers)
            {
                for (ushort i = 0; i < videoPlayer.audioTrackCount; i++)
                {
                    videoPlayer.EnableAudioTrack(i, false);
                }
            }
            
            videoPlayerDict = new Dictionary<VideoName, VideoPlayer>();
            videoPlayerDict[VideoName.Sun] = videoPlayers[0];
            videoPlayerDict[VideoName.Light] = videoPlayers[1];
            videoPlayerDict[VideoName.Wave] = videoPlayers[2];
            videoPlayerDict[VideoName.BlueTear] = videoPlayers[3];
        }

        public void PlayVideo(VideoName videoName)
        {
            var videoPlayer = videoPlayerDict[videoName];
            videoPlayer.Play();
            
        }

        public void StopVideo(VideoName videoName)
        {
            var videoPlayer = videoPlayerDict[videoName];
            videoPlayer.Pause();
        }
        
        
        
    }

    public enum VideoName
    {
        Sun,
        Light,
        Wave,
        BlueTear
    }
}