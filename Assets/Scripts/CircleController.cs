using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//具体逻辑，玩家的Input->command->人物的控制

/// <summary>
/// 小圆的控制器
/// </summary>
public class CircleController : MonoBehaviour
{
    private CircleInput circleInput;
    public CellMaker cellMaker;
    public CommandsCarrier commandsCarrier;
    private int startFrame = 0;
    private void Start()
    {
        circleInput = new CircleInput(this);
        //位置归到地图原点
        transform.position = new Vector3(cellMaker.transform.position.x, cellMaker.transform.position.y, 0);
    }
    void Update()
    {
        if (commandsCarrier.isLoad)
        {
            return;
        }
        MoveCommand command=circleInput.GetMoveCommand();
        if (command != null)
        {
            command.Excute();
            commandsCarrier.AddCommands(command); 
        }
    }
    public void Rest()
    {
        transform.position = new Vector3(cellMaker.transform.position.x, cellMaker.transform.position.y, 0);
        startFrame = Time.frameCount;
    }
    public int GetRealFrameCount()
    {
        return Time.frameCount - startFrame;
    }

    #region 通过命令数据控制小圆
    public void ControlByCommandData(CommandData data,bool isExcute=true)
    {
        Command command = ParseCommandData(data);
        if (isExcute)
        {
            command.Excute();
        }
        else
        {
            command.Undo();
        }
    }
    private Command ParseCommandData(CommandData data)
    {
        switch (data.commandEnum)
        {
            case CommandEnum.move:
                return new MoveCommand(this, cellMaker, Vector3Parse(data.parameter));
        }
        return null;
    }
    private Vector3 Vector3Parse(string vecString)
    {
        vecString = vecString.Replace('(',  ' ').Replace(')',' ');
        string[] nums = vecString.Split(',');
        return new Vector3(float.Parse(nums[0]), float.Parse(nums[1]), float.Parse(nums[2]));
    }
      #endregion
}
