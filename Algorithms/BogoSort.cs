using SortingApp.Models;

namespace SortingApp.Algorithms {
  public class BogoSort : ISortAlgorithm {
    public SortAlgorithmType Type => SortAlgorithmType.Bogo;

    // Лимит итераций задаётся извне (см. BogoSettings)
    public int MaxIterations { get; set; } = 100_000;

    public double[] Sort(double[] input, SortDirection direction, SortContext context) {
      var a = (double[])input.Clone();
      bool asc = direction == SortDirection.Ascending;
      var rnd = new Random();
      int iter = 0;

      while (!IsSorted(a, asc)) {
        context.CheckCancellation();

        if (iter >= MaxIterations) {
          throw new InvalidOperationException($"BOGO-сортировка не завершилась за {MaxIterations} итераций.");
        }

        for (int i = a.Length - 1; i > 0; --i) {
          int j = rnd.Next(i + 1);
          (a[i], a[j]) = (a[j], a[i]);
        }

        context.Report(a);
        iter++;
      }

      context.Report(a);

      return a;
    }

    private static bool IsSorted(double[] a, bool asc) {
      for (int i = 1; i < a.Length; ++i) {
        if (asc && a[i - 1] > a[i]) return false;
        if (!asc && a[i - 1] < a[i]) return false;
      }

      return true;
    }
  }
}
