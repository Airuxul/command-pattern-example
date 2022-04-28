using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Replay管理类，继承mono单例模式
/// </summary>
public class ReplayManager : BaseMonoManager<ReplayManager>
{
    /// <summary>
    /// circle控制字典
    /// </summary>
    private readonly Dictionary<int, CircleController> controllerDic = new Dictionary<int, CircleController>();
    /// <summary>
    /// 命令集
    /// </summary>
    private List<CommandData> commands = new List<CommandData>(1000);
    private  int playerCount;
    
    /// <summary>
    /// 录像最后帧
    /// </summary>
    [HideInInspector]
    public  int lastFrame = -1;
    /// <summary>
    /// 当前播放帧
    /// </summary>
    private int _curFrame;
    /// <summary>
    /// 当前播放帧属性
    /// </summary>
    public int curFrame
    {
        get => _curFrame;
        set => GoTargetFrame(value);
    }
    
    /// <summary>
    /// 到达目标帧
    /// </summary>
    /// <param name="targetFrame"></param>
    private void GoTargetFrame(int targetFrame)
    {
        bool isExcute = targetFrame > _curFrame;
        while (_curFrame != targetFrame)
        {
            _curFrame += (isExcute?1:-1);
            if (isExcute &&_curFrame >= commands[curIndex].frame || !isExcute && _curFrame <= commands[curIndex].frame)
            {
                controllerDic[commands[curIndex].id].ControlByCommandData(commands[curIndex],isExcute);
                curIndex=Mathf.Clamp(curIndex+(isExcute?1:-1),0,commands.Count-1);
            }
        }
    }
    
    /// <summary>
    /// 当前命令的下标
    /// </summary>
    private  int curIndex;

    /// <summary>
    /// 是否处于录像模式
    /// </summary>
    public bool isReplay { private set; get; }

    /// <summary>
    /// 设定是否开始播放
    /// </summary>
    public bool playOnAwake;

    /// <summary>
    /// 是否处于自动播放模式
    /// </summary>
    public bool isPlay { set; get; }

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
            if (curIndex >= commands.Count)
            {
                isPlay = false;
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
        _curFrame = 0;
        curIndex = 0;
    }
}