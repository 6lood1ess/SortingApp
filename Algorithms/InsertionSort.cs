using SortingApp.Models;

namespace SortingApp.Algorithms {
  public class InsertionSort : ISortAlgorithm {
    public SortAlgorithmType Type => SortAlgorithmType.Insertion;

    public double[] Sort(double[] input, SortDirection direction, SortContext context) {
      var a = (double[])input.Clone();
      bool asc = direction == SortDirection.Ascending;

      for (int i = 1; i < a.Length; ++i) {
        context.CheckCancellation();
        double key = a[i];

        int j = i - 1;
        while (j >= 0 && (asc ? a[j] > key : a[j] < key)) {
          a[j + 1] = a[j];
          j--;
          context.Report(a, j + 1, j);
        }

        a[j + 1] = key;
        context.Report(a, j + 1, i);
      }

      return a;
    }
  }
}
