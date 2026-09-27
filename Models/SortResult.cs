namespace SortingApp.Models {
  public class SortResult {
    public SortAlgorithmType Algorithm { get; init; }
    public double[] Sorted { get; init; } = System.Array.Empty<double>();
    public double ElapsedMs { get; init; }
    public bool Success { get; init; }
    public string? Error { get; init; }

    public static SortResult Ok(SortAlgorithmType alg, double[] arr, double ms) =>
      new() { Algorithm = alg, Sorted = arr, ElapsedMs = ms, Success = true };

    public static SortResult Fail(SortAlgorithmType alg, string error, double ms) =>
      new() { Algorithm = alg, Error = error, ElapsedMs = ms, Success = false };
  }
}
