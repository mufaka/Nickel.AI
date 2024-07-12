using Microsoft.Data.Analysis;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.UI.Controls;
using Nickel.AI.Desktop.UI.Modals;

namespace Nickel.AI.Desktop.UI.Panels;

public class ExamplePanel : Panel
{
    private DataFrameTable DataFrameTable { get; set; }
    private ChooseFileDialog ChooseFileDialog { get; set; }
    private DataProjectDialog DataProjectDialog { get; set; }
    private readonly ILogger _logger;

    public ExamplePanel(ILogger<ExamplePanel> logger)
    {
        _logger = logger;
        ChooseFileDialog = new ChooseFileDialog();
        DataProjectDialog = new DataProjectDialog();
        DataFrameTable = new DataFrameTable();
        DataFrameTable.Frame = DataFrame.LoadCsv(@"D:\DataSets\Spotify\Most Streamed Spotify Songs 2024.csv");
    }

    // Interactive ImGui manual: https://pthom.github.io/imgui_manual_online/manual/imgui_manual.html

    public override void DoRender()
    {
        //DataFrameTable.Render();
        DataProjectDialog.ShowDialogButton("Create", "Create Data Project");
    }
}