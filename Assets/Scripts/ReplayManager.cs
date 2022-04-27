using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Replay管理类，继承mono单例模式
/// </summary>
public class ReplayManager : BaseMonoManager<ReplayManager>
{
    private readonly Dictionary<int, CircleController> controllerDic = new Dictionary<int, CircleController>();
    private List<CommandData> commands = new List<CommandData>(1000);
    private  int playerCount;
    [HideInInspector]
    public  int lastFrame = -1;
    [HideInInspector]
    public  int curFrame;
    private  int curIndex;
    
    [HideInInspector]
    public  bool isReplay;

    public bool playOnAwake;
    [HideInInspector]
    public  bool isPlay;
    
    /// <summary>
    /// 导入所有的Controller
    /// </summary>
    /// <param name="circleController"></param>
    /// <returns></returns>
    public int InputController(CircleController circleController)
    {
        ++playerCount;
        controllerDic[playerCount] = circleController;
        return playerCount;
    }
    
    /// <summary>
    /// 记录命令类
    /// </summary>
    /// <param name="data"></param>
    public void InputCommand(CommandData data)
    {
        commands.Add(data);
    }
    
    /// <summary>
    /// 保存命令到Json
    /// </summary>
    public void SaveToJson()
    {
        Debug.Log("Save Commands");
        JsonDataMgr.GetInstance().SaveToJson(commands,"Commands","Commands.json");
    }
    /// <summary>
    /// 开始播放
    /// </summary>
    public void StartReplay()
    {
        LoadJson();
        isReplay = true;
        isPlay = playOnAwake;
    }

    /// <summary>
    /// 自动播放更新
    /// </summary>
    private void Update()
    {
        if (isReplay&& isPlay)
        {
            ++curFrame;
            if (curFrame > commands[curIndex].frame)
            {
                controllerDic[commands[curIndex].id].ControlByCommandData(commands[curIndex]);
                curIndex++;
                if (curIndex >= commands.Count)
                {
                    isPlay = false;
                }
            }
        }
    }

    /// <summary>
    /// 到达目标帧
    /// </summary>
    /// <param name="targetFrame"></param>
    public void GoTargetFrame(int targetFrame)
    {
        bool isExcute = targetFrame > curFrame;
        while (curFrame != targetFrame)
        {
            curFrame=curFrame+(isExcute?1:-1);
            if (isExcute&&curFrame > commands[curIndex].frame || !isExcute && curFrame < commands[curIndex].frame)
            {
                controllerDic[commands[curIndex].id].ControlByCommandData(commands[curIndex],isExcute);
                curIndex=Mathf.Max(0,curIndex+(isExcute?1:-1));
            }
        }
    }

    /// <summary>
    /// 加载命令Json文件
    /// </summary>
    private void LoadJson()
    {
        Rest();
        //获取存储的json信息
        commands=JsonDataMgr.GetInstance().LoadFromJson<List<CommandData>>("Commands", "Commands.json");
        if (commands.Count!=0)
        {
            //给个50帧的延时
            lastFrame = commands[commands.Count-1].frame;
        }
        else
        {
            Debug.LogError("json中没有命令，请输入后存储再加载");
        }
    }
    
    /// <summary>
    /// 重置所有控制类的位置
    /// </summary>
    public void Rest()
    {
        foreach (var pair in controllerDic)
        {
            pair.Value.Rest();
        }
        isReplay = false;
        lastFrame = -1;
        curFrame = 0;
        curIndex = 0;
    }
}