using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using System.Runtime.InteropServices;

namespace Nickel.AI.Desktop.UI;

public static class UiManager
{
    public static List<Panel> Panels { get; set; } = new();
    public static bool Quit = false;

    public static void Setup()
    {
        rlImGui.Setup(true);

        // enable docking.
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        // TODO: Persist theme choice.
        Themes.SetStyleMoonlight();
        SetFont();
        SetupClipboard();

    }

    private static void SetFont()
    {
        var io = ImGui.GetIO();
        io.Fonts.Clear();
        io.Fonts.AddFontFromFileTTF(Path.Combine("Resources", "JetBrainsMono-Medium.ttf"), 22);
        //io.Fonts.AddFontFromFileTTF(Path.Combine("Resources", "IckyticketMono-nKpJ.ttf"), 24);
        rlImGui.ReloadFonts();
    }

    private static void DrawMainMenu()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Exit"))
                    Quit = true;

                ImGui.EndMenu();
            }

            if (Panels.Count > 0)
            {
                // get distinct menu values from panels
                var menus = Panels.Select(x => x.MenuCategory).Distinct().Order().ToList();

                foreach (var menu in menus)
                {
                    if (ImGui.BeginMenu(menu))
                    {
                        // get all panels for this menu
                        var menuItems = Panels.Where(x => x.MenuCategory == menu).ToList();

                        foreach (var menuItem in menuItems)
                        {
                            ImGui.MenuItem(menuItem.Label, string.Empty, ref menuItem.Open);
                        }

                        ImGui.EndMenu();
                    }

                }
            }

            DrawThemeMenu();

            ImGui.EndMainMenuBar();
        }
    }

    private static void DrawThemeMenu()
    {
        if (ImGui.BeginMenu("Theme"))
        {
            if (ImGui.MenuItem("Default"))
                Themes.SetStyleDefault();

            if (ImGui.MenuItem("Blender"))
                Themes.SetStyleBlender();

            if (ImGui.MenuItem("Darcula"))
                Themes.SetStyleDarcula();

            if (ImGui.MenuItem("Dark Ruda"))
                Themes.SetStyleDarkRuda();

            if (ImGui.MenuItem("Light"))
                Themes.SetStyleLight();

            if (ImGui.MenuItem("Material Flat"))
                Themes.SetStyleMaterialFlat();

            if (ImGui.MenuItem("Moonlight"))
                Themes.SetStyleMoonlight();

            if (ImGui.MenuItem("Nord"))
                Themes.SetStyleNord();

            ImGui.EndMenu();
        }
    }

    public static void Shutdown()
    {
        foreach (var panel in Panels)
            panel.Detach();
        rlImGui.Shutdown();
    }

    public static void Update()
    {
        foreach (var panel in Panels)
            panel.Update();
    }

    public static void Render()
    {
        rlImGui.Begin();
        DrawMainMenu();

        ImGui.DockSpaceOverViewport(ImGui.GetID("NickelAI"), ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);

        foreach (var panel in Panels)
            panel.Render();

        //ImGui.ShowDemoWindow();

        rlImGui.End();
    }

    // NOTE: The following code is copied from rlImgui-cs with the exception
    //       of moving getClip and setClip to static variables to prevent
    //       the delegates from getting garbage collected and causing
    //       a hard crash with System.ExecutionEngineException.
    unsafe internal static sbyte* rImGuiGetClipText(IntPtr userData)
    {
        return Raylib.GetClipboardText();
    }

    unsafe internal static void rlImGuiSetClipText(IntPtr userData, sbyte* text)
    {
        Raylib.SetClipboardText(text);
    }

    private unsafe delegate sbyte* GetClipTextCallback(IntPtr userData);
    private unsafe delegate void SetClipTextCallback(IntPtr userData, sbyte* text);

    private unsafe static GetClipTextCallback getClip = new GetClipTextCallback(rImGuiGetClipText);
    private unsafe static SetClipTextCallback setClip = new SetClipTextCallback(rlImGuiSetClipText);

    private static void SetupClipboard()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        unsafe
        {
            io.SetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(setClip);
            io.GetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(getClip);
        }
    }
    // NOTE: End rlImgui-cs code copy/paste/modify.

}