using System.Diagnostics;
using SortingApp.Algorithms;
using SortingApp.Models;

namespace SortingApp.Services {
  public class SortRunner {
    public event Action<SortAlgorithmType, double[], int, int> Step;
    public event Action<SortResult> Finished;

    private readonly Dictionary<SortAlgorithmType, int> _bogoLimits = new();

    public void SetBogoLimit(int limit) => _bogoLimits[SortAlgorithmType.Bogo] = limit;

    public int GetBogoLimit()
    => _bogoLimits.TryGetValue(SortAlgorithmType.Bogo, out int v) ? v : 100_000;

    public async Task RunAsync(
      double[] input,
      IEnumerable<SortAlgorithmType> algorithms,
      SortDirection direction,
      CancellationToken token) {

      foreach (var type in algorithms) {
        var result = await Task.Run(() => RunOne(input, type, direction, token), token);
        Finished?.Invoke(result);
      }
    }

    private SortResult RunOne(double[] input, SortAlgorithmType type, SortDirection dir, CancellationToken token) {
      var alg = SortAlgorithmFactory.Create(type);

      if (alg is BogoSort bogo && _bogoLimits.TryGetValue(type, out int lim)) {
        bogo.MaxIterations = lim;
      }

      var ctx = new SortContext(
        onStep: (arr, i, j) => Step?.Invoke(type, arr, i, j),
        checkCancellation: () => token.ThrowIfCancellationRequested());

      var sw = System.Diagnostics.Stopwatch.StartNew();
      try {
        var sorted = alg.Sort(input, dir, ctx);
        sw.Stop();
        return SortResult.Ok(type, sorted, sw.Elapsed.TotalMilliseconds);

      } catch (Exception ex) {
        sw.Stop();
        return SortResult.Fail(type, ex.Message, sw.Elapsed.TotalMilliseconds);
      }
    }
  }
}
