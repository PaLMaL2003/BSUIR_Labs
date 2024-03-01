using System;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;

public class Form1 : Form
{
    private OdbcConnection connection;
    public Form1()
    {
        FormLayout();
        try
        {
            ConnectToDatabase();
        }
        catch (System.Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }

    }

    public void FormLayout()
    {
        // Set up form properties
        this.Name = "Database controller";
        this.Text = "Database controller";
        this.Size = new System.Drawing.Size(1000, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Create buttons
        Button addButton = new Button();
        addButton.Text = "Add";
        addButton.Location = new System.Drawing.Point(10, 10);
        addButton.Click += AddButton_Click;

        Button deleteButton = new Button();
        deleteButton.Text = "Delete";
        deleteButton.Location = new System.Drawing.Point(120, 10);
        deleteButton.Click += DeleteButton_Click;

        Button viewAllButton = new Button();
        viewAllButton.Text = "View All";
        viewAllButton.Location = new System.Drawing.Point(230, 10);
        viewAllButton.Click += ViewAllButton_Click;

        // Add buttons to the form
        this.Controls.Add(addButton);
        this.Controls.Add(deleteButton);
        this.Controls.Add(viewAllButton);
    }

    private void ConnectToDatabase()
    {
        string connectionString = "Driver={SQL Server};Server=SF;Database=Garage;Uid=sa;Pwd=lolpro29lolpro29;";
        connection = new OdbcConnection(connectionString);
        connection.Open();
    }

    private void ViewAllButton_Click(object sender, EventArgs e)
    {
        using (ViewAllCarsForm viewAllCarsForm = new ViewAllCarsForm(connection))
        {
            viewAllCarsForm.ShowDialog();
        }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        // Open the modal form for adding a new record
        using (DeleteCarForm deleteCarForm = new DeleteCarForm(this.connection))
        {
            deleteCarForm.ShowDialog();
            Console.WriteLine("Delete car form closed");
        }
    }


    private void AddButton_Click(object sender, EventArgs e)
    {
        // Open the modal form for adding a new record
        using (AddCarForm addCarForm = new AddCarForm(this.connection))
        {
            addCarForm.ShowDialog();
            Console.WriteLine("Add car form closed");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (connection != null))
        {
            connection.Dispose();
        }
        base.Dispose(disposing);
    }
}

