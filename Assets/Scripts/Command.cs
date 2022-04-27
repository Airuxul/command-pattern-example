using System;
using UnityEngine;


/// <summary>
/// 命令接口
/// </summary>
public interface ICommand
{
    void Excute();
    void Undo();

    CommandData CreatCommandData();
}

/// <summary>
/// 命令抽象类
/// </summary>
public abstract class Command : ICommand
{
    public int frame;
    public int id;
    public virtual void Excute()
    {
        throw new System.NotImplementedException();
    }
    public virtual void Undo()
    {
        throw new System.NotImplementedException();
    }
    public virtual CommandData CreatCommandData()
    {
        throw new NotImplementedException();
    }
} 

/// <summary>
/// 移动命令类
/// </summary>
public class MoveCommand : Command
{
    private CircleController controller;
    private CellMaker cellMaker;
    private Vector3 moveVec;
    public MoveCommand(CircleController circleController, CellMaker _cellMaker)
    {
        controller = circleController;
        cellMaker = _cellMaker;
        frame = circleController.GetRealFrameCount();
        id = circleController.GetID();
    }
    public MoveCommand(CircleController circleController,CellMaker _cellMaker,int x,int y)
        :this(circleController, _cellMaker)
    {
        moveVec.x = x;
        moveVec.y = y;
    }
    public MoveCommand(CircleController circleController, CellMaker _cellMaker, Vector3 _moveVec) 
        : this(circleController, _cellMaker)
    {
        moveVec = _moveVec;
    }
    
    public override void Excute()
    {
        controller.transform.position = Vector3XYClamp(controller.transform.position + moveVec, 
            cellMaker.transform.position, cellMaker.GetMaxPos());
    }
    public override void Undo()
    {
        controller.transform.position = Vector3XYClamp(controller.transform.position - moveVec, 
            cellMaker.transform.position, cellMaker.GetMaxPos());
    }

    public override CommandData CreatCommandData()
    {
        return new CommandData(CommandEnum.move, id, frame, moveVec.ToString());
    }

    /// <summary>
    /// 仅对XY坐标进行Clamp操作，限制玩家移动范围
    /// </summary>
    /// <param name="val"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private Vector3 Vector3XYClamp(Vector3 val,Vector3 min,Vector3 max)
    {
        return new Vector3(Mathf.Clamp(val.x, min.x, max.x),
            Mathf.Clamp(val.y, min.y, max.y),
            0);
    }
}

/// <summary>
/// 命令枚举
/// </summary>
public enum CommandEnum
{
    move=0
}

/// <summary>
/// 命令信息类，用于json存储
/// </summary>
public class CommandData
{
    public CommandEnum commandEnum;
    public int frame;
    public int id;
    public string parameter;
    public CommandData(){}
    public CommandData(CommandEnum _enum,int _id, int _frame, string _parameter)
    {
        commandEnum = _enum;
        frame = _frame;
        id = _id;
        parameter = _parameter;
    }
    
    public override string ToString()
    {
        string enumStr="";
        switch (commandEnum)
        {
            case CommandEnum.move:
                enumStr = "Move";
                break;
        }
        return "Frame " + frame.ToString() + ":" + enumStr +" "+ parameter;
    }
}