using SortingApp.Models;

namespace SortingApp.Algorithms {
  public interface ISortAlgorithm {
    SortAlgorithmType Type { get; }

    // Сортирует копию массива. Через <see cref="SortContext.Report"/> оповещает о шагах
    double[] Sort(double[] input, SortDirection direction, SortContext context);
  }
}
