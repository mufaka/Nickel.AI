using ImGuiNET;
using rlImGui_cs;

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
    }

    public static ImFontPtr FONT_JETBRAINS_MONO_MEDIUM_20;
    public static ImFontPtr FONT_JETBRAINS_MONO_MEDIUM_16;

    private static void SetFont()
    {
        var io = ImGui.GetIO();
        io.Fonts.Clear();
        FONT_JETBRAINS_MONO_MEDIUM_20 = io.Fonts.AddFontFromFileTTF(Path.Combine("Resources", "JetBrainsMono-Medium.ttf"), 20);
        FONT_JETBRAINS_MONO_MEDIUM_16 = io.Fonts.AddFontFromFileTTF(Path.Combine("Resources", "JetBrainsMono-Medium.ttf"), 16);
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
}