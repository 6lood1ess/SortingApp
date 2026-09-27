using SortingApp.Models;

namespace SortingApp.Algorithms {
  public static class SortAlgorithmFactory {
    public static ISortAlgorithm Create(SortAlgorithmType type) => type switch {
      SortAlgorithmType.Bubble => new BubbleSort(),
      SortAlgorithmType.Insertion => new InsertionSort(),
      SortAlgorithmType.Shaker => new ShakerSort(),
      SortAlgorithmType.Quick => new QuickSort(),
      SortAlgorithmType.Bogo => new BogoSort(),

      _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    public static IEnumerable<SortAlgorithmType> All() {
      yield return SortAlgorithmType.Bubble;
      yield return SortAlgorithmType.Insertion;
      yield return SortAlgorithmType.Shaker;
      yield return SortAlgorithmType.Quick;
      yield return SortAlgorithmType.Bogo;
    }
  }
}
