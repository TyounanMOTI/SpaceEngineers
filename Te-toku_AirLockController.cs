const string endl = "\r\n";

void Main(string argument)
{
    Setup();

    switch (argument) {
        case "Entered Mid Sensor":
            OnEnteredMidSensor();
            break;
        case "Leaved Mid Sensor":
            OnLeavedMidSensor();
            break;
        case "Entered Inner Sensor":
            OnEnteredInnerSensor();
            break;
        case "Leaved Inner Sensor":
            OnLeavedInnerSensor();
            break;
        case "":
            Debug_GetActions();
            break;
        default:
            Write("Invalid sensor event :(");
            return;
    }
}

IMyAirVent airVent;
IMyDoor midDoor;
IMyDoor innerDoor;
IMyLightingBlock passageLight;

void Setup()
{
    airVent = GridTerminalSystem.GetBlockWithName("Te-toku Air Lock Vent") as IMyAirVent;
    if (airVent == null) {
        Write("Vent Not Found :(");
        return;
    }

    midDoor = GridTerminalSystem.GetBlockWithName("Te-toku Mid Door") as IMyDoor;
    if (midDoor == null) {
        Write("Mid Door not Found :(");
        return;
    }

    innerDoor = GridTerminalSystem.GetBlockWithName("Te-toku Inner Door") as IMyDoor;
    if (innerDoor == null) {
        Write("Inner Door not Found :(");
        return;
    }

    passageLight = GridTerminalSystem.GetBlockWithName("Te-toku Air Lock Light") as IMyLightingBlock;
    if (passageLight == null) {
        Write("Light not found :(");
        return;
    }

    Write("Setup OK :)");
}

void OnEnteredMidSensor()
{

}

void OnLeavedMidSensor()
{
    midDoor.ApplyAction("Open_Off");
}

void OnEnteredInnerSensor()
{

}

void OnLeavedInnerSensor()
{
    innerDoor.ApplyAction("Open_Off");
}

void Debug_GetActions()
{
    var text = "";
    var actionList = new List<ITerminalAction>();
    innerDoor.GetActions(actionList);
    for (var i = 0; i < actionList.Count; i++) {
        text += actionList[i].Id + endl;
    }
    Write(text);
}

void Write(string text)
{
    Echo(text);
}

string Format(VRage.MyFixedPoint value)
{
    return string.Format("{0:f4}", (double)(value));
}

string Format(double value)
{
    return string.Format("{0:f4}", value);
}

