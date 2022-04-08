using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
public interface ICommand
{
    void Excute();
    void Undo();

    CommandData CreatCommandData();
}
public abstract class Command : ICommand
{
    public int frame;
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
    }
    public MoveCommand(CircleController circleController,CellMaker _cellMaker,int x,int y):this(circleController, _cellMaker)
    {
        moveVec.x = x;
        moveVec.y = y;
    }
    public MoveCommand(CircleController circleController, CellMaker _cellMaker, Vector3 _moveVec) : this(circleController, _cellMaker)
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
        return new CommandData(CommandEnum.move, frame, moveVec.ToString());
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

public enum CommandEnum
{
    move=0
}

public class CommandData
{
    public CommandEnum commandEnum;
    public int frame;
    public string parameter;
    public CommandData(){}
    public CommandData(CommandEnum _enum, int _frame, string _parameter)
    {
        commandEnum = _enum;
        frame = _frame;
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