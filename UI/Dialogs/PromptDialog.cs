namespace SortingApp.UI.Dialogs {
  public static class PromptDialog {
    public static string Show(string text, string caption, string defaultValue = "") {
      using var form = new Form {
        Width = 420,
        Height = 150,
        Text = caption,
        StartPosition = FormStartPosition.CenterParent,
        FormBorderStyle = FormBorderStyle.FixedDialog,
        MinimizeBox = false,
        MaximizeBox = false
      };

      var lbl = new Label {
        Left = 10, Top = 10,
        Width = 380, Text = text 
      };
      var tb = new TextBox {
        Left = 10, Top = 35,
        Width = 380, Text = defaultValue
      };
      var okButton = new Button {
        Text = "OK", Left = 220,
        Width = 80, Top = 70,
        DialogResult = DialogResult.OK 
      };
      var cancelButton = new Button {
        Text = "Отмена", Left = 310,
        Width = 80, Top = 70,
        DialogResult = DialogResult.Cancel 
      };

      form.Controls.AddRange(new Control[] { lbl, tb, okButton, cancelButton });
      form.AcceptButton = okButton; form.CancelButton = cancelButton;

      return form.ShowDialog() == DialogResult.OK ? tb.Text : "";
    }
  }
}
