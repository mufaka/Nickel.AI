using ImGuiNET;
using System.Numerics;

namespace Nickel.AI.Desktop.UI
{
    internal static class Themes
    {
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

        public static void SetStyleDarkRuda()
        {
            // Dark Ruda styleRaikiri from ImThemes
            var style = ImGuiNET.ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.6000000238418579f;
            style.WindowPadding = new Vector2(8.0f, 8.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 1.0f;
            style.WindowMinSize = new Vector2(32.0f, 32.0f);
            style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Left;
            style.ChildRounding = 0.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 0.0f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(4.0f, 3.0f);
            style.FrameRounding = 4.0f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(8.0f, 4.0f);
            style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
            style.CellPadding = new Vector2(4.0f, 2.0f);
            style.IndentSpacing = 21.0f;
            style.ColumnsMinSpacing = 6.0f;
            style.ScrollbarSize = 14.0f;
            style.ScrollbarRounding = 9.0f;
            style.GrabMinSize = 10.0f;
            style.GrabRounding = 4.0f;
            style.TabRounding = 4.0f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.9490196108818054f, 0.95686274766922f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.3568627536296844f, 0.4196078479290009f, 0.4666666686534882f, 1.0f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.1098039224743843f, 0.1490196138620377f, 0.168627455830574f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.1490196138620377f, 0.1764705926179886f, 0.2196078449487686f, 1.0f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.0784313753247261f, 0.0784313753247261f, 0.0784313753247261f, 0.9399999976158142f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.0784313753247261f, 0.09803921729326248f, 0.1176470592617989f, 1.0f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.1176470592617989f, 0.2000000029802322f, 0.2784313857555389f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.08627451211214066f, 0.1176470592617989f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.08627451211214066f, 0.1176470592617989f, 0.1372549086809158f, 0.6499999761581421f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0784313753247261f, 0.09803921729326248f, 0.1176470592617989f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.0f, 0.0f, 0.0f, 0.5099999904632568f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.1490196138620377f, 0.1764705926179886f, 0.2196078449487686f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.01960784383118153f, 0.01960784383118153f, 0.01960784383118153f, 0.3899999856948853f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.1764705926179886f, 0.2196078449487686f, 0.2470588237047195f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.08627451211214066f, 0.2078431397676468f, 0.3098039329051971f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.2784313857555389f, 0.5568627715110779f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.2784313857555389f, 0.5568627715110779f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.3686274588108063f, 0.6078431606292725f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.2784313857555389f, 0.5568627715110779f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.05882352963089943f, 0.529411792755127f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 0.550000011920929f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 1.0f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 0.7799999713897705f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.25f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.6700000166893005f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.949999988079071f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.1098039224743843f, 0.1490196138620377f, 0.168627455830574f, 1.0f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.2000000029802322f, 0.2470588237047195f, 0.2862745225429535f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.1098039224743843f, 0.1490196138620377f, 0.168627455830574f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.1098039224743843f, 0.1490196138620377f, 0.168627455830574f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.8980392217636108f, 0.6980392336845398f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.0f, 0.6000000238418579f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.1882352977991104f, 0.1882352977991104f, 0.2000000029802322f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.3098039329051971f, 0.3098039329051971f, 0.3490196168422699f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.2274509817361832f, 0.2274509817361832f, 0.2470588237047195f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.05999999865889549f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.3499999940395355f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.0f, 1.0f, 0.0f, 0.8999999761581421f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(1.0f, 1.0f, 1.0f, 0.699999988079071f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.2000000029802322f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.800000011920929f, 0.800000011920929f, 0.800000011920929f, 0.3499999940395355f);
        }

        public static void SetStyleLight()
        {
            // Light styledougbinks from ImThemes
            var style = ImGuiNET.ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.6000000238418579f;
            style.WindowPadding = new Vector2(8.0f, 8.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 1.0f;
            style.WindowMinSize = new Vector2(32.0f, 32.0f);
            style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Left;
            style.ChildRounding = 0.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 0.0f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(4.0f, 3.0f);
            style.FrameRounding = 0.0f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(8.0f, 4.0f);
            style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
            style.CellPadding = new Vector2(4.0f, 2.0f);
            style.IndentSpacing = 21.0f;
            style.ColumnsMinSpacing = 6.0f;
            style.ScrollbarSize = 14.0f;
            style.ScrollbarRounding = 9.0f;
            style.GrabMinSize = 10.0f;
            style.GrabRounding = 0.0f;
            style.TabRounding = 4.0f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.6000000238418579f, 0.6000000238418579f, 0.6000000238418579f, 1.0f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.9372549057006836f, 0.9372549057006836f, 0.9372549057006836f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(1.0f, 1.0f, 1.0f, 0.9800000190734863f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.0f, 0.0f, 0.0f, 0.300000011920929f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.4000000059604645f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.6700000166893005f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.95686274766922f, 0.95686274766922f, 0.95686274766922f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.8196078538894653f, 0.8196078538894653f, 0.8196078538894653f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(1.0f, 1.0f, 1.0f, 0.5099999904632568f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.8588235378265381f, 0.8588235378265381f, 0.8588235378265381f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.9764705896377563f, 0.9764705896377563f, 0.9764705896377563f, 0.5299999713897705f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.686274528503418f, 0.686274528503418f, 0.686274528503418f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.4862745106220245f, 0.4862745106220245f, 0.4862745106220245f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.4862745106220245f, 0.4862745106220245f, 0.4862745106220245f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.7799999713897705f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.4588235318660736f, 0.5372549295425415f, 0.800000011920929f, 0.6000000238418579f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.4000000059604645f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.05882352963089943f, 0.529411792755127f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.3100000023841858f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.3882353007793427f, 0.3882353007793427f, 0.3882353007793427f, 0.6200000047683716f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.1372549086809158f, 0.4392156898975372f, 0.800000011920929f, 0.7799999713897705f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.1372549086809158f, 0.4392156898975372f, 0.800000011920929f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.3490196168422699f, 0.3490196168422699f, 0.3490196168422699f, 0.1700000017881393f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.6700000166893005f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.949999988079071f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.7607843279838562f, 0.7960784435272217f, 0.8352941274642944f, 0.9309999942779541f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.5921568870544434f, 0.7254902124404907f, 0.8823529481887817f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.9176470637321472f, 0.9254902005195618f, 0.9333333373069763f, 0.9861999750137329f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.7411764860153198f, 0.8196078538894653f, 0.9137254953384399f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.3882353007793427f, 0.3882353007793427f, 0.3882353007793427f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.8980392217636108f, 0.6980392336845398f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.0f, 0.4470588266849518f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.7764706015586853f, 0.8666666746139526f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.5686274766921997f, 0.5686274766921997f, 0.6392157077789307f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.6784313917160034f, 0.6784313917160034f, 0.7372549176216125f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(0.2980392277240753f, 0.2980392277240753f, 0.2980392277240753f, 0.09000000357627869f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.3499999940395355f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.949999988079071f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 0.800000011920929f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.6980392336845398f, 0.6980392336845398f, 0.6980392336845398f, 0.699999988079071f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.2000000029802322f, 0.2000000029802322f, 0.2000000029802322f, 0.2000000029802322f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.2000000029802322f, 0.2000000029802322f, 0.2000000029802322f, 0.3499999940395355f);
        }

        public static void SetStyleMaterialFlat()
        {
            // Material Flat styleImJC1C from ImThemes
            var style = ImGuiNET.ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 0.5f;
            style.WindowPadding = new Vector2(8.0f, 8.0f);
            style.WindowRounding = 0.0f;
            style.WindowBorderSize = 1.0f;
            style.WindowMinSize = new Vector2(32.0f, 32.0f);
            style.WindowTitleAlign = new Vector2(0.0f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Left;
            style.ChildRounding = 0.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 0.0f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(4.0f, 3.0f);
            style.FrameRounding = 0.0f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(8.0f, 4.0f);
            style.ItemInnerSpacing = new Vector2(4.0f, 4.0f);
            style.CellPadding = new Vector2(4.0f, 2.0f);
            style.IndentSpacing = 21.0f;
            style.ColumnsMinSpacing = 6.0f;
            style.ScrollbarSize = 14.0f;
            style.ScrollbarRounding = 0.0f;
            style.GrabMinSize = 10.0f;
            style.GrabRounding = 0.0f;
            style.TabRounding = 0.0f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Left;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(0.8313725590705872f, 0.8470588326454163f, 0.8784313797950745f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.8313725590705872f, 0.8470588326454163f, 0.8784313797950745f, 0.501960813999176f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.1725490242242813f, 0.1921568661928177f, 0.2352941185235977f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.1587982773780823f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.1725490242242813f, 0.1921568661928177f, 0.2352941185235977f, 1.0f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.2039215713739395f, 0.2313725501298904f, 0.2823529541492462f, 1.0f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 0.501960813999176f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 0.250980406999588f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.01960784383118153f, 0.01960784383118153f, 0.01960784383118153f, 0.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.5333333611488342f, 0.5333333611488342f, 0.5333333611488342f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.3333333432674408f, 0.3333333432674408f, 0.3333333432674408f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.6000000238418579f, 0.6000000238418579f, 0.6000000238418579f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.239215686917305f, 0.5215686559677124f, 0.8784313797950745f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9803921580314636f, 1.0f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.1529411822557449f, 0.1725490242242813f, 0.2117647081613541f, 0.501960813999176f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.1529411822557449f, 0.1725490242242813f, 0.2117647081613541f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.1529411822557449f, 0.1725490242242813f, 0.2117647081613541f, 1.0f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 0.250980406999588f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.4274509847164154f, 0.4274509847164154f, 0.4980392158031464f, 0.5f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 0.7799999713897705f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.09803921729326248f, 0.4000000059604645f, 0.7490196228027344f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 0.250980406999588f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.1529411822557449f, 0.1725490242242813f, 0.2117647081613541f, 1.0f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 0.250980406999588f);
            style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.1529411822557449f, 0.1725490242242813f, 0.2117647081613541f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.3098039329051971f, 0.6235294342041016f, 0.9333333373069763f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.6078431606292725f, 0.6078431606292725f, 0.6078431606292725f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(1.0f, 0.4274509847164154f, 0.3490196168422699f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.8980392217636108f, 0.6980392336845398f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(1.0f, 0.6000000238418579f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.2039215713739395f, 0.2313725501298904f, 0.2823529541492462f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.2039215713739395f, 0.2313725501298904f, 0.2823529541492462f, 0.5021458864212036f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(1.0f, 1.0f, 1.0f, 0.03862661123275757f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.2039215713739395f, 0.2313725501298904f, 0.2823529541492462f, 1.0f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(1.0f, 1.0f, 0.0f, 0.8999999761581421f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.2588235437870026f, 0.5882353186607361f, 0.9764705896377563f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.2039215713739395f, 0.2313725501298904f, 0.2823529541492462f, 0.7529411911964417f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 0.7529411911964417f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.105882354080677f, 0.1137254908680916f, 0.1372549086809158f, 0.7529411911964417f);
        }

        public static void SetStyleMoonlight()
        {
            // Moonlight styleMadam-Herta from ImThemes
            var style = ImGuiNET.ImGui.GetStyle();

            style.Alpha = 1.0f;
            style.DisabledAlpha = 1.0f;
            style.WindowPadding = new Vector2(12.0f, 12.0f);
            style.WindowRounding = 11.5f;
            style.WindowBorderSize = 0.0f;
            style.WindowMinSize = new Vector2(20.0f, 20.0f);
            style.WindowTitleAlign = new Vector2(0.5f, 0.5f);
            style.WindowMenuButtonPosition = ImGuiDir.Right;
            style.ChildRounding = 0.0f;
            style.ChildBorderSize = 1.0f;
            style.PopupRounding = 0.0f;
            style.PopupBorderSize = 1.0f;
            style.FramePadding = new Vector2(20.0f, 3.400000095367432f);
            style.FrameRounding = 11.89999961853027f;
            style.FrameBorderSize = 0.0f;
            style.ItemSpacing = new Vector2(4.300000190734863f, 5.5f);
            style.ItemInnerSpacing = new Vector2(7.099999904632568f, 1.799999952316284f);
            //style.CellPadding = new Vector2(12.10000038146973f, 9.199999809265137f);
            style.CellPadding = new Vector2(8.00f, 6.00f);
            style.IndentSpacing = 21.0f;
            style.ColumnsMinSpacing = 4.900000095367432f;
            style.ScrollbarSize = 11.60000038146973f;
            style.ScrollbarRounding = 15.89999961853027f;
            style.GrabMinSize = 3.700000047683716f;
            style.GrabRounding = 20.0f;
            style.TabRounding = 0.0f;
            style.TabBorderSize = 0.0f;
            style.TabMinWidthForCloseButton = 0.0f;
            style.ColorButtonPosition = ImGuiDir.Right;
            style.ButtonTextAlign = new Vector2(0.5f, 0.5f);
            style.SelectableTextAlign = new Vector2(0.0f, 0.0f);

            style.Colors[(int)ImGuiCol.Text] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TextDisabled] = new Vector4(0.2745098173618317f, 0.3176470696926117f, 0.4509803950786591f, 1.0f);
            style.Colors[(int)ImGuiCol.WindowBg] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.ChildBg] = new Vector4(0.09411764889955521f, 0.1019607856869698f, 0.1176470592617989f, 1.0f);
            style.Colors[(int)ImGuiCol.PopupBg] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.Border] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
            style.Colors[(int)ImGuiCol.BorderShadow] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBg] = new Vector4(0.1137254908680916f, 0.125490203499794f, 0.1529411822557449f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
            style.Colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
            style.Colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.09803921729326248f, 0.105882354080677f, 0.1215686276555061f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrab] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.1568627506494522f, 0.168627455830574f, 0.1921568661928177f, 1.0f);
            style.Colors[(int)ImGuiCol.ScrollbarGrabActive] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.CheckMark] = new Vector4(0.9725490212440491f, 1.0f, 0.4980392158031464f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrab] = new Vector4(0.9725490212440491f, 1.0f, 0.4980392158031464f, 1.0f);
            style.Colors[(int)ImGuiCol.SliderGrabActive] = new Vector4(1.0f, 0.7960784435272217f, 0.4980392158031464f, 1.0f);
            style.Colors[(int)ImGuiCol.Button] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.1803921610116959f, 0.1882352977991104f, 0.196078434586525f, 1.0f);
            style.Colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.1529411822557449f, 0.1529411822557449f, 0.1529411822557449f, 1.0f);
            style.Colors[(int)ImGuiCol.Header] = new Vector4(0.1411764770746231f, 0.1647058874368668f, 0.2078431397676468f, 1.0f);
            style.Colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.105882354080677f, 0.105882354080677f, 0.105882354080677f, 1.0f);
            style.Colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.Separator] = new Vector4(0.1294117718935013f, 0.1490196138620377f, 0.1921568661928177f, 1.0f);
            style.Colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.1568627506494522f, 0.1843137294054031f, 0.250980406999588f, 1.0f);
            style.Colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.1568627506494522f, 0.1843137294054031f, 0.250980406999588f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGrip] = new Vector4(0.1450980454683304f, 0.1450980454683304f, 0.1450980454683304f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGripHovered] = new Vector4(0.9725490212440491f, 1.0f, 0.4980392158031464f, 1.0f);
            style.Colors[(int)ImGuiCol.ResizeGripActive] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.Tab] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.TabHovered] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.TabActive] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.0784313753247261f, 0.08627451211214066f, 0.1019607856869698f, 1.0f);
            style.Colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.125490203499794f, 0.2745098173618317f, 0.572549045085907f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLines] = new Vector4(0.5215686559677124f, 0.6000000238418579f, 0.7019608020782471f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotLinesHovered] = new Vector4(0.03921568766236305f, 0.9803921580314636f, 0.9803921580314636f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogram] = new Vector4(0.8823529481887817f, 0.7960784435272217f, 0.5607843399047852f, 1.0f);
            style.Colors[(int)ImGuiCol.PlotHistogramHovered] = new Vector4(0.95686274766922f, 0.95686274766922f, 0.95686274766922f, 1.0f);
            style.Colors[(int)ImGuiCol.TableHeaderBg] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderStrong] = new Vector4(0.0470588244497776f, 0.05490196123719215f, 0.07058823853731155f, 1.0f);
            style.Colors[(int)ImGuiCol.TableBorderLight] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBg] = new Vector4(0.1176470592617989f, 0.1333333402872086f, 0.1490196138620377f, 1.0f);
            style.Colors[(int)ImGuiCol.TableRowBgAlt] = new Vector4(0.09803921729326248f, 0.105882354080677f, 0.1215686276555061f, 1.0f);

            // NOTE: The provided color for this is too light / too similar to font color, making the text unreadable when selected. Using Blender TextSelectedBg instead.
            //style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.9372549057006836f, 0.9372549057006836f, 0.9372549057006836f, 1.0f);
            style.Colors[(int)ImGuiCol.TextSelectedBg] = new Vector4(0.28f, 0.45f, 0.70f, 1.00f);
            style.Colors[(int)ImGuiCol.DragDropTarget] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.NavHighlight] = new Vector4(0.2666666805744171f, 0.2901960909366608f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingHighlight] = new Vector4(0.4980392158031464f, 0.5137255191802979f, 1.0f, 1.0f);
            style.Colors[(int)ImGuiCol.NavWindowingDimBg] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 0.501960813999176f);
            style.Colors[(int)ImGuiCol.ModalWindowDimBg] = new Vector4(0.196078434586525f, 0.1764705926179886f, 0.5450980663299561f, 0.501960813999176f);
        }
    }
}
