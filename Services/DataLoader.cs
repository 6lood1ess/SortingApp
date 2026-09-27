namespace SortingApp.Services {
  public static class DataLoader {
    private static readonly HttpClient Http = new HttpClient();

    public static List<double> LoadFromCsv(string path) {
      var result = new List<double>();

      foreach (var line in File.ReadAllLines(path)) {
        foreach (var token in line.Split(new[] { ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)) {
          if (Utils.NumberParser.TryParse(token, out double v)) {
            result.Add(v);
          }
        }
      }

      return result;
    }

    public static async Task<List<double>> LoadFromGoogleAsync(string url) {
      var result = new List<double>();
      string content = await Http.GetStringAsync(url);

      foreach (var line in content.Split('\n')) {
        foreach (var token in line.Split(new[] { ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)) {
          if (Utils.NumberParser.TryParse(token, out double v)) {
            result.Add(v);
          }
        }
      }

      return result;
    }

    public static List<double> Generate(int count, double min, double max, int decimals) {
      var rnd = new Random();
      var list = new List<double>(count);

      for (int i = 0; i < count; ++i) {
        list.Add(Math.Round(min + rnd.NextDouble() * (max - min), decimals));
      }

      return list;
    }
  }
}
