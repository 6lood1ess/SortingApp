using SortingApp.Models;

namespace SortingApp.UI.Controls {
  public class AlgorithmInfoPanel : Panel {
    private readonly RichTextBox _text;

    public AlgorithmInfoPanel() {
      BackColor = Color.WhiteSmoke;
      Padding = new Padding(8);
      _text = new RichTextBox {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        BorderStyle = BorderStyle.None,
        BackColor = Color.WhiteSmoke,
        ScrollBars = RichTextBoxScrollBars.Vertical,
        Font = new Font("Segoe UI", 10f)
      };

      Controls.Add(_text);
    }

    public void ApplyTheme(bool dark) {
      Color back = dark ? Color.FromArgb(40, 40, 40) : Color.WhiteSmoke;
      Color fore = dark ? Color.Gainsboro : Color.Black;

      BackColor = back;
      _text.BackColor = back;
      _text.ForeColor = fore;
    }

    public void Show(IEnumerable<SortAlgorithmType> algorithms) {
      _text.Clear();

      bool first = true;
      foreach (var t in algorithms) {
        if (!first) {
          _text.AppendText("\n\n");
        }

        first = false;

        _text.SelectionFont = new Font("Segoe UI", 11f, FontStyle.Bold);
        _text.SelectionColor = _text.ForeColor;
        _text.AppendText("Выбран алгоритм: " + AlgorithmDescriptions.GetTitle(t) + "\n");

        _text.SelectionFont = new Font("Segoe UI", 10f, FontStyle.Regular);
        _text.SelectionColor = _text.ForeColor;
        _text.AppendText(AlgorithmDescriptions.GetDescription(t));
      }
    }
  }
}
