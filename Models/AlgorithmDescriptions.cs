namespace SortingApp.Models {
  public static class AlgorithmDescriptions {
    public static readonly Dictionary<SortAlgorithmType, (string Title, string Description)> Map = new() {
      [SortAlgorithmType.Bubble] = (
        "Пузырьковая сортировка",
        "Многократно проходит по массиву, сравнивая соседние элементы и меняя их местами, " +
        "если они стоят в неправильном порядке."),

      [SortAlgorithmType.Insertion] = (
        "Сортировка вставками",
        "Последовательно берёт каждый элемент и вставляет его в уже отсортированную часть массива " +
        "на нужную позицию."),

      [SortAlgorithmType.Shaker] = (
        "Шейкерная сортировка",
        "Улучшенная пузырьковая сортировка: проходы выполняются в обе стороны поочерёдно."),

      [SortAlgorithmType.Quick] = (
        "Быстрая сортировка",
        "Рекурсивно выбирает опорный элемент и разделяет массив на две части: меньше и больше опорного."),

      [SortAlgorithmType.Bogo] = (
        "BOGO-сортировка",
        "Случайно перемешивает массив до тех пор, пока он не окажется отсортированным. " +
        "Имеет ограничение по итерациям во избежание бесконечного выполнения.")
    };

    public static string GetTitle(SortAlgorithmType t)
      => Map.TryGetValue(t, out var v) ? v.Title : t.ToString();

    public static string GetDescription(SortAlgorithmType t)
      => Map.TryGetValue(t, out var v) ? v.Description : "";
  }
}
