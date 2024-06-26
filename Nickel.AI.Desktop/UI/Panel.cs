namespace Nickel.AI.Desktop.UI;

public abstract class Panel
{
    public bool Open { get; set; }

    public abstract void Attach();
    public abstract void Detach();
    public abstract void Render();
    public abstract void Update();
}