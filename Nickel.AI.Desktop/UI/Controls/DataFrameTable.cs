using ImGuiNET;
using Microsoft.Data.Analysis;

namespace Nickel.AI.Desktop.UI.Controls
{
    public class DataFrameTable : ImGuiControl
    {
        private DataFrame? _dataFrame = null;
        private Dictionary<string, ColumnState> _columnState = new Dictionary<string, ColumnState>();

        // Given a DataFrame, draw an ImGui table.
        public DataFrame? Frame
        {
            get { return _dataFrame; }
            set
            {
                _dataFrame = value;
                SetupColumnOptions();
            }
        }

        private void SetupColumnOptions()
        {
            if (_dataFrame != null)
            {
                foreach (DataFrameColumn column in _dataFrame.Columns)
                {
                    if (!_columnState.ContainsKey(column.Name))
                    {
                        _columnState.Add(column.Name, new ColumnState() { Visible = true });
                    }
                }
            }
        }

        // NOTE: Need to have a reference type for Checkbox
        private class ColumnState
        {
            public bool Visible = false;
        }

        public override void Render()
        {
            if (_dataFrame != null)
            {
                // column options
                if (ImGui.BeginTable("columnSelectTable", 6, ImGuiTableFlags.None))
                {
                    foreach (DataFrameColumn column in _dataFrame.Columns)
                    {
                        ImGui.TableNextColumn();
                        ImGui.Checkbox(column.Name, ref _columnState[column.Name].Visible);
                    }

                    ImGui.EndTable();
                }

                // data
                if (ImGui.BeginTable("frameTable", _dataFrame.Columns.Count, ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY))
                {
                    for (int i = 0; i < _dataFrame.Columns.Count; i++)
                    {
                        var column = _dataFrame.Columns[i];

                        if (_columnState.ContainsKey(column.Name))
                        {
                            if (_columnState[column.Name].Visible)
                            {
                                ImGui.TableSetupColumn(column.Name);
                            }
                            ImGui.TableSetColumnEnabled(i, _columnState[column.Name].Visible);
                        }
                    }

                    ImGui.TableHeadersRow();

                    // TODO: Need clipper here for larger lists as the performance tanks quickly
                    //       because of the drawing that needs to be done. ImGui.NET doesn't seem
                    //       to implement ImGuiListClipper fully or I am not understanding how 
                    //       to use it. The below comment is an excerpt from the ImGui samples.
                    /*
                        // Demonstrate using clipper for large vertical lists
                        ImGuiListClipper clipper;
                        clipper.Begin(items.Size);

                        while (clipper.Step())
                        {
                            for (int row_n = clipper.DisplayStart; row_n < clipper.DisplayEnd; row_n++)
                    */

                    // NOTE: ImGuiListClipper seems a little hacky for vertical scrolling. Just implement paging?

                    int rowCounter = 0;
                    foreach (DataFrameRow row in _dataFrame.Rows)
                    {
                        ImGui.TableNextRow();

                        var valueEnumerator = row.GetEnumerator();
                        int columnIdx = 0;
                        int displayColumnIdx = 0;

                        while (valueEnumerator.MoveNext())
                        {
                            var column = _dataFrame.Columns[columnIdx];
                            if (_columnState.ContainsKey(column.Name) && _columnState[column.Name].Visible)
                            {
                                ImGui.TableSetColumnIndex(displayColumnIdx);
                                ImGui.TextUnformatted(Convert.ToString(valueEnumerator.Current));
                                displayColumnIdx++;
                            }
                            columnIdx++;

                        }

                        rowCounter++;
                        if (rowCounter > 100) break;
                    }
                    ImGui.EndTable();
                }
            }
        }
    }
}
