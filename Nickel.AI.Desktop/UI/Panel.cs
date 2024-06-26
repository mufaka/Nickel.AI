namespace Nickel.AI.Desktop.UI;

public abstract class Panel
{
    public bool Open = false;

    public string Menu { get; set; } = "Menu";
    public string Label { get; set; } = "Label";

    public abstract void Attach();
    public abstract void Detach();
    public abstract void Render();
    public abstract void Update();
}