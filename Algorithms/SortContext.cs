namespace SortingApp.Algorithms {
  public class SortContext {
    private readonly Action<double[], int, int> _onStep;
    private readonly Action _checkCancellation;

    public SortContext(Action<double[], int, int> onStep, Action checkCancellation = null) {
      _onStep = onStep;
      _checkCancellation = checkCancellation ?? (() => { });
    }

    public void CheckCancellation() => _checkCancellation();

    public void Report(double[] array, int a = -1, int b = -1)
      => _onStep?.Invoke(array, a, b);
  }
}
