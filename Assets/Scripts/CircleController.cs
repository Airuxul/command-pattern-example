using UnityEngine;

//具体逻辑，玩家的Input->command->人物的控制

/// <summary>
/// 小圆的控制器
/// </summary>
public class CircleController : MonoBehaviour
{
    /// <summary>
    /// 输入类
    /// </summary>
    [SerializeField]
    private CircleInput circleInput;
    
    //地图类
    public CellMaker cellMaker;
    /// <summary>
    /// 字典对应id
    /// </summary>
    public int id { get; private set; }
    
    /// <summary>
    /// 开始帧
    /// </summary>
    private int startFrame;

    /// <summary>
    /// 通过临时变量减少Update的消耗
    /// </summary>
    private Command tempCommd;

    private void Start()
    {
        //初始化输入模块
        circleInput.SetController(this);

        //存入ReplayManager中
        id=ReplayManager.GetInstance().InputController(this);
        
        //位置归到地图原点
        Rest();
    }
    
    void Update()
    {
        //如果处于录像模式，直接返回
        if (ReplayManager.GetInstance().isReplay)
        {
            return;
        }
        
        //获取Input输入命令
        tempCommd=circleInput.GetMoveCommand();
        //不为空则执行且存入录像
        if (tempCommd != null)
        {
            tempCommd.Excute();
            ReplayManager.GetInstance().InputCommand(tempCommd.CreatCommandData());
            tempCommd = null;
        }
    }
    /// <summary>
    /// 重置小圆位置，设置开始帧
    /// </summary>
    public void Rest()
    {
        var initialPos = cellMaker.transform.position;
        transform.position = new Vector3(initialPos.x, initialPos.y, 0);
        startFrame = Time.frameCount;
    }
    /// <summary>
    /// 获取相对帧
    /// </summary>
    /// <returns></returns>
    public int GetRealFrameCount()
    {
        return Time.frameCount - startFrame;
    }

    #region 通过命令数据控制小圆
    /// <summary>
    /// 根据命令信息来控制小圆
    /// </summary>
    /// <param name="data"></param>
    /// <param name="isExcute"></param>
    public void ControlByCommandData(CommandData data,bool isExcute=true)
    {
        tempCommd = ParseCommandData(data);
        if (isExcute)
        {
            tempCommd.Excute();
        }
        else
        {
            tempCommd.Undo();
        }
    }
    
    /// <summary>
    /// 根据data创建Command类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private Command ParseCommandData(CommandData data)
    {
        switch (data.commandEnum)
        {
            case CommandEnum.move:
                return new MoveCommand(this, cellMaker, Vector3Parse(data.parameter));
        }
        return null;
    }
    
    /// <summary>
    /// 根据vecString解析Vector3
    /// </summary>
    /// <param name="vecString"></param>
    /// <returns></returns>
    private Vector3 Vector3Parse(string vecString)
    {
        vecString = vecString.Replace('(',  ' ').Replace(')',' ');
        string[] nums = vecString.Split(',');
        return new Vector3(float.Parse(nums[0]), float.Parse(nums[1]), float.Parse(nums[2]));
    }
    #endregion
}
