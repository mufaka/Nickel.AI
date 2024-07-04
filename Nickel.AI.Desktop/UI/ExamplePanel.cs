using Microsoft.Data.Analysis;
using Nickel.AI.Desktop.UI.Controls;
using Nickel.AI.Desktop.UI.Modals;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
    private DataFrameTable DataFrameTable { get; set; }
    private ChooseFileDialog ChooseFileDialog { get; set; }
    private FileChooser _fileChooser;

    public ExamplePanel(string menu = "Examples", string label = "Label")
    {
        DefaultWindowSize.X = 600;
        DefaultWindowSize.Y = 400;
        MenuCategory = menu;
        Label = label;

        _fileChooser = new FileChooser();
        ChooseFileDialog = new ChooseFileDialog();
        DataFrameTable = new DataFrameTable();
        DataFrameTable.Frame = DataFrame.LoadCsv(@"D:\DataSets\Spotify\Most Streamed Spotify Songs 2024.csv");
    }

    // Interactive ImGui manual: https://pthom.github.io/imgui_manual_online/manual/imgui_manual.html

    public override void DoRender()
    {
        ChooseFileDialog.ShowDialogButton("Choose...", "Choose File or Directory");

        /*
        if (ChooseFileDialog.SelectedPath != String.Empty)
        {
            ImGui.Text(ChooseFileDialog.SelectedPath);
        }
        */
        //DataFrameTable.Render();
    }

    public override void Update()
    {
    }
}