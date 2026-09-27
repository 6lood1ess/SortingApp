using SortingApp.Models;

namespace SortingApp.Algorithms {
  public class QuickSort : ISortAlgorithm {
    public SortAlgorithmType Type => SortAlgorithmType.Quick;

    public double[] Sort(double[] input, SortDirection direction, SortContext context) {
      var a = (double[])input.Clone();
      bool asc = direction == SortDirection.Ascending;

      QuickRec(a, 0, a.Length - 1, asc, context);

      return a;
    }

    private static void QuickRec(double[] a, int lo, int hi, bool asc, SortContext ctx) {
      if (lo >= hi) return;

      ctx.CheckCancellation();

      int p = Partition(a, lo, hi, asc, ctx);

      QuickRec(a, lo, p - 1, asc, ctx);
      QuickRec(a, p + 1, hi, asc, ctx);
    }

    private static int Partition(double[] a, int lo, int hi, bool asc, SortContext ctx) {
      double pivot = a[hi];
      int i = lo - 1;

      for (int j = lo; j < hi; ++j) {
        ctx.CheckCancellation();

        if (asc ? a[j] < pivot : a[j] > pivot) {
          i++;
          (a[i], a[j]) = (a[j], a[i]);
        }

        ctx.Report(a, i, j);
      }

      (a[i + 1], a[hi]) = (a[hi], a[i + 1]);
      ctx.Report(a, i + 1, hi);

      return i + 1;
    }
  }
}
