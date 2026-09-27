using SortingApp.UI.Controls;

namespace SortingApp.Utils {
  public class ThemeManager {
    public bool IsDark { get; private set; }

    public Color FormBack => IsDark ? Color.FromArgb(32, 32, 32) : SystemColors.Control;
    public Color FormFore => IsDark ? Color.Gainsboro : Color.Black;
    public Color GridBack => IsDark ? Color.FromArgb(45, 45, 45) : Color.White;
    public Color GridFore => IsDark ? Color.Gainsboro : Color.Black;
    public Color GridHeaderBack => IsDark ? Color.FromArgb(60, 60, 60) : SystemColors.Control;
    public Color GridHeaderFore => IsDark ? Color.Gainsboro : Color.Black;
    public Color VizBack => IsDark ? Color.FromArgb(40, 40, 40) : Color.WhiteSmoke;
    public Color MenuBack => IsDark ? Color.FromArgb(50, 50, 50) : SystemColors.Control;
    public Color MenuFore => IsDark ? Color.Gainsboro : Color.Black;
    public Color StatusBack => IsDark ? Color.FromArgb(45, 45, 45) : SystemColors.Control;
    public Color StatusFore => IsDark ? Color.Gainsboro : Color.Black;

    public void Toggle() => IsDark = !IsDark;

    public void Apply(Form form, DataGridView grid,
                      VisualizationPanel viz, AlgorithmInfoPanel info,
                      MenuStrip menu, StatusStrip status) {

      form.BackColor = FormBack;
      form.ForeColor = FormFore;

      menu.BackColor = MenuBack;
      menu.ForeColor = MenuFore;

      foreach (ToolStripItem item in menu.Items) {
        ApplyMenuColors(item);
      }

      grid.BackgroundColor = GridBack;
      grid.GridColor = IsDark ? Color.FromArgb(80, 80, 80) : SystemColors.ControlDark;
      grid.DefaultCellStyle.BackColor = GridBack;
      grid.DefaultCellStyle.ForeColor = GridFore;
      grid.DefaultCellStyle.SelectionBackColor = IsDark ? Color.FromArgb(90, 90, 140) : SystemColors.Highlight;
      grid.DefaultCellStyle.SelectionForeColor = Color.White;
      grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBack;
      grid.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderFore;
      grid.EnableHeadersVisualStyles = false;
      grid.RowHeadersDefaultCellStyle.BackColor = GridHeaderBack;
      grid.RowHeadersDefaultCellStyle.ForeColor = GridHeaderFore;

      viz.ApplyTheme(IsDark);
      info.ApplyTheme(IsDark);

      status.BackColor = StatusBack;
      status.ForeColor = StatusFore;

      foreach (ToolStripItem item in status.Items) {
        item.BackColor = StatusBack;
        item.ForeColor = StatusFore;
      }
    }

    private void ApplyMenuColors(ToolStripItem item) {
      item.BackColor = MenuBack;
      item.ForeColor = MenuFore;

      if (item is ToolStripMenuItem mi) {
        foreach (ToolStripItem sub in mi.DropDownItems) {
          ApplyMenuColors(sub);
        }
      }
    }
  }
}
