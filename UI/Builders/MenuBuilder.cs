using SortingApp.Models;

namespace SortingApp.UI.Builders {
  public class MenuBuilder {
    public MenuStrip Menu { get; }
    public ToolStripMenuItem LoadMenu { get; }
    public ToolStripMenuItem AlgorithmsMenu { get; }
    public ToolStripMenuItem ActionsMenu { get; }

    public event Action LoadCsvClicked;
    public event Action LoadGoogleClicked;
    public event Action GenerateClicked;
    public event Action RefreshAlgorithmsRequested;
    public event Action BogoLimitClicked;
    public event Action AscendingSelected;
    public event Action DescendingSelected;
    public event Action RunClicked;
    public event Action ClearClicked;
    public event Action ValidateClicked;
    public event Action ThemeToggleClicked;

    private ToolStripMenuItem _themeItem;

        public HashSet<SortAlgorithmType> SelectedAlgorithms { get; } = new();

    public MenuBuilder() {
      Menu = new MenuStrip();
      LoadMenu = new ToolStripMenuItem("Загрузка");
      AlgorithmsMenu = new ToolStripMenuItem("Алгоритмы");
      ActionsMenu = new ToolStripMenuItem("Действия");

      BuildLoad();
      BuildAlgorithms();
      BuildDirection();
      BuildActions();

      Menu.Items.Add(LoadMenu);
      Menu.Items.Add(AlgorithmsMenu);
      Menu.Items.Add(new ToolStripMenuItem("Направление", null, BuildDirectionItems()));
      Menu.Items.Add(ActionsMenu);

      var spacer = new ToolStripMenuItem { Alignment = ToolStripItemAlignment.Right };
      _themeItem = new ToolStripMenuItem("🌙 Тёмная тема") {
        Alignment = ToolStripItemAlignment.Right
      };

      _themeItem.Click += (_, __) => ThemeToggleClicked?.Invoke();

      Menu.Items.Add(spacer);
      Menu.Items.Add(_themeItem);
    }

    public void SetDarkThemeLabel(bool dark)
      => _themeItem.Text = dark ? "☀ Светлая тема" : "🌙 Тёмная тема";

    private void BuildLoad() {
      LoadMenu.DropDownItems.Add("Из Excel/CSV...", null, (_, __) => LoadCsvClicked?.Invoke());
      LoadMenu.DropDownItems.Add("Из Google-таблицы...", null, (_, __) => LoadGoogleClicked?.Invoke());
      LoadMenu.DropDownItems.Add("Сгенерировать...", null, (_, __) => GenerateClicked?.Invoke());
    }

    private void BuildAlgorithms() {
      foreach (SortAlgorithmType t in SortAlgorithmFactoryAll()) {
        var item = new ToolStripMenuItem(AlgorithmDescriptions.GetTitle(t)) {
          Tag = t,
          Checked = true,
          CheckOnClick = true 
        };

        item.CheckedChanged += (_, __) => RecomputeSelected();
        AlgorithmsMenu.DropDownItems.Add(item);
        SelectedAlgorithms.Add(t);
      }

      AlgorithmsMenu.DropDownItems.Add(new ToolStripSeparator());
      AlgorithmsMenu.DropDownItems.Add("Лимит итераций BOGO...", null,
        (_, __) => BogoLimitClicked?.Invoke());
    }

    private void BuildDirection() {
    }

    private ToolStripItem[] BuildDirectionItems() {
      var asc = new ToolStripMenuItem("По возрастанию") { Checked = true, CheckOnClick = true };
      var desc = new ToolStripMenuItem("По убыванию") { Checked = false, CheckOnClick = true };

      asc.Click += (_, __) => { asc.Checked = true; desc.Checked = false; AscendingSelected?.Invoke(); };
      desc.Click += (_, __) => { desc.Checked = true; asc.Checked = false; DescendingSelected?.Invoke(); };

      return new ToolStripItem[] { asc, desc };
    }

    private void BuildActions() {
      ActionsMenu.DropDownItems.Add("Рассчитать", null, (_, __) => RunClicked?.Invoke());
      ActionsMenu.DropDownItems.Add("Очистить", null, (_, __) => ClearClicked?.Invoke());
      ActionsMenu.DropDownItems.Add(new ToolStripSeparator());
      ActionsMenu.DropDownItems.Add("Проверить данные", null, (_, __) => ValidateClicked?.Invoke());
    }

    private void RecomputeSelected() {
      SelectedAlgorithms.Clear();

      foreach (ToolStripItem item in AlgorithmsMenu.DropDownItems) {
        if (item is ToolStripMenuItem mi && mi.CheckOnClick && mi.Checked && mi.Tag is SortAlgorithmType t) {
          SelectedAlgorithms.Add(t);
        }
      }

      RefreshAlgorithmsRequested?.Invoke();
    }

    private static IEnumerable<SortAlgorithmType> SortAlgorithmFactoryAll()
      => Algorithms.SortAlgorithmFactory.All();
  }
}
