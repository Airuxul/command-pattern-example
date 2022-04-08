using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandsCarrier: MonoBehaviour
{
    private List<CommandData> commands=new List<CommandData>(100000);
    public CircleController circleController;
    public Scrollbar bar;
    private int finalFrame=-1;
    private float curFrame = 0;
    private int curIndex = 0;
    
    public bool isLoad = false;
    public bool overLoad = false;
    public void AddCommands(Command command)
    {
        commands.Add(command.CreatCommandData());
    }
    public void Reset()
    {
        //命令载体Reset
        commands.Clear();
        finalFrame = -1;
        
        //小圆恢复到原位置
        circleController.Rest();
        
        //UI部分，mask小时的调用函数在外面button event中
        //Bar设置可移动
        bar.interactable =false;
        bar.value = 0;

        isLoad = false;
        overLoad = false;
    }
    public void SaveToJson()
    {
        Debug.Log("Save Commands");
        JsonDataMgr.GetInstance().SaveToJson(commands,"Commands","Commands.json");
    }

    public void LoadCommands()
    {
        //命令载体播放开始数据重置
        curFrame = 0;
        curIndex = 0;
        bar.value = 0;
        //获取存储的json信息
        commands=JsonDataMgr.GetInstance().LoadFromJson<List<CommandData>>("Commands", "Commands.json");
        finalFrame = commands[commands.Count-1].frame;
        
        //小圆恢复到原位置
        circleController.Rest();
        
        //UI部分，mask出现的调用函数在外面button event中
        //Bar设置可移动
        //bar.interactable = true;

        overLoad = false;
        isLoad = true;
    }

    private void Update()
    {
        if(!isLoad||overLoad){return;}
        ++curFrame;
        bar.value = curFrame / finalFrame;
        while (commands[curIndex].frame <= curFrame)
        {
            circleController.ControlByCommandData(commands[curIndex]);
            curIndex++;
            if (curIndex == commands.Count)
            {
                overLoad = true;
                return;
            }
        }
        
    }
}
