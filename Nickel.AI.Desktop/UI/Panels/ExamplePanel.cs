using Hexa.NET.ImGui;
using Microsoft.Data.Analysis;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.External.Mochi;
using Nickel.AI.Desktop.Settings;
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

    public override void HandleUiMessage(UiMessage message)
    {
    }

    public override void DoRender()
    {
        if (ImGui.Button("Add Log"))
        {
            _logger.LogInformation("Button Clicked");
            MessageQueue.Instance.Enqueue(UiMessageConstants.LOG_SHOW_LOG, null);
        }
        ImGui.SameLine();

        if (ImGui.Button("Clear Log"))
        {
            MessageQueue.Instance.Enqueue(UiMessageConstants.LOG_CLEAR_LOG, null);
        }
        ImGui.SameLine();

        if (ImGui.Button("Ask Ollama"))
        {
            MessageQueue.Instance.Enqueue(UiMessageConstants.CHAT_ASK_QUESTION, "What is a good way to pass messages between ImGui windows?");
        }

        if (ImGui.Button("Get Mochi Decks"))
        {
            GetMochiDecks();
        }

    }

    private async void GetMochiDecks()
    {
        try
        {
            var mochiClient = new MochiClient(SettingsManager.ApplicationSettings.Mochi.ApiKey);
            var deckResponse = await mochiClient.GetDeckList();

            foreach (MochiDeck deck in deckResponse.Decks)
            {
                _logger.LogInformation($"Found Mochi Deck: {deck.Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}