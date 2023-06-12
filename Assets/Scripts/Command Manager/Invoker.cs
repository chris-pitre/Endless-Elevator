using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker
{
    Queue<ICommand> commands;
    public Invoker(){
        commands = new Queue<ICommand>();
    }

    public void Execute(ICommand command){
        if(command != null){
            commands.Enqueue(command);
            commands.Dequeue().Execute();
        }
    }
}
