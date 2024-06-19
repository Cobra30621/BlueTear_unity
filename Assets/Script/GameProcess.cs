using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Script
{
    /// <summary>
    /// 遊戲主流程
    /// </summary>
    public class GameProcess : MonoBehaviour
    {
        public AudioSource ds, itro, bt;
        public AudioSource  People, Seagull; // Loop Audio
        public SunPlayer sunPlayer;
        public RawImage rSun, rLight, rSea;

        public VideoPlayerSetting videoPlayerSetting;
        
        /// <summary>
        /// 表示現在在哪個場景
        /// </summary>
        public string Stage;
        /// <summary>
        /// 目前的手勢
        /// </summary>
        public static string HandPose;
        
        /// <summary>
        /// 主要負責計時，同時也會是一些音量、顏色淡入淡出的數值
        /// </summary>
        public int count;
        /// <summary>
        /// 負責燈塔的記數、亮度控制
        /// </summary>
        public int lightCount;
        public int reloadCount = 0;
        public int reloadNeedSeconds = 180; 
        
        public int sunFrame;
        public bool bLight, change;
        
        public Transform Camera, iWind;
        public ParticleSystem pWind;
        public GameObject[] info = new GameObject[4];

        public StepDisplay stepDisplay;
        public FadeController fadeController;
    
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Seagull.Play();
            People.Play();
            
            InitGame();
        }

        /// <summary>
        /// 初始化遊戲
        /// </summary>
        private void InitGame()
        {
            count = 0; lightCount = 0;
            bLight = false; change = false;
            sunPlayer.PlayAtFirstFrame();
            
            Stage = "Init";
            itro.Play(); itro.volume = 0;
            ds.Play(); ds.volume = 0;

            Seagull.volume = 0;
            People.volume = 0;
            
            pWind.Stop();
            
            videoPlayerSetting.StopVideo(VideoName.Light);
            videoPlayerSetting.StopVideo(VideoName.BlueTear);
            videoPlayerSetting.StopVideo(VideoName.Wave);
        }

        #region Stage Handle

        void FixedUpdate()
        {
            switch (Stage)
            {
                case "Init":
                    HandleInitStage();
                    break;
                case "Sun":
                    HandleSunStage();
                    break;
                case "Light":
                    HandleLightStage();
                    break;
                case "Wind":
                    HandleWindStage();
                    break;
                case "BlueTear":
                    HandleBlueTearStage();
                    break;
            }

            HandleReloadCheck();
        }


        /// <summary>
        /// 開始場景(太陽升起)
        /// </summary>
        void HandleInitStage()
        {
            if (sunPlayer.GetSunState() == SunState.SunRising)
            {
                sunFrame = sunPlayer.GetFrame();
                ds.volume = sunFrame / 150f;
                Seagull.volume = sunFrame / 150f;
                
                sunPlayer.SetPlaySpeed(1);
            }
            else
            {
                sunPlayer.Pause();
                stepDisplay.Show("請比「剪刀」手勢，控制日落");
                Stage = "Sun";
            }
        }

        /// <summary>
        /// 日落場景
        /// </summary>
        void HandleSunStage()
        {
            sunFrame = sunPlayer.GetFrame();
            
            if (!change)
            {
                if (Input.GetKey(KeyCode.D) || (HandPose == "SunRight")) // SunRight
                {
                    sunPlayer.PlaySunSet();
                }
                else
                {
                    sunPlayer.SetPlaySpeed(0);
                }
                
                if (sunPlayer.GetSunState() == SunState.IsNight)
                {
                    if (count < 256)
                    {
                        count += 4;
                        Seagull.volume = 1 - (count / 256f);
                    }
                    else if (count >= 256)
                    {
                        change = true;
                        sunPlayer.SetPlaySpeed(0);
                        videoPlayerSetting.PlayVideo(VideoName.Light);
                    }
                }
                else
                {
                    if (count > 0)
                    {
                        count--;
                        Seagull.volume = 1 - (count / 256f);
                    }
                }
            }
            else
            {
                if (count > 0)
                {
                    stepDisplay.Hide();
                    info[1].SetActive(true);
                    Color color = rLight.color;
                    color.a = 1 - ((1 / 256f) * count);
                    rLight.color = color;
                    People.volume = 1 - ((1 / 256f) * count);
                    count--;
                }
                else
                {
                    change = false;
                    Color c = rSea.color;
                    c.a = 255;
                    rSea.color = c;
                    stepDisplay.Show("請比「石頭」手勢，關閉燈塔");
                    lightCount = 0;
                    Stage = "Light";
                }
            }
        }

        /// <summary>
        /// 關燈場景
        /// </summary>
        void HandleLightStage()
        {
            if (!change)
            {
                if (Input.GetKey(KeyCode.W) || (HandPose == "CloseLight")) // CloseLight
                {
                    bLight = false;
                }
                else
                {
                    bLight = true;
                }
                Light(bLight);

                ds.volume = vSea(1 - ((1 / 256f) * lightCount));
                People.volume = (1 - ((1 / 256f) * lightCount));
            
                if (rLight.color.a == 0)
                {
                    ds.volume = vSea(0);
                    itro.volume = 0;
                    itro.Play();
                    count = 350;
                    change = true;
                    
                    videoPlayerSetting.PlayVideo(VideoName.BlueTear);
                    videoPlayerSetting.PlayVideo(VideoName.Wave);
                }
                else
                {
                    if (count > 0)
                    {
                        count -= 2;
                        ds.volume = vSea(1 - ((1 / 256f) * count));
                        People.volume = (1 / 256f) * count;
                    }
                }
            }
            else
            {
                if (count > 0)
                {
                    if (count < 255)
                    {
                        stepDisplay.Hide();
                        info[2].SetActive(true);
                        Color color = rSun.color;
                        color.a = (1 / 256f) * count;
                        itro.volume = 1 - (1 / 256f) * count;
                        rSun.color = color;
                    }
                
                    count--;
                }
                else
                {
                    stepDisplay.Show("請比「讚」手勢，誠心祈禱");
                    lightCount = 0;
                    change = false;
                    pWind.Play();
                    Stage = "Wind";
                }
            }
        }
    
        /// <summary>
        /// 風吹場景
        /// </summary>
        void HandleWindStage()
        {
            if (!change)
            {
                if (itro.isPlaying == false)
                {
                    itro.Play();
                }
            
                if ((Input.GetKey(KeyCode.W) || (HandPose == "Pray")) && (Mathf.Abs(Camera.position.x) < 10)) // Pray
                {
                    count += 1;
                    if (count < 350)
                    {
                        count++;
                    }
                    else
                    {
                        count = 256;
                        change = true;
                    }
                }
                else
                {
                    Wind(0);
                }
        
            
                if (Input.GetKey(KeyCode.D) || (HandPose == "WindRight")) // WindRight
                {
                    Wind(1);
                }
                else if (Input.GetKey(KeyCode.A) || (HandPose == "WindLeft")) // WindLeft
                {
                    Wind(-1);
                }
            
            }
            else
            {
                itro.volume = 0;
                if (bt.isPlaying == false)
                {
                    pWind.Stop();
                    bt.Play();
                    bt.volume = 1;
                }
                if (count > 0)
                {
                    info[3].SetActive(true);
                    stepDisplay.Hide();
                    Color color = rSea.color;
                    color.a = 0.6f + ((1 / 256f) * count) * 0.4f;
                    rSea.color = color;
                    count--;
                }
                else
                {
                    change = false;
                    stepDisplay.Show("請比「七」手勢，放大畫面");
                    Stage = "BlueTear";
                }
            }
        }

        /// <summary>
        /// 藍眼淚場景
        /// </summary>
        void HandleBlueTearStage()
        {
            if (!change)
            {
                if (Input.GetKey(KeyCode.W) || (HandPose == "WindForward")) // WindForward
                {
                    BlueTear(true);
                }
                else
                {
                    BlueTear(false);
                }
                if (count < 2300)
                {
                    count++;
                }
                else
                {
                    change = true;
                }
            }
            else
            {
                if (count > 0)
                {
                    stepDisplay.Hide();
                    BlueTear(false);
                    Color color = rSun.color;
                    color.a = 1f - (count / 1000f);
                    rSun.color = color;
                    count -= 5;
                }
                else
                {
                    info[0].SetActive(true);
                    change = false;
                    InitGame();
                }
            }
        }

        
        /// <summary>
        /// 如果閒置過長，重新讀取場警
        /// </summary>
        private void HandleReloadCheck()
        {
            if (!Input.anyKey && (HandPose == "noPose") && Stage != "Sun")
            {
                reloadCount++;
            }
            else
            {
                reloadCount = 0;
            }
        
            if (reloadCount == reloadNeedSeconds * 3600)
            {
                reloadCount = 0;
                fadeController.StartFadeOut();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                fadeController.StartFadeOut();
            }
        }

    

        #endregion
    
        #region Tools
        
        void Light(bool open)
        {
            Color cLight = rLight.color;
            if (open)
            {
                lightCount--;
            }
            else
            {
                lightCount += 2;
            }
            lightCount = Mathf.Clamp(lightCount, 0, 255);
            cLight.a = 1-(lightCount / 255f);
            rLight.color = cLight;
        }
        void Wind(int dir)
        {
            float x = Camera.position.x;
            float d = Mathf.Abs(Camera.position.x);
            iWind.LookAt(Camera, Vector3.up);
            if (dir==1)
            {
                x+=2f;
            }
            else if (dir==-1)
            {
                x -= 2f;
            }
            else if (dir==0)
            {
                if (x > 0)
                {
                    x -= d/50f;
                }
                else if (x < 0)
                {
                    x += d/50f;
                }
            }
            x = Mathf.Clamp(x, -150f, 150f);
            Camera.position = new Vector3(x, Camera.position.y, Camera.position.z);
        }
        
        
        void BlueTear(bool zoom)
        {
            float z = Camera.position.z;
            float x = Camera.rotation.eulerAngles.x;
            float d = Mathf.Abs(Camera.position.z+150f);
            float a  = Mathf.Abs(Camera.eulerAngles.x -1f);
            if (zoom)
            {
                z += d / 50f;
                x -= a / 50f;
            }
            else
            {
                z -= d / 50f;
                x += a / 50f;
            }
            z = Mathf.Clamp(z, -800f, -200f);
            x = Mathf.Clamp(x, -5f, 0f);
            Camera.position = new Vector3(Camera.position.x, Camera.position.y, z);
            Camera.eulerAngles = new Vector3(x, Camera.eulerAngles.y, Camera.eulerAngles.z);
        }
        float vSea(float v)    {
            return (0.05f) + (v / 0.95f);
        }
    


        #endregion

    }
}