using System.Globalization;

namespace SortingApp.Utils {
  public static class NumberParser {
    public static bool TryParse(string raw, out double value) {
      if (string.IsNullOrWhiteSpace(raw)) {
        value = 0;
        return false;
      }

      var normalized = raw.Trim().Replace('.', ',');
      return double.TryParse(normalized,
        NumberStyles.Float | NumberStyles.AllowThousands,
        CultureInfo.GetCultureInfo("ru-RU"), out value);
    }
  }
}
