using System;
using UnityEngine;
using UnityEngine.Video;

namespace Script
{
    public enum SunState
    {
        SunRising, // 日出階段 0 ~ 150 
        SunSetting, // 日落階段 151 ~ 298
        IsNight // 已經夜晚了 299
    }
    
    public class SunPlayer : MonoBehaviour
    {
        public VideoPlayer vSun;
        
        public void PlayAtFirstFrame()
        {
            vSun.Stop();
            vSun.Play();
            vSun.enabled = true;
            vSun.playbackSpeed = 1f;
        }
        
        public SunState GetSunState()
        {
            return vSun.frame switch
            {
                <= 150 => SunState.SunRising,
                >= 299 => SunState.IsNight,
                _ => SunState.SunSetting
            };
        }

        public void PlaySunSet()
        {
            if (GetSunState() == SunState.SunSetting)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        

        public int GetFrame()
        {
            return (int)vSun.frame;
        }

        public void Pause()
        {
            vSun.playbackSpeed = 0f;
        }
        
        public void Resume()
        {
            vSun.playbackSpeed = 1f;
        }
    }
}