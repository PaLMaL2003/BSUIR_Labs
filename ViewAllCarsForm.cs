using System;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data;

public class ViewAllCarsForm : Form
{
    private DataGridView dataGridView;

    public ViewAllCarsForm(OdbcConnection connection)
    {
        FormLayout();
        try
        {
            PopulateData(connection);
        }
        catch (System.Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
            this.DialogResult = DialogResult.Cancel;
            return;
        }

    }

    private void FormLayout()
    {
        // Set up form properties
        this.Name = "View All Cars";
        this.Text = "View All Cars";
        this.Size = new System.Drawing.Size(500, 400);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Create DataGridView control
        dataGridView = new DataGridView();
        dataGridView.Dock = DockStyle.Fill;

        // Add DataGridView to the form
        this.Controls.Add(dataGridView);
    }

    private void PopulateData(OdbcConnection connection)
    {
        // Retrieve data from the "Cars" table
        using (OdbcCommand command = new OdbcCommand("SELECT * FROM Cars", connection))
        {
            try
            {
                using (OdbcDataAdapter adapter = new OdbcDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the data to the DataGridView
                    dataGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}