using Friflo.EcGui;
using ImGuiNET;

namespace ImGui;

public static class ImGuiUtils
{
    public static void ConfigureIO()
    {
        // --- ImGui integration (begin)
        ImGuiNET.ImGui.CreateContext();
        var io = ImGuiNET.ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;       // Enable navigation with arrow keys
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;        // Enable navigation GamePad controller
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        // io.FontGlobalScale = 3; // Hacky way to increase font size. Fonts are rendered blurry.
            
        io.Fonts.AddFontFromFileTTF(Path.Combine(AppContext.BaseDirectory, "Content", "Inter-Regular.ttf"), 40);
        io.Fonts.Build();
            
        ImGuiNET.ImGui.StyleColorsLight();
        EcGui.Setup.SetDefaultStyles(); // optional
    }
}