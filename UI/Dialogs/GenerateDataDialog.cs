namespace SortingApp.UI.Dialogs {
  public class GenerateDataDialog : Form {
    public int Count { get; private set; } = 10;
    public double Min { get; private set; } = -100;
    public double Max { get; private set; } = 100;
    public int Decimals { get; private set; } = 2;

    public GenerateDataDialog() {
      Text = "Генерация данных";
      Width = 413; Height = 270;
      StartPosition = FormStartPosition.CenterParent;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false; MinimizeBox = false;
      AutoScaleMode = AutoScaleMode.Font;
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      Font = new System.Drawing.Font("Segoe UI", 9F);

      var lblCount = new Label {
        Text = "Количество (2..50):", Left = 15, Top = 15, Width = 420 };
      var tbCount = new TextBox {
        Left = 15, Top = 38, Width = 420, Text = "10" };

      var lblMin = new Label {
        Text = "Минимум значения:", Left = 15, Top = 70, Width = 420 };
      var tbMin = new TextBox {
        Left = 15, Top = 93, Width = 420, Text = "-100" };

      var lblMax = new Label {
        Text = "Максимум значения:", Left = 15, Top = 125, Width = 420 };
      var tbMax = new TextBox {
        Left = 15, Top = 148, Width = 420, Text = "100" };

      var lblDec = new Label {
        Text = "Знаков после запятой (0..4):", Left = 15, Top = 180, Width = 420 };
      var tbDec = new TextBox {
        Left = 15, Top = 203, Width = 420, Text = "2" };

      var okButton = new Button {
        Text = "OK", Left = 260, Top = 250, Width = 80, DialogResult = DialogResult.OK };
      var cancelButton = new Button {
        Text = "Отмена", Left = 355, Top = 250, Width = 80, DialogResult = DialogResult.Cancel };

      Controls.AddRange(new Control[] { lblCount, tbCount, lblMin, tbMin, lblMax, tbMax, lblDec, tbDec, okButton, cancelButton });
      AcceptButton = okButton; CancelButton = cancelButton;

      okButton.Click += (_, __) => {
        if (!int.TryParse(tbCount.Text, out int c) || !Services.DataValidator.IsCountValid(c)) {
          MessageBox.Show(
            $"Количество элементов: {Services.DataValidator.MinCount}..{Services.DataValidator.MaxCount}.");

          return;
        }

        if (!Utils.NumberParser.TryParse(tbMin.Text, out double mn)) {
          MessageBox.Show("Минимум — не число.");
          return;
        }

        if (!Utils.NumberParser.TryParse(tbMax.Text, out double mx)) {
          MessageBox.Show("Максимум — не число.");
          return;
        }

          if (mn > mx) {
          MessageBox.Show("Минимум не может быть больше максимума.");
          return;
        }

        if (!int.TryParse(tbDec.Text, out int d) || d < 0 || d > 4) {
          MessageBox.Show("Знаков после запятой: 0..4.");
          return;
        }

        Count = c; Min = mn; Max = mx; Decimals = d;
        DialogResult = DialogResult.OK;
        Close();
      };
    }
  }
}
