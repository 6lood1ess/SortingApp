using SortingApp.Models;

namespace SortingApp.Algorithms {
  public class ShakerSort : ISortAlgorithm {
    public SortAlgorithmType Type => SortAlgorithmType.Shaker;

    public double[] Sort(double[] input, SortDirection direction, SortContext context) {
      var a = (double[])input.Clone();
      bool asc = direction == SortDirection.Ascending;
      int left = 0, right = a.Length - 1;

      while (left < right) {
        context.CheckCancellation();

        for (int i = left; i < right; ++i) {
          if (asc ? a[i] > a[i + 1] : a[i] < a[i + 1]) {
            (a[i], a[i + 1]) = (a[i + 1], a[i]);
          }

          context.Report(a, i, i + 1);
        }

        right--;

        for (int i = right; i > left; --i) {
          if (asc ? a[i - 1] > a[i] : a[i - 1] < a[i]) {
            (a[i - 1], a[i]) = (a[i], a[i - 1]);
          }

          context.Report(a, i - 1, i);
        }

        left++;
      }

      return a;
    }
  }
}
