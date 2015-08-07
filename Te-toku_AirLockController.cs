const string endl = "\r\n";

IMyAirVent airVent;
IMyDoor midDoor;
IMyDoor innerDoor;
IMyLightingBlock passageLight;
IMyTimerBlock timer;

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
        case "Check Air Vent Job":
            CheckAirVentJob();
            break;
        case "":
            Debug_GetActions(timer);
            break;
        default:
            Write("Invalid sensor event :(");
            return;
    }
}

void Setup()
{
    airVent = GridTerminalSystem.GetBlockWithName("Te-toku Air Lock Vent") as IMyAirVent;
    if (airVent == null) {
        Write("Air Vent Not Found :(");
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
    
    timer = GridTerminalSystem.GetBlockWithName("Te-toku Air Lock Timer") as IMyTimerBlock;
    if (timer == null) {
        Write("Timer not found :(");
        return;
    }

    Write("Setup OK :)");
}

void OnEnteredMidSensor()
{
    airVent.ApplyAction("Depressurize_On");
    passageLight.ApplyAction("OnOff_Off");
    WaitForAirVentJob();
}

void OnLeavedMidSensor()
{
    midDoor.ApplyAction("Open_Off");
    StopPollingAirVentJob();
}

void OnEnteredInnerSensor()
{
    airVent.ApplyAction("Depressurize_Off");
    passageLight.ApplyAction("OnOff_On");
    WaitForAirVentJob();
}

void OnLeavedInnerSensor()
{
    innerDoor.ApplyAction("Open_Off");
    StopPollingAirVentJob();
}

void WaitForAirVentJob()
{
    timer.ApplyAction("Start");
}

void StopPollingAirVentJob()
{
    timer.ApplyAction("Stop");
}

void CheckAirVentJob()
{
    if (innerDoor.OpenRatio > 0 || midDoor.OpenRatio > 0) {
        // Do nothing when either door is not completely closed
        Write("Waiting for Doors are completely closed.");
        return;
    }
    
    if ((!airVent.IsDepressurizing && airVent.GetOxygenLevel() < 1.0f)
        || (airVent.IsDepressurizing && airVent.GetOxygenLevel() > 0.3f)) {
        // Do nothing while Depressure/Pressure-ing
        Write("Waiting for AirVent. Oxygen Level: " + Format(airVent.GetOxygenLevel()));
        return;
    }
    
    // Depressure/Pressure completed! Let's open the door.
    if (airVent.IsDepressurizing) {
        // Open no-oxygen door when passage is no-oxygen.
        midDoor.ApplyAction("Open_On");
    } else {
        // Open full-oxygen door when passage is full-oxygen.
        innerDoor.ApplyAction("Open_On");
    }
    
    Write("Last Oxygen Level: " + Format(airVent.GetOxygenLevel()));

    // AirVent job completed.
    StopPollingAirVentJob();
}

void Debug_GetActions(IMyTerminalBlock target)
{
    var text = "";
    var actionList = new List<ITerminalAction>();
    target.GetActions(actionList);
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

