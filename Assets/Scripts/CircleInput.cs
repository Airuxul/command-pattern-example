using System;
using UnityEngine;

/// <summary>
/// circleInput设置类
/// </summary>
[Serializable]
public class CircleInput
{
    private CircleController Controller;
    private int horizontalMove;
    private int verticalMove;
    public KeyCode leftButton=KeyCode.A;
    public KeyCode rightButton=KeyCode.D;
    public KeyCode upButton=KeyCode.W;
    public KeyCode downButto=KeyCode.S;

    public CircleInput(CircleController controller) { SetController(controller);}
    public void SetController(CircleController controller) { Controller = controller;}
    public MoveCommand GetMoveCommand()
    {
        horizontalMove = (Input.GetKeyDown(leftButton) ? -1 : 0) + (Input.GetKeyDown(rightButton) ? 1 : 0);
        verticalMove = (Input.GetKeyDown(downButto) ? -1 : 0) + (Input.GetKeyDown(upButton) ? 1 : 0);
        if (horizontalMove == 0 && verticalMove == 0)
        {
            return null;
        }
        return new MoveCommand(Controller, Controller.cellMaker,horizontalMove, verticalMove);
    }
}
