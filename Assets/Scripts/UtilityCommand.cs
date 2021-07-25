using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCommandBase
{
    private string _commandID;
    private string _commandDescription;
    private string _commandFormat;

    public string commandID { get { return _commandID; }}
    public string commandDescription { get { return _commandDescription; }}
    public string commandFormat { get { return _commandFormat; }}
    
    public UtilityCommandBase(string id,string description, string format)
    {
        _commandID = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class UtilityCommand : UtilityCommandBase
{
    private Action command;

    public UtilityCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class UtilityCommand<T1> : UtilityCommandBase
{
    private Action<T1> command;

    public UtilityCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

public class UtilityCommand<T1,T2>: UtilityCommandBase
{
    private Action<T1,T2> command;

    public UtilityCommand(string id, string description, string format, Action<T1,T2> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value1,T2 value2)
    {
        command.Invoke(value1, value2);
    }
}

