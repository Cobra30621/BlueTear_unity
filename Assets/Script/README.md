# 程式說明


## Game Progress.cs
負責遊戲的主流控制。

目前程式碼挺肥大，不過要重構的牽扯到很多東西， 重構成本偏高(沒重構好會變成很複雜、很難讀懂架構)
因此決定先維持現狀。


### 遊戲流程 (2024/06/16 版本)

![blueTear_flowChart.jpg](Ducument%2FblueTear_flowChart.jpg)


### 重要變數

#### 狀態類

- Stage: 表示現在在哪個場景，包含 
  - Init: 開始遊戲初始化
  - Sun: 白天 (A)
  - Light: 夜晚-燈塔(B)
  - Wind: 夜晚-南風(C)
  - BlueTear: 藍眼淚
- HandPose: 目前的手勢

#### 記數類

- count: 
  - 主要負責計時，同時也會是一些音量、顏色淡入淡出的數值
  - 目前過度耦合，但是要解耦合有點困難就先擱置了
- lightCount
  - 負責燈塔的記數、亮度控制
- reloadCount、reloadNeedCount
  - 遊戲每 5 分鐘，如果沒有操作就會重新讀取場景
  - 負責此功能的記數
- sunFrame, maxFrame
  - 負責 Sun 場景，日落的影片撥放


### 重要方法


#### Stage Handle(流程控制)

- FixedUpdate() : 每偵更新，根據現在的場景，執行對應的方法
  - HandleInitStage() : 負責處理開始場景每偵更新
  - HandleSunStage() : 負責處理白天場景每偵更新
  - HandleLightStage(): 負責處理燈塔場景每偵更新
  - HandleWindStage(): 負責處理南風場景每偵更新
  - HandleBlueTearStage(): 負責處理藍眼淚場景每偵更新
-  HandleReloadCheck(): 執行遊戲每 5 分鐘，如果沒有操作就會重新讀取場景

#### Tools

- Sun(): 控制日落影片
- Light(): 控制燈光明亮
- Wind(): 控制風吹
- BlueTear(): 控制藍眼淚鏡頭縮放


## SocketServer.cs
- 負責與手勢偵測 python App 連線


