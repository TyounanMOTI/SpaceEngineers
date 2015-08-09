void Main(string argument)
{
    if (argument == "reset") {
        Storage = "0";
    } else {    
        Storage = "" + (int.Parse(Storage) + 1);
    }
    Write(Storage);
}

void Write(string text)
{
    IMyTextPanel textPanel = GridTerminalSystem.GetBlockWithName("Lab Display") as IMyTextPanel;
    if (textPanel != null) {
        textPanel.WritePublicText(text);
    }
    Echo(text);
}
