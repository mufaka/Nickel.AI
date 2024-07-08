using ImGuiNET;
using Microsoft.Data.Analysis;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Controls
{
    public class DataFrameTable : ImGuiControl
    {
        public int PageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;

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

                ImGui.Separator();
                DrawPager();

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

                    var rows = _dataFrame.Rows.Skip((PageNumber - 1) * PageSize).Take(PageSize);

                    foreach (DataFrameRow row in rows)
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
                    }
                    ImGui.EndTable();
                }
            }
        }

        private void DrawPager()
        {
            var pages = (_dataFrame!.Rows.Count + PageSize - 1) / PageSize;

            if (PageNumber > 1)
            {
                if (ImGui.Button("<<"))
                {
                    PageNumber = 1;
                }
                ImGui.SameLine();
                if (ImGui.Button("<"))
                {
                    PageNumber -= 1;
                }
                ImGui.SameLine();
            }

            var firstPager = Math.Max(1, PageNumber - 3);
            var lastPager = Math.Min(firstPager + 5, pages);

            for (int x = firstPager; x <= lastPager; x++)
            {
                if (x == PageNumber)
                {
                    Vector4 color = ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonActive];
                    ImGui.PushStyleColor(ImGuiCol.Button, color);
                    ImGui.Button($"{x}");
                    ImGui.PopStyleColor();
                }
                else
                {
                    if (ImGui.Button($"{x}"))
                    {
                        PageNumber = x;
                    }
                }
                ImGui.SameLine();
            }

            if (PageNumber < pages)
            {
                if (ImGui.Button(">"))
                {
                    PageNumber += 1;
                }
                ImGui.SameLine();

                if (ImGui.Button(">>"))
                {
                    PageNumber = (int)pages;
                }
            }
            else
            {
                ImGui.NewLine();
            }
        }
    }
}
