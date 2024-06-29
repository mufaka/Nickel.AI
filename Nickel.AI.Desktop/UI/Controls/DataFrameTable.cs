using ImGuiNET;
using Microsoft.Data.Analysis;

namespace Nickel.AI.Desktop.UI.Controls
{
    public class DataFrameTable : ImGuiControl
    {
        // Given a DataFrame, draw an ImGui table.
        public DataFrame? Frame { get; set; }

        public override void Render()
        {
            if (Frame != null)
            {
                if (ImGui.BeginTable("frameTable", Frame.Columns.Count, ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders))
                {
                    foreach (DataFrameColumn column in Frame.Columns)
                    {
                        ImGui.TableSetupColumn(column.Name);
                    }
                    ImGui.TableHeadersRow();

                    foreach (DataFrameRow row in Frame.Rows)
                    {
                        ImGui.TableNextRow();
                        var valueEnumerator = row.GetEnumerator();
                        int columnIdx = 0;

                        while (valueEnumerator.MoveNext())
                        {
                            ImGui.TableSetColumnIndex(columnIdx);
                            ImGui.TextUnformatted(Convert.ToString(valueEnumerator.Current));
                            columnIdx++;
                        }
                    }

                    ImGui.EndTable();
                }
            }
        }
    }
}
