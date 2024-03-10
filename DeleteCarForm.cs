using System;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data;
public class DeleteCarForm : Form
{
    private TextBox _idTextBox;
    private OdbcConnection connection;

    public int Id { get; private set; }
    public DeleteCarForm(OdbcConnection connection)
    {
        this.connection = connection;
        FormLayout();
    }

    private void FormLayout()
    {
        // Set up form properties
        this.Text = "Delete Car";
        this.Size = new System.Drawing.Size(300, 150);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Create labels
        Label idLabel = new Label();
        idLabel.Text = "Car Id:";
        idLabel.Location = new System.Drawing.Point(10, 10);

        // Create text boxes
        _idTextBox = new TextBox();
        _idTextBox.Location = new System.Drawing.Point(125, 10);
        _idTextBox.Width = 150;

        // Create OK and Cancel buttons
        Button okButton = new Button();
        okButton.Text = "Delete";
        okButton.DialogResult = DialogResult.OK;
        okButton.Location = new System.Drawing.Point(80, 80);
        okButton.Click += OkButton_Click;

        Button cancelButton = new Button();
        cancelButton.Text = "Cancel";
        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Location = new System.Drawing.Point(170, 80);

        // Add controls to the form
        this.Controls.Add(_idTextBox);
        this.Controls.Add(idLabel);
        this.Controls.Add(okButton);
        this.Controls.Add(cancelButton);
    }

    private void OkButton_Click(object sender, EventArgs e)
    {
        if (!ValidateInput())
        {
            MessageBox.Show("Invalid input. Please check the fields and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.Cancel;
            return;
        }

        try
        {
            if (!CarExistsInDatabase())
            {
                MessageBox.Show("There is no car with the specified ID in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            removeCarFromDatabase();
        }
        catch (OdbcException ex)
        {
            MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.Cancel;
            return;
        }
        catch (System.Exception ex)
        {
            MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.DialogResult = DialogResult.Cancel;
            return;
        }
        MessageBox.Show("Car with " + Id + " id deleted successfully", "Car added", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.DialogResult = DialogResult.OK;
    }

    private void removeCarFromDatabase()
    {
        using (OdbcCommand command = new OdbcCommand("EXEC deleteCar @carId = ?", connection))
        {
            int.TryParse(_idTextBox.Text, out int parsedId);
            Id = parsedId;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@carId", Id);
            command.ExecuteNonQuery();
        }
    }

    private bool ValidateInput()
    {
        if (!int.TryParse(_idTextBox.Text, out int year) || year < 0)
            return false;

        return true;
    }

    private bool CarExistsInDatabase()
    {
        using (OdbcCommand command = new OdbcCommand("SELECT COUNT(*) FROM Cars WHERE id = ?", connection))
        {
            int.TryParse(_idTextBox.Text, out int parsedId);
            Id = parsedId;
            command.Parameters.AddWithValue("@carId", Id);
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }
}