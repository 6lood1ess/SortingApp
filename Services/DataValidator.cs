using SortingApp.Utils;

namespace SortingApp.Services {
  public static class DataValidator {
    public const int MinCount = 2;
    public const int MaxCount = 50;

    public static bool IsCountValid(int count) => count >= MinCount && count <= MaxCount;

    public static bool TryParse(string raw, out double value, out string error) {
      value = 0;
      error = string.Empty;

      if (string.IsNullOrWhiteSpace(raw)) {
        error = "Пустое значение.";
        return false;
      }

      if (!NumberParser.TryParse(raw, out value)) {
        error = $"«{raw}» — не число.";
        return false;
      }

      return true;
    }
  }
}
