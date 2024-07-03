using Microsoft.Data.Analysis;
using Nickel.AI.Desktop.UI.Controls;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
    private DataFrameTable DataFrameTable { get; set; }
    private FileChooser _fileChooser;

    public ExamplePanel(string menu = "Examples", string label = "Label")
    {
        DefaultWindowSize.X = 600;
        DefaultWindowSize.Y = 400;
        MenuCategory = menu;
        Label = label;

        _fileChooser = new FileChooser();
        DataFrameTable = new DataFrameTable();
        DataFrameTable.Frame = DataFrame.LoadCsv(@"D:\DataSets\Spotify\Most Streamed Spotify Songs 2024.csv");
    }

    // Interactive ImGui manual: https://pthom.github.io/imgui_manual_online/manual/imgui_manual.html

    public override void DoRender()
    {
        if (_fileChooser != null)
        {
            _fileChooser.Render();
        }

        //DataFrameTable.Render();
    }

    public override void Update()
    {
    }
}