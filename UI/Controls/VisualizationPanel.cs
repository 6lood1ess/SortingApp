using System.Drawing.Drawing2D;
using SortingApp.Models;

namespace SortingApp.UI.Controls {
  public class VisualizationPanel : Panel {
    private class Series {
      public string Name = "";
      public double[] Values = Array.Empty<double>();
      public int HighlightA = -1;
      public int HighlightB = -1;
      public Color Color = Color.SteelBlue;
      public double TimeMs = -1;
    }

    private readonly Dictionary<SortAlgorithmType, Series> _series = new();
    private readonly object _sync = new();

    public VisualizationPanel() {
      DoubleBuffered = true;
      BackColor = Color.White;
    }

    public void Reset(IEnumerable<SortAlgorithmType> algorithms) {
      lock (_sync) {
        _series.Clear();

        foreach (var a in algorithms) {
          _series[a] = new Series { Name = a.ToString(), Color = PickColor(a) };
        }
      }

      Invalidate();
    }

    public void UpdateSeries(SortAlgorithmType type, double[] values, int a, int b, double? timeMs = null) {
      lock (_sync) {
        if (!_series.TryGetValue(type, out var s)) return;

        s.Values = (double[])values.Clone();
        s.HighlightA = a;
        s.HighlightB = b;

        if (timeMs.HasValue) s.TimeMs = timeMs.Value;
      }

      Invalidate();
    }

    private static Color PickColor(SortAlgorithmType t) => t switch {
      SortAlgorithmType.Bubble => Color.IndianRed,
      SortAlgorithmType.Insertion => Color.SeaGreen,
      SortAlgorithmType.Shaker => Color.MediumPurple,
      SortAlgorithmType.Quick => Color.Orange,
      SortAlgorithmType.Bogo => Color.SlateGray,
      _ => Color.SteelBlue
    };

    private bool _dark;

    public void ApplyTheme(bool dark) {
      _dark = dark;
      BackColor = dark ? Color.FromArgb(40, 40, 40) : Color.White;
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);

      lock (_sync) {
        if (_series.Count == 0) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        int rowH = Height / _series.Count;
        int top = 0;

        foreach (var s in _series.Values) {
          DrawSeries(e.Graphics, s, top, rowH);
          top += rowH;
        }
      }
    }

    private void DrawSeries(Graphics g, Series s, int top, int height) {
      Color border = _dark ? Color.FromArgb(80, 80, 80) : Color.LightGray;
      using var borderPen = new Pen(border);
      g.DrawRectangle(borderPen, 0, top, Width - 1, height - 1);

      string header = s.Name;

      if (s.TimeMs >= 0) {
        if (s.TimeMs < 1.0) {
          header += $"  |  {s.TimeMs * 1000.0:F1} мкс";
        } else {
          header += $"  |  {s.TimeMs:F3} мс";
        }
      }

      using (var f = new Font("Segoe UI", 9, FontStyle.Bold)) {
        using (var b = new SolidBrush(s.Color)) {
          g.DrawString(header, f, b, 6, top + 4);
        }
      }

      if (s.Values.Length == 0) return;

      int pad = 8;
      int bottom = top + height - pad;
      int topBar = top + 22;
      int usableH = bottom - topBar;

      if (usableH <= 0) return;

      double maxV = 0;
      foreach (var v in s.Values) if (v > maxV) maxV = v;
      if (maxV <= 0) return;

      int n = s.Values.Length;
      int barW = Math.Max(2, (Width - 2 * pad) / n);
      int gap = barW > 6 ? 1 : 0;

      for (int i = 0; i < n; ++i) {
        int h = (int)(s.Values[i] / maxV * usableH);
        var rect = new Rectangle(pad + i * barW, bottom - h, barW - gap, h);
        Color c = (i == s.HighlightA || i == s.HighlightB) ? Color.Red : s.Color;
        using var brush = new SolidBrush(c);
        g.FillRectangle(brush, rect);
      }
    }
  }
}
