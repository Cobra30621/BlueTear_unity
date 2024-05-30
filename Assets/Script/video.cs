
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Threading;
using Script;
using UnityEngine.Serialization;


public class video : MonoBehaviour
{
    public AudioSource ds, itro, ready, bt, People, Seagull;
    public VideoPlayer vSun, vLight;
    public RawImage rSun, rLight, rSea, rBlueTear;
    public string Stage;
    public int cFrame, maxFrame, count, lCount;
    public int initCounter = 0, initNeedCount = 3600; // if idle too long, back to init
    public bool reverse, bLight, change;
    public Transform Camera, iWind;
    public ParticleSystem pWind;
    public Text tStage, tcFrame, tCount, tlCount, tMove;
    public GameObject[] info = new GameObject[4];

    public StepDisplay stepDisplay;
    public FadeController fadeController;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        vSun.frame = 0;
        maxFrame = 599;
        count = 0; lCount = 0;
        reverse = false; bLight = false; change = false;
        vSun.enabled = true; vLight.enabled = true;
        vSun.playbackSpeed = 1f; vLight.playbackSpeed = 1f;
        vSun.Play(); vLight.Play();
        Stage = "Init";
        itro.Play(); itro.volume = 0;
        ready.Play(); ready.volume = 0;
        bt.Play(); bt.volume = 0;
        ds.Play(); ds.volume = 0;
        Seagull.Play(); Seagull.volume = 0;
        People.Play(); People.volume = 0;
        pWind.Stop();
    }

    #region Stage Handle

    void FixedUpdate()
    {
        UpdateDebugUI();
        
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

        HandleBackToInit();
    }

    

    void HandleInitStage()
    {
        if (vSun.frame <= 150)
        {
            ds.volume = vSun.frame / 150f;
            Seagull.volume = vSun.frame / 150f;
            vSun.playbackSpeed = 1f;
        }
        else
        {
            vSun.playbackSpeed = 0f;
            stepDisplay.Show("舉起右手比剪刀，控制日落");
            Stage = "Sun";
        }
    }

    void HandleSunStage()
    {
        if (!change)
        {
            if (Input.GetKey(KeyCode.D) || (tMove.text == "SunRight")) // SunRight
            {
                Sun(1);
            }
            else if (Input.GetKey(KeyCode.A) || (tMove.text == "SunLeft")) // SunLeft
            {
                Sun(-1);
            }
            else
            {
                Sun(0);
            }
            if (vSun.frame == 299 || vSun.frame == 300)
            {
                if (count < 256)
                {
                    count++;
                    Seagull.volume = 1 - (count / 256f);
                }
                else if (count >= 256)
                {
                    change = true;
                    vSun.frame = 0;
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
                stepDisplay.Show("右手置於胸前，手心面對畫作，握拳緩緩關閉燈塔");
                lCount = 0;
                Stage = "Light";
            }
        }
    }

    void HandleLightStage()
    {
        if (!change)
        {
            if (Input.GetKey(KeyCode.W) || (tMove.text == "CloseLight")) // CloseLight
            {
                bLight = false;
            }
            else
            {
                bLight = true;
            }
            Light(bLight);

            if (rLight.color.a == 0)
            {
                if (count < 256)
                {
                    count++;
                    ds.volume = vSea(1 - ((1 / 256f) * count));
                    People.volume = (1 - ((1 / 256f) * count));
                }
                else if (count >= 256)
                {
                    ds.volume = vSea(0);
                    itro.volume = 0;
                    ready.volume = 0;
                    itro.Play();
                    ready.Play();
                    change = true;
                }
            }
            else
            {
                if (count > 0)
                {
                    count--;
                    ds.volume = vSea(1 - ((1 / 256f) * count));
                    People.volume = (1 / 256f) * count;
                }
            }
        }
        else
        {
            if (count > 0)
            {
                stepDisplay.Hide();
                info[2].SetActive(true);
                Color color = rSun.color;
                color.a = (1 / 256f) * count;
                itro.volume = 1 - (1 / 256f) * count;
                rSun.color = color;
                count--;
            }
            else
            {
                stepDisplay.Show("南風吹拂之日，右手比讚，誠心祈禱");
                lCount = 0;
                change = false;
                pWind.Play();
                Stage = "Wind";
            }
        }
    }

    void HandleWindStage()
    {
        if (!change)
        {
            if (itro.isPlaying == false)
            {
                itro.Play();
                ready.Play();
            }
            if (lCount < 768)
            {
                lCount++;
            }
            if (Input.GetKey(KeyCode.D) || (tMove.text == "WindRight")) // WindRight
            {
                Wind(1);
            }
            else if (Input.GetKey(KeyCode.A) || (tMove.text == "WindLeft")) // WindLeft
            {
                Wind(-1);
            }
            else if ((Input.GetKey(KeyCode.W) || (tMove.text == "Pray")) && (Mathf.Abs(Camera.position.x) < 10)) // Pray
            {
                count += 1;
                if (count < 256)
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
                    count--;
                }
                Wind(0);
            }
        }
        else
        {
            if (ready.isPlaying == true)
            {
                ready.volume = 1;
                itro.volume = 0;
            }
            else
            {
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
                    stepDisplay.Show("右手比 7 ，擁抱碧波蒼茫");
                    Stage = "BlueTear";
                }
            }
        }
    }

    void HandleBlueTearStage()
    {
        if (!change)
        {
            if (Input.GetKey(KeyCode.W) || (tMove.text == "WindForward")) // WindForward
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
                Stage = "Init";
            }
        }
    }

    // if idle too long, back to init    
    private void HandleBackToInit()
    {
        if (!Input.anyKey && (tMove.text == "noPose"))
        {
            initCounter++;
        }
        else
        {
            initCounter = 0;
        }

        if (initCounter > 3600)
        {
            fadeController.StartFadeOut();
        }

        if (Input.GetKey(KeyCode.R))
        {
            fadeController.StartFadeOut();
        }
    }

    

    #endregion
    
    #region Tools

    void Sun(int kInt)
    {
        cFrame = (int)vSun.frame;
        if (kInt==1)
        {
            if (reverse)
            { 
                vSun.frame = maxFrame - cFrame;
                
                reverse = false;
            }

            if ((cFrame<0) || (cFrame >= 299))
            {
                vSun.playbackSpeed = 0f;
            }
            else
            {
                vSun.playbackSpeed = 1f;
            }
            
        }
        else if (kInt==-1)
        {
            if (!reverse)
            {
                vSun.frame = maxFrame - cFrame;
                
                reverse = true;
            }
            if ((cFrame < 299) || (cFrame >= 600))
            {
                vSun.playbackSpeed = 0f;
            }
            else
            {
                vSun.playbackSpeed = 1f;
            }

        }
        else if (kInt==0)
        {
            vSun.playbackSpeed = 0f;
        }
        else if (kInt == 2)
        {
            vSun.frame = 0;
        }
        
    }
    void Light(bool open)
    {
        Color cLight = rLight.color;
        if (open)
        {
            lCount--;
        }
        else
        {
            lCount++;
        }
        lCount = Mathf.Clamp(lCount, 0, 255);
        cLight.a = 1-(lCount / 255f);
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
    
    private void UpdateDebugUI()
    {
        tStage.text = Stage.ToString();
        tcFrame.text = cFrame.ToString();
        tCount.text = count.ToString();
        tlCount.text = lCount.ToString();
    }

    #endregion

}