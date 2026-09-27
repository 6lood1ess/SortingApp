using SortingApp.Models;
using SortingApp.Services;
using SortingApp.UI.Builders;
using SortingApp.UI.Controls;
using SortingApp.UI.Dialogs;
using SortingApp.Utils;

namespace SortingApp.UI {
  public class MainForm : Form {
    private DataGridView _grid;
    private VisualizationPanel _viz;
    private AlgorithmInfoPanel _infoPanel;
    private StatusStrip _status;
    private ToolStripStatusLabel _statusLabel;
    private MenuBuilder _menuBuilder;

    private readonly SortRunner _runner = new();
    private readonly ThemeManager _theme = new();
    private SortDirection _direction = SortDirection.Ascending;
    private readonly CancellationTokenSource _cts = new();

    public MainForm() {
      Text = "Cортировки с визуализацией";
      Width = 1200;
      Height = 800;
      StartPosition = FormStartPosition.CenterScreen;

      BuildLayout();
      WireRunner();
      RefreshAlgorithmInfo();

      _grid.Rows.Add(24.5, 2.0, 50.0, 17.3, 8.8, 45.1, 12.6, 33.4);

      FormClosing += (_, __) => _cts.Cancel();
      ApplyTheme();
    }

    private void BuildLayout() {
      _menuBuilder = new MenuBuilder();
      _menuBuilder.LoadCsvClicked += LoadFromCsv;
      _menuBuilder.LoadGoogleClicked += async () => await LoadFromGoogleAsync();
      _menuBuilder.GenerateClicked += GenerateData;
      _menuBuilder.BogoLimitClicked += SetBogoLimit;
      _menuBuilder.AscendingSelected += () => _direction = SortDirection.Ascending;
      _menuBuilder.DescendingSelected += () => _direction = SortDirection.Descending;
      _menuBuilder.RunClicked += async () => await RunSortingAsync();
      _menuBuilder.ClearClicked += ClearAll;
      _menuBuilder.ValidateClicked += () => ValidateAll();
      _menuBuilder.RefreshAlgorithmsRequested += RefreshAlgorithmInfo;
      _menuBuilder.ThemeToggleClicked += () => {
        _theme.Toggle();
        _menuBuilder.SetDarkThemeLabel(_theme.IsDark);
        ApplyTheme();
      };

      _menuBuilder.Menu.Dock = DockStyle.Fill;
      MainMenuStrip = _menuBuilder.Menu;

      _status = new StatusStrip { Dock = DockStyle.Fill };
      _statusLabel = new ToolStripStatusLabel("Готово");
      _status.Items.Add(_statusLabel);

      var root = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        RowCount = 3,
        Margin = new Padding(0),
        Padding = new Padding(0)
      };
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
      root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      var content = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 1,
        RowCount = 2,
        Margin = new Padding(6, 6, 6, 0),
        Padding = new Padding(0)
      };
      content.RowStyles.Add(new RowStyle(SizeType.Absolute, 240));
      content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

      var top = new TableLayoutPanel {
        Dock = DockStyle.Fill,
        ColumnCount = 2,
        RowCount = 1,
        Margin = new Padding(0),
        Padding = new Padding(0)
      };
      top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
      top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

      _grid = new DataGridView {
        Dock = DockStyle.Fill,
        AllowUserToAddRows = true,
        AllowUserToDeleteRows = true,
        EditMode = DataGridViewEditMode.EditOnEnter,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
        Margin = new Padding(0, 0, 4, 0)
      };
      _grid.Columns.Add(new DataGridViewTextBoxColumn {
        HeaderText = "Значение",
        Name = "Value"
      });
      _grid.CellValidating += Grid_CellValidating;
      _grid.DataError += (_, e) => e.ThrowException = false;

      _infoPanel = new AlgorithmInfoPanel {
        Dock = DockStyle.Fill,
        Margin = new Padding(4, 0, 0, 0)
      };

      top.Controls.Add(_grid, 0, 0);
      top.Controls.Add(_infoPanel, 1, 0);

      _viz = new VisualizationPanel {
        Dock = DockStyle.Fill,
        Margin = new Padding(0, 6, 0, 0)
      };

      content.Controls.Add(top, 0, 0);
      content.Controls.Add(_viz, 0, 1);

      root.Controls.Add(_menuBuilder.Menu, 0, 0);
      root.Controls.Add(content,           0, 1);
      root.Controls.Add(_status,           0, 2);

