using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CircleInput
{
    private CircleController Controller;
    private int horizontalMove=0;
    private int verticalMove=0;
    public CircleInput(CircleController controller) { Controller = controller;}
    public MoveCommand GetMoveCommand()
    {
        horizontalMove = (Input.GetKeyDown(KeyCode.A) ? -1 : 0) + (Input.GetKeyDown(KeyCode.D) ? 1 : 0);
        verticalMove = (Input.GetKeyDown(KeyCode.S) ? -1 : 0) + (Input.GetKeyDown(KeyCode.W) ? 1 : 0);
        if (horizontalMove == 0 && verticalMove == 0)
        {
            return null;
        }
        return new MoveCommand(Controller, Controller.cellMaker,horizontalMove, verticalMove);
    }
}
