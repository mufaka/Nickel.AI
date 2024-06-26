using ImGuiNET;
using System.Numerics;

namespace Nickel.AI.Desktop.UI;

public abstract class Panel
{
    public bool Open = false;
    public Vector2 WindowSize = new Vector2(600, 400);

    public string MenuCategory { get; set; } = "Menu";
    public string Label { get; set; } = "Label";

    public void Attach()
    {
        Open = true;
    }

    public void Detach()
    {
        Open = false;
    }

    public void Render()
    {
        bool isOpen = Open;
        if (!isOpen) return;

        // NOTE: imgui.ini persists changes to sizing and location. This will set for initial load
        //       ONLY if there is no .ini entry for a panel with this name.
        ImGui.SetNextWindowSize(WindowSize, ImGuiCond.Once); // Once means once per session (app run), FirstUseEver means respect .ini after first use

        if (ImGui.Begin(Label, ref isOpen))
        {
            DoRender();
            ImGui.End();
        }
        if (!isOpen) Detach();
    }

    public abstract void DoRender();

    public abstract void Update();
}