      Controls.Add(root);
    }

    private void WireRunner() {
      _runner.Step += (type, arr, a, b) => _viz.UpdateSeries(type, arr, a, b);

      _runner.Finished += result => {
        if (result.Success) {
          _viz.UpdateSeries(result.Algorithm, result.Sorted, -1, -1, result.ElapsedMs);
        } else {
          MessageBox.Show($"{result.Algorithm}: {result.Error}",
                          "Ошибка сортировки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      };
    }

    private void RefreshAlgorithmInfo() {
      _viz.Reset(_menuBuilder.SelectedAlgorithms);
      _infoPanel.Show(_menuBuilder.SelectedAlgorithms);
    }

    private void ApplyTheme() {
      _theme.Apply(this, _grid, _viz, _infoPanel, _menuBuilder.Menu, _status);
    }

    private void Grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
      if (e.ColumnIndex != 0) return;

      var raw = e.FormattedValue?.ToString()?.Trim();
      if (string.IsNullOrEmpty(raw)) return;

      if (!DataValidator.TryParse(raw, out double _, out string error)) {
        e.Cancel = true;
        MessageBox.Show(error, "Ошибка ввода",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private bool ValidateAll() {
      for (int r = 0; r < _grid.Rows.Count; ++r) {
        var cell = _grid.Rows[r].Cells[0];

        if (cell.Value == null) continue;

        var raw = cell.Value.ToString()?.Trim();
        if (string.IsNullOrEmpty(raw)) continue;

        if (!DataValidator.TryParse(raw, out double _, out string error)) {
          MessageBox.Show($"Строка {r + 1}: {error}", "Ошибка",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);

          return false;
        }
      }

      _statusLabel.Text = "Данные корректны.";
      return true;
    }

    private void LoadFromCsv() {
      using var dlg = new OpenFileDialog {
        Filter = "CSV/TSV (*.csv;*.tsv;*.txt)|*.csv;*.tsv;*.txt|Все файлы (*.*)|*.*"
      };

      if (dlg.ShowDialog() != DialogResult.OK) return;

      try {
        var values = DataLoader.LoadFromCsv(dlg.FileName);

        if (values.Count == 0) {
          MessageBox.Show("В файле не найдено чисел.", "Внимание",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);

          return;
        }

        FillGrid(values);
        if (!DataValidator.IsCountValid(values.Count)) {
          MessageBox.Show(
            $"Загружено {values.Count} значений. Допустимо от {DataValidator.MinCount} до {DataValidator.MaxCount}.",
              "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        _statusLabel.Text = $"Загружено {values.Count} значений.";

      } catch (Exception ex) {
        MessageBox.Show("Ошибка чтения файла: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async Task LoadFromGoogleAsync() {
      string url = PromptDialog.Show(
        "Введите URL опубликованной Google-таблицы (CSV):",
        "Google Sheets");

      if (string.IsNullOrWhiteSpace(url)) return;

      try {
        _statusLabel.Text = "Загрузка...";
        var values = await DataLoader.LoadFromGoogleAsync(url.Trim());

        if (values.Count == 0) {
          MessageBox.Show("Не удалось извлечь числа из таблицы.",
                          "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

          _statusLabel.Text = "Готово";
          return;
        }

        FillGrid(values);
        if (!DataValidator.IsCountValid(values.Count)) {
          MessageBox.Show(
            $"Загружено {values.Count} значений. Допустимо от {DataValidator.MinCount} до {DataValidator.MaxCount}.",
              "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        _statusLabel.Text = $"Загружено {values.Count} значений.";

      } catch (Exception ex) {
        MessageBox.Show("Ошибка загрузки: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _statusLabel.Text = "Готово";
      }
    }

    private void GenerateData() {
      using var dlg = new GenerateDataDialog();

      if (dlg.ShowDialog() != DialogResult.OK) return;

      var values = DataLoader.Generate(dlg.Count, dlg.Min, dlg.Max, dlg.Decimals);
      FillGrid(values);
      _statusLabel.Text = $"Сгенерировано {values.Count} значений.";
    }

    private void FillGrid(List<double> values) {
      _grid.Rows.Clear();

      foreach (var v in values) {
        _grid.Rows.Add(v);
      }
    }

    private void ClearAll() {
      _grid.Rows.Clear();
      _viz.Reset(_menuBuilder.SelectedAlgorithms);
      _statusLabel.Text = "Очищено.";
    }

    private void SetBogoLimit() {
      using var dlg = new BogoLimitDialog(_runner.GetBogoLimit());

      if (dlg.ShowDialog() != DialogResult.OK) return;

      _runner.SetBogoLimit(dlg.Limit);
      _statusLabel.Text = $"Лимит BOGO = {dlg.Limit}";
    }

    private double[] ReadGridValues() {
      var list = new List<double>();

      for (int r = 0; r < _grid.Rows.Count; ++r) {
        var cell = _grid.Rows[r].Cells[0];
        if (cell.Value == null) continue;

        var raw = cell.Value.ToString()?.Trim();
        if (string.IsNullOrEmpty(raw)) continue;

        if (!DataValidator.TryParse(raw, out double v, out string error)) {
          throw new InvalidOperationException($"Строка {r + 1}: {error}");
        }

        list.Add(v);
      }

      return list.ToArray();
    }

    private async Task RunSortingAsync() {
      if (_menuBuilder.SelectedAlgorithms.Count == 0) {
        MessageBox.Show("Не выбран ни один алгоритм.", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

        return;
      }

      double[] input;
      try {
        input = ReadGridValues();

      } catch (Exception ex) {
        MessageBox.Show(ex.Message, "Ошибка данных",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

        return;
      }

      if (!DataValidator.IsCountValid(input.Length)) {
        MessageBox.Show(
          $"Количество элементов должно быть от {DataValidator.MinCount} до {DataValidator.MaxCount}. " +
          $"Сейчас: {input.Length}.",
          "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

          return;
      }

      _menuBuilder.ActionsMenu.Enabled = false;
      _statusLabel.Text = "Сортировка...";

      try {
        await _runner.RunAsync(
          input,
          new List<SortAlgorithmType>(_menuBuilder.SelectedAlgorithms),
          _direction,
          _cts.Token);

        _statusLabel.Text = "Сортировка завершена.";

      } catch (OperationCanceledException) {
             _statusLabel.Text = "Сортировка отменена.";

      } catch (Exception ex) {
        MessageBox.Show("Ошибка сортировки: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

        _statusLabel.Text = "Ошибка.";

      } finally {
        _menuBuilder.ActionsMenu.Enabled = true;
      }
    }
  }
}