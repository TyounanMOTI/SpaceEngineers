void Main(string argument)
{
    string text = "Te-toku Hentai!" + endl + endl;

    text += GetRefineryUtilization();
    text += GetReactorVolume();

    Write(text);
}

const string endl = "\r\n";

void Write(string text)
{
    IMyTextPanel textPanel = GridTerminalSystem.GetBlockWithName("Te-toku Display") as IMyTextPanel;
    if (textPanel != null) {
        textPanel.WritePublicText(text);
    }
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

string GetRefineryUtilization()
{
    IMyRefinery refinery = GridTerminalSystem.GetBlockWithName("BASE Refinery 01") as IMyRefinery;
    if (refinery == null) {
        return "Refinery Not Found :(";
    }
    IMyInventory inventory = TerminalBlockExtentions.GetInventory(refinery, 0);
    if (inventory == null) {
        return "Refinery Not Found :(";
    }

    var percent = (double)(inventory.CurrentVolume) / (double)(inventory.MaxVolume);

    return "Refinery Ore: " + Format(inventory.CurrentVolume) + " L / " + Format(inventory.MaxVolume) + " L "
            + "(" + Format(percent) + "%)" + endl;
}

// Store fuel mass to Strage
double CalcFuelConsumeSpeed(VRage.MyFixedPoint currentFuelMass)
{
    var previousFuelMass = VRage.MyFixedPoint.DeserializeString(Storage);
    var consumeSpeed = (double)(previousFuelMass - currentFuelMass) / ElapsedTime.TotalHours;
    Storage = currentFuelMass.SerializeString();
    if (consumeSpeed < 0) {
        // fuel supplied
        return 0.0;
    } else {
        // fuel consumed
        return consumeSpeed;
    }
}

string GetReactorVolume()
{
    var reactors = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyReactor>(reactors);

    var fuel = new VRage.MyFixedPoint();
    for (int i = 0; i < reactors.Count; i++) {
        var inventory = TerminalBlockExtentions.GetInventory(reactors[i], 0);
        fuel += inventory.CurrentMass;
    }
    
    var consumeSpeed = CalcFuelConsumeSpeed(fuel);
    
    return "Reactor Remain: " + Format(fuel) + " kg" + endl
            + "Fuel Consume: " + Format(consumeSpeed) + " kg/h" + endl;
}

