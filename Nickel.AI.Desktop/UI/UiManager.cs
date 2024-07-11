using ImGuiNET;
using Nickel.AI.Desktop.Models;
using Nickel.AI.Desktop.Settings;
using rlImGui_cs;

namespace Nickel.AI.Desktop.UI;

public static class UiManager
{
    public static List<Panel> Panels { get; set; } = new();
    public static bool Quit = false;
    public static ApplicationSettings ApplicationSettings = SettingsManager.ApplicationSettings;

    public static void Setup()
    {
        // Dark mode and docking enabled.
        rlImGui.Setup(true, true);
        SetTheme(ApplicationSettings.Theme);
        SetFonts();

        // 
        foreach (var panel in Panels)
        {
            panel.Setup();
        }
    }

    public static ImFontPtr FONT_JETBRAINS_MONO_MEDIUM_20;
    public static ImFontPtr FONT_JETBRAINS_MONO_MEDIUM_16;

    private static void SetFonts()
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
                SetTheme("Default");

            if (ImGui.MenuItem("Blender"))
                SetTheme("Blender");

            if (ImGui.MenuItem("Darcula"))
                SetTheme("Darcula");

            if (ImGui.MenuItem("Dark Ruda"))
                SetTheme("Dark Ruda");

            if (ImGui.MenuItem("Light"))
                SetTheme("Light");

            if (ImGui.MenuItem("Material Flat"))
                SetTheme("Material Flat");

            if (ImGui.MenuItem("Moonlight"))
                SetTheme("Moonlight");

            if (ImGui.MenuItem("Nord"))
                SetTheme("Nord");

            ImGui.EndMenu();
        }
    }

    private static void SetTheme(string theme)
    {
        ApplicationSettings.Theme = theme;
        SettingsManager.ApplicationSettings = ApplicationSettings;

        switch (theme)
        {
            case "Default":
                Themes.SetStyleDefault();
                break;
            case "Blender":
                Themes.SetStyleBlender();
                break;
            case "Darcula":
                Themes.SetStyleDarcula();
                break;
            case "Dark Ruda":
                Themes.SetStyleDarkRuda();
                break;
            case "Light":
                Themes.SetStyleLight();
                break;
            case "Material Flat":
                Themes.SetStyleMaterialFlat();
                break;
            case "Moonlight":
                Themes.SetStyleMoonlight();
                break;
            case "Nord":
                Themes.SetStyleNord();
                break;
            default:
                Themes.SetStyleMoonlight();
                break;
        }
    }

    public static void Shutdown()
    {
        foreach (var panel in Panels)
            panel.Detach();
        rlImGui.Shutdown();
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