using SortingApp.Models;

namespace SortingApp.Algorithms {
  public class BubbleSort : ISortAlgorithm {
    public SortAlgorithmType Type => SortAlgorithmType.Bubble;

    public double[] Sort(double[] input, SortDirection direction, SortContext context) {
      var a = (double[])input.Clone();
      bool asc = direction == SortDirection.Ascending;

      for (int i = 0; i < a.Length - 1; ++i) {
        for (int j = 0; j < a.Length - 1 - i; ++j) {
          context.CheckCancellation();

          bool swap = asc ? a[j] > a[j + 1] : a[j] < a[j + 1];
          if (swap) {
            (a[j], a[j + 1]) = (a[j + 1], a[j]);
          }

          context.Report(a, j, j + 1);
        }
      }

      return a;
    }
  }
}
