namespace SortingApp.UI.Dialogs {
  public class BogoLimitDialog : Form {
    public int Limit { get; private set; } = 100_000;

    public BogoLimitDialog(int current) {
      Text = "Лимит итераций BOGO";
      Width = 340; Height = 156;
      StartPosition = FormStartPosition.CenterParent;
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false; MinimizeBox = false;

      var lbl = new Label {
        Text = "Максимум итераций:", Left = 10,
        Top = 15, Width = 300
      };
      var tb = new TextBox {
        Left = 10, Top = 40,
        Width = 300, Text = current.ToString()
      };

      var okButton = new Button {
        Text = "OK", Left = 146,
        Top = 75, Width = 80,
        DialogResult = DialogResult.OK
      };
      var cancelButton = new Button {
        Text = "Отмена", Left = 231,
        Top = 75, Width = 80,
        DialogResult = DialogResult.Cancel
      };

      Controls.AddRange(new Control[] { lbl, tb, okButton, cancelButton });
      AcceptButton = okButton; CancelButton = cancelButton;

      okButton.Click += (_, __) => {
        if (!int.TryParse(tb.Text, out int v) || v <= 0) {
          MessageBox.Show("Введите положительное целое число.");
          return;
        }

        Limit = v;
        DialogResult = DialogResult.OK;
        Close();
      };
    }
  }
}
