using SortingApp.UI;

namespace SortingApp {
  internal static class Program {
    [STAThread]
    static void Main() {
      ApplicationConfiguration.Initialize();
      Application.Run(new MainForm());
    }
  }
}