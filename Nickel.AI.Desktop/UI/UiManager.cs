using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using System.Numerics;
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
        SetStyleDefault();
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

    private static void DoMainMenu()
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

            if (ImGui.BeginMenu("Theme"))
            {
                if (ImGui.MenuItem("Default"))
                    SetStyleDefault();

                if (ImGui.MenuItem("Blender"))
                    SetStyleBlender();

                if (ImGui.MenuItem("Darcula"))
                    SetStyleDarcula();

                if (ImGui.MenuItem("Nord"))
                    SetStyleNord();

                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
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
        DoMainMenu();

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

    public static void SetStyleDefault()
    {
        // Style modified from https://github.com/ocornut/imgui/issues/707#issuecomment-917151020
        var style = ImGui.GetStyle();
        style.CellPadding = new Vector2(4.00f, 3.00f);
        style.WindowRounding = 3;
        style.ChildRounding = 3;
        style.FrameRounding = 3;
        style.PopupRounding = 3;
        style.GrabRounding = 3;

        var colors = style.Colors;
        colors[(int)ImGuiCol.Text] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
        colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.11f, 0.11f, 0.11f, 1.00f);
        colors[(int)ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
        colors[(int)ImGuiCol.PopupBg] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);
        colors[(int)ImGuiCol.Border] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);
        colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.24f);
        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.71f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.10f, 0.10f, 0.10f, 1.00f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.00f, 0.00f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.05f, 0.05f, 0.05f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.34f, 0.34f, 0.34f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.40f, 0.40f, 0.40f, 0.54f);
        colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.56f, 0.56f, 0.56f, 0.54f);
        colors[(int)ImGuiCol.CheckMark] = new Vector4(0.86f, 0.61f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.86f, 0.61f, 0.33f, 0.54f);
        colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.86f, 0.61f, 0.33f, 0.92f);
        colors[(int)ImGuiCol.Button] = new Vector4(0.05f, 0.05f, 0.05f, 0.71f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.Header] = new Vector4(0.00f, 0.00f, 0.00f, 0.71f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.19f, 0.19f, 0.19f, 0.54f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.Separator] = new Vector4(0.19f, 0.19f, 0.19f, 0.92f);
        colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.92f);
        colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);
        colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
        colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.44f, 0.44f, 0.44f, 0.29f);
        colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.40f, 0.44f, 0.47f, 1.00f);
        colors[(int)ImGuiCol.Tab] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        colors[(int)ImGuiCol.TabHovered] = new Vector4(0.86f, 0.61f, 0.33f, 0.54f);
        colors[(int)ImGuiCol.TabActive] = new Vector4(0.86f, 0.61f, 0.33f, 0.36f);
        colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.86f, 0.61f, 0.33f, 0.54f);
        colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        colors[(int)ImGuiCol.PlotLines] = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
        colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
        colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.00f, 0.00f, 0.00f, 0.52f);
        colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.28f, 0.28f, 0.28f, 0.29f);
        colors[(int)ImGuiCol.TableRowBg] = new Vector4(1.00f, 1.00f, 1.00f, 0.06f);
        colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.00f, 1.00f, 1.00f, 0.12f);
        colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.20f, 0.22f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.86f, 0.61f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.86f, 0.61f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.86f, 0.61f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.51f, 0.55f, 0.60f, 0.20f);
        colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.51f, 0.55f, 0.60f, 0.35f);
    }

    // Below themes adapted from https://github.com/Raais/ImguiCandy/blob/main/ImCandy/candy.h

    public static void SetStyleNord()
    {
        ImGui.StyleColorsDark();

        var style = ImGui.GetStyle();
        var colors = style.Colors;

        style.WindowBorderSize = 1.00f;
        style.ChildBorderSize = 1.00f;
        style.PopupBorderSize = 1.00f;
        style.FrameBorderSize = 1.00f;

        colors[(int)ImGuiCol.Text] = new Vector4(0.85f, 0.87f, 0.91f, 0.88f);
        colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.49f, 0.50f, 0.53f, 1.00f);
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.18f, 0.20f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.ChildBg] = new Vector4(0.16f, 0.17f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.PopupBg] = new Vector4(0.23f, 0.26f, 0.32f, 1.00f);
        colors[(int)ImGuiCol.Border] = new Vector4(0.14f, 0.16f, 0.19f, 1.00f);
        colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.09f, 0.09f, 0.09f, 0.00f);
        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.23f, 0.26f, 0.32f, 1.00f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.56f, 0.74f, 0.73f, 1.00f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.53f, 0.75f, 0.82f, 1.00f);
        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.16f, 0.16f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.16f, 0.16f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.16f, 0.16f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.16f, 0.16f, 0.20f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.18f, 0.20f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.23f, 0.26f, 0.32f, 0.60f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.23f, 0.26f, 0.32f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.23f, 0.26f, 0.32f, 1.00f);
        colors[(int)ImGuiCol.CheckMark] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.51f, 0.63f, 0.76f, 1.00f);
        colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.Button] = new Vector4(0.18f, 0.20f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.51f, 0.63f, 0.76f, 1.00f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.Header] = new Vector4(0.51f, 0.63f, 0.76f, 1.00f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.53f, 0.75f, 0.82f, 1.00f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.56f, 0.74f, 0.73f, 1.00f);
        colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.53f, 0.75f, 0.82f, 1.00f);
        colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.53f, 0.75f, 0.82f, 0.86f);
        colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.61f, 0.74f, 0.87f, 1.00f);
        colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.Tab] = new Vector4(0.18f, 0.20f, 0.25f, 1.00f);
        colors[(int)ImGuiCol.TabHovered] = new Vector4(0.22f, 0.24f, 0.31f, 1.00f);
        colors[(int)ImGuiCol.TabActive] = new Vector4(0.23f, 0.26f, 0.32f, 1.00f);
        colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.13f, 0.15f, 0.18f, 1.00f);
        colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.17f, 0.19f, 0.23f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.56f, 0.74f, 0.73f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.53f, 0.75f, 0.82f, 1.00f);
        colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.37f, 0.51f, 0.67f, 1.00f);
        colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.53f, 0.75f, 0.82f, 0.86f);
    }

    public static void SetStyleBlender()
    {
        ImGui.StyleColorsDark();

        var style = ImGui.GetStyle();
        var colors = style.Colors;

        style.WindowPadding = new Vector2(12.00f, 8.00f);
        style.ItemSpacing = new Vector2(7.00f, 3.00f);
        style.GrabMinSize = 20.00f;
        style.WindowRounding = 8.00f;
        style.FrameBorderSize = 0.00f;
        style.FrameRounding = 4.00f;
        style.GrabRounding = 12.00f;

        colors[(int)ImGuiCol.Text] = new Vector4(0.84f, 0.84f, 0.84f, 1.00f);
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.22f, 0.22f, 0.22f, 1.00f);
        colors[(int)ImGuiCol.ChildBg] = new Vector4(0.19f, 0.19f, 0.19f, 1.00f);
        colors[(int)ImGuiCol.PopupBg] = new Vector4(0.09f, 0.09f, 0.09f, 1.00f);
        colors[(int)ImGuiCol.Border] = new Vector4(0.17f, 0.17f, 0.17f, 1.00f);
        colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.10f, 0.10f, 0.10f, 0.00f);
        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.47f, 0.47f, 0.47f, 1.00f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.16f, 0.16f, 0.16f, 1.00f);
        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.11f, 0.11f, 0.11f, 1.00f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.11f, 0.11f, 0.11f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.35f, 0.35f, 0.35f, 1.00f);
        colors[(int)ImGuiCol.CheckMark] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.Button] = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.40f, 0.40f, 0.40f, 1.00f);
        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.Header] = new Vector4(0.27f, 0.27f, 0.27f, 1.00f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.27f, 0.27f, 0.27f, 1.00f);
        colors[(int)ImGuiCol.Separator] = new Vector4(0.18f, 0.18f, 0.18f, 1.00f);
        colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.54f, 0.54f, 0.54f, 1.00f);
        colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.19f, 0.39f, 0.69f, 1.00f);
        colors[(int)ImGuiCol.Tab] = new Vector4(0.11f, 0.11f, 0.11f, 1.00f);
        colors[(int)ImGuiCol.TabHovered] = new Vector4(0.14f, 0.14f, 0.14f, 1.00f);
        colors[(int)ImGuiCol.TabActive] = new Vector4(0.19f, 0.19f, 0.19f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.20f, 0.39f, 0.69f, 1.00f);
        colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
        colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
    }

    public static void SetStyleDarcula()
    {
        // Darcula styleice1000 from ImThemes
        var style = ImGuiNET.ImGui.GetStyle();

        style.Alpha = 1.0f;
        style.DisabledAlpha = 0.6000000238418579f;
        style.WindowPadding = new Vector2(8.0f, 8.0f);
        style.WindowRounding = 5.300000190734863f;
        style.WindowBorderSize = 1.0f;
        style.WindowMinSize = new Vector2(32.0f, 32.0f);
        style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
        style.WindowMenuButtonPosition = ImGuiDir.Left;
        style.ChildRounding = 0.0f;
        style.ChildBorderSize = 1.0f;
        style.PopupRounding = 0.0f;
        style.PopupBorderSize = 1.0f;
        style.FramePadding = new Vector2(4.0f, 3.0f);
        style.FrameRounding = 2.299999952316284f;
        style.FrameBorderSize = 1.0f;
        style.ItemSpacing = new Vector2(8.0f, 6.5f);
        style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
        style.CellPadding = new Vector2(4.0f, 2.0f);
        style.IndentSpacing = 21.0f;
        style.ColumnsMinSpacing = 6.0f;
        style.ScrollbarSize = 14.0f;
        style.ScrollbarRounding = 5.0f;
        style.GrabMinSize = 10.0f;
        style.GrabRounding = 2.299999952316284f;
        style.TabRounding = 4.0f;
        style.TabBorderSize = 0.0f;
        style.TabMinWidthForCloseButton = 0.0f;
        style.ColorButtonPosition = ImGuiDir.Right;
        style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
        style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

        style.Colors[(int)ImGuiCol.Text] = new Vector4(0.7333333492279053f, 0.7333333492279053f, 0.7333333492279053f, 1.0f);
        style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.3450980484485626f, 0.3450980484485626f, 0.3450980484485626f, 1.0f);
        style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.2352941185235977f, 0.2470588237047195f, 0.2549019753932953f, 0.9399999976158142f);
        style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.2352941185235977f, 0.2470588237047195f, 0.2549019753932953f, 0.0f);
        style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.2352941185235977f, 0.2470588237047195f, 0.2549019753932953f, 0.9399999976158142f);
        style.Colors[(int)ImGuiCol.Border] = new Vector4(0.3333333432674408f, 0.3333333432674408f, 0.3333333432674408f, 0.5f);
        style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.1568627506494522f, 0.1568627506494522f, 0.1568627506494522f, 0.0f);
        style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.168627455830574f, 0.168627455830574f, 0.168627455830574f, 0.5400000214576721f);
        style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.4509803950786591f, 0.6745098233222961f, 0.9960784316062927f, 0.6700000166893005f);
        style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.4705882370471954f, 0.4705882370471954f, 0.4705882370471954f, 0.6700000166893005f);
        style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.03921568766236305f, 0.03921568766236305f, 0.03921568766236305f, 1.0f);
        style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0f, 0.0f, 0.0f, 0.5099999904632568f);
        style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.1568627506494522f, 0.2862745225429535f, 0.47843137383461f, 1.0f);
        style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.2705882489681244f, 0.2862745225429535f, 0.2901960909366608f, 0.800000011920929f);
        style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.2705882489681244f, 0.2862745225429535f, 0.2901960909366608f, 0.6000000238418579f);
        style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.2196078449487686f, 0.3098039329051971f, 0.4196078479290009f, 0.5099999904632568f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.2196078449487686f, 0.3098039329051971f, 0.4196078479290009f, 1.0f);
        style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.1372549086809158f, 0.1921568661928177f, 0.2627451121807098f, 0.9100000262260437f);
        style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.8980392217636108f, 0.8980392217636108f, 0.8980392217636108f, 0.8299999833106995f);
        style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.6980392336845398f, 0.6980392336845398f, 0.6980392336845398f, 0.6200000047683716f);
        style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.2980392277240753f, 0.2980392277240753f, 0.2980392277240753f, 0.8399999737739563f);
        style.Colors[(int)ImGuiCol.Button] = new Vector4(0.3333333432674408f, 0.3529411852359772f, 0.3607843220233917f, 0.4900000095367432f);
        style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.2196078449487686f, 0.3098039329051971f, 0.4196078479290009f, 1.0f);
        style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.1372549086809158f, 0.1921568661928177f, 0.2627451121807098f, 1.0f);
        style.Colors[(int)ImGuiCol.Header] = new Vector4(0.3333333432674408f, 0.3529411852359772f, 0.3607843220233917f, 0.5299999713897705f);
        style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.4509803950786591f, 0.6745098233222961f, 0.9960784316062927f, 0.6700000166893005f);
        style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.4705882370471954f, 0.4705882370471954f, 0.4705882370471954f, 0.6700000166893005f);
        style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.3137255012989044f, 0.3137255012989044f, 0.3137255012989044f, 1.0f);
        style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.3137255012989044f, 0.3137255012989044f, 0.3137255012989044f, 1.0f);
        style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.3137255012989044f, 0.3137255012989044f, 0.3137255012989044f, 1.0f);
        style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(1.0f, 1.0f, 1.0f, 0.8500000238418579f);
        style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(1.0f, 1.0f, 1.0f, 0.6000000238418579f);
        style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(1.0f, 1.0f, 1.0f, 0.8999999761581421f);
        style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.1764705926179886f, 0.3490196168422699f, 0.5764706134796143f, 0.8619999885559082f);
        style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
        style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.196078434586525f, 0.407843142747879f, 0.6784313917160034f, 1.0f);
        style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.06666667014360428f, 0.1019607856869698f, 0.1450980454683304f, 0.9724000096321106f);
        style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.1333333402872086f, 0.2588235437870026f, 0.4235294163227081f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.8980392217636108f, 0.6980392336845398f, 0.0f, 1.0f);
        style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.0f, 0.6000000238418579f, 0.0f, 1.0f);
        style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.1882352977991104f, 0.1882352977991104f, 0.2000000029802322f, 1.0f);
        style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.3098039329051971f, 0.3098039329051971f, 0.3490196168422699f, 1.0f);
        style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.2274509817361832f, 0.2274509817361832f, 0.2470588237047195f, 1.0f);
        style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.05999999865889549f);
        style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.1843137294054031f, 0.3960784375667572f, 0.7921568751335144f, 0.8999999761581421f);
        style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.0f, 1.0f, 0.0f, 0.8999999761581421f);
        style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
        style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.699999988079071f);
        style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.2000000029802322f);
        style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.3499999940395355f);
    }
}