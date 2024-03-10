using System;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
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
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }

    public void FormLayout()
    {
        this.Name = "Database controller";
        this.Text = "Database controller";
        this.Size = new System.Drawing.Size(1000, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

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

        Button showProcedure = new Button();
        showProcedure.Text = "Procedures";
        showProcedure.Location = new System.Drawing.Point(340, 10);
        showProcedure.Click += ShowProcedure_Click;


        this.Controls.Add(addButton);
        this.Controls.Add(deleteButton);
        this.Controls.Add(viewAllButton);
        this.Controls.Add(showProcedure);
    }
    private void ShowProcedure_Click(object sender, EventArgs e)
    {
        if (IsDatabaseConnected())
        {
            try
            {
                string query = "SELECT name FROM sys.procedures";
                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        string procedures = "";
                        while (reader.Read())
                        {
                            string procedureName = reader["name"].ToString();
                            procedures += procedureName + Environment.NewLine;
                        }
                        reader.Close();

                        MessageBox.Show(procedures, "Stored Procedures", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            MessageBox.Show("Database connection is lost.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private void ConnectToDatabase()
    {
        string connectionString = "Driver={SQL Server};Server=SF;Database=Garage;Uid=sa;Pwd=lolpro29lolpro29;";
        connection = new OdbcConnection(connectionString);

        try
        {
            connection.Open();
        }
        catch (OdbcException ex)
        {
            throw new Exception("Error connecting to the database: " + ex.Message);
        }
    }

    private void ViewAllButton_Click(object sender, EventArgs e)
    {
        if (IsDatabaseConnected())
        {
            using (ViewAllCarsForm viewAllCarsForm = new ViewAllCarsForm(connection))
            {
                try
                {
                    viewAllCarsForm.ShowDialog();
                }
                catch (OdbcException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Database connection is lost.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
        if (IsDatabaseConnected())
        {
            using (DeleteCarForm deleteCarForm = new DeleteCarForm(this.connection))
            {
                deleteCarForm.ShowDialog();
                Console.WriteLine("Delete car form closed");
            }
        }
        else
        {
            MessageBox.Show("Database connection is lost.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private void AddButton_Click(object sender, EventArgs e)
    {
        if (IsDatabaseConnected())
        {
            using (AddCarForm addCarForm = new AddCarForm(this.connection))
            {
                addCarForm.ShowDialog();
                Console.WriteLine("Add car form closed");
            }
        }
        else
        {
            MessageBox.Show("Database connection is lost.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

    public bool IsDatabaseConnected()
    {
        try
        {

            if (connection.State != ConnectionState.Open)
            {

                connection.OpenAsync();


                return true;
            }



            return true;
        }
        catch (OdbcException ex)
        {


            return false;
        }
    }
}