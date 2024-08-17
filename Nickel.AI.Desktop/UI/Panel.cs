using Hexa.NET.ImGui;
using System.Numerics;

namespace Nickel.AI.Desktop.UI;

public abstract class Panel
{
    public bool Open = false;
    public bool HasMenuBar = false;
    public Vector2 DefaultWindowSize = new Vector2(600, 400);

    public string MenuCategory { get; set; } = "Menu";
    public string Label { get; set; } = "Label";

    public void Attach()
    {
        Open = true;
    }

    public void Detach()
    {
        Open = false;
        DoDetach();
    }

    public void Render()
    {
        bool isOpen = Open;
        if (!isOpen) return;

        // NOTE: imgui.ini persists changes to sizing and location. This will set for initial load
        //       ONLY if there is no .ini entry for a panel with this name.
        ImGui.SetNextWindowSize(DefaultWindowSize, ImGuiCond.Once); // Once means once per session (app run), FirstUseEver means respect .ini after first use

        ImGuiWindowFlags flags = HasMenuBar ? ImGuiWindowFlags.MenuBar : ImGuiWindowFlags.None;

        if (ImGui.Begin(Label, ref isOpen, flags))
        {
            DoRender();
            ImGui.End();
        }

        if (!isOpen) Detach();
    }

    // called before rendering loop
    public virtual void Setup() { }

    // handle UiMessages
    public abstract void HandleUiMessage(UiMessage message);

    // called for rendering
    public abstract void DoRender();

    // called for suspending panel, eg: pausing background tasks
    public virtual void DoDetach()
    {

    }

}