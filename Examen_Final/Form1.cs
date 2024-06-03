using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PeliculasCRUD
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost;Database=peliscrud;Uid=root;Pwd=LuisFlores;";
        

        public Form1()
        {
            InitializeComponent();
            LeerPeliculas();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Películas";
            this.Size = new Size(800, 600);

            GroupBox gbDatos = new GroupBox();
            gbDatos.Text = "Información de la Película";
            gbDatos.Location = new Point(10, 10);
            gbDatos.Size = new Size(350, 300);
            this.Controls.Add(gbDatos);

            GroupBox gbAcciones = new GroupBox();
            gbAcciones.Text = "Acciones";
            gbAcciones.Location = new Point(370, 10);
            gbAcciones.Size = new Size(150, 300);
            this.Controls.Add(gbAcciones);

            Label lblTitulo = new Label() { Text = "Título", Location = new Point(20, 30), AutoSize = true };
            Label lblDirector = new Label() { Text = "Director", Location = new Point(20, 70), AutoSize = true };
            Label lblFechaEstreno = new Label() { Text = "Fecha Estreno", Location = new Point(20, 110), AutoSize = true };
            Label lblGenero = new Label() { Text = "Género", Location = new Point(20, 150), AutoSize = true };
            Label lblDuracion = new Label() { Text = "Duración", Location = new Point(20, 190), AutoSize = true };
            Label lblRating = new Label() { Text = "Rating", Location = new Point(20, 230), AutoSize = true };
            Label lblIdioma = new Label() { Text = "Idioma", Location = new Point(20, 270), AutoSize = true };

            TextBox txtTitulo = new TextBox() { Name = "txtTitulo", Location = new Point(120, 30), Width = 200 };
            TextBox txtDirector = new TextBox() { Name = "txtDirector", Location = new Point(120, 70), Width = 200 };
            DateTimePicker dtpFechaEstreno = new DateTimePicker() { Name = "dtpFechaEstreno", Location = new Point(120, 110), Width = 200 };
            TextBox txtGenero = new TextBox() { Name = "txtGenero", Location = new Point(120, 150), Width = 200 };
            TextBox txtDuracion = new TextBox() { Name = "txtDuracion", Location = new Point(120, 190), Width = 200 };
            TextBox txtRating = new TextBox() { Name = "txtRating", Location = new Point(120, 230), Width = 200 };
            TextBox txtIdioma = new TextBox() { Name = "txtIdioma", Location = new Point(120, 270), Width = 200 };

            gbDatos.Controls.Add(lblTitulo);
            gbDatos.Controls.Add(lblDirector);
            gbDatos.Controls.Add(lblFechaEstreno);
            gbDatos.Controls.Add(lblGenero);
            gbDatos.Controls.Add(lblDuracion);
            gbDatos.Controls.Add(lblRating);
            gbDatos.Controls.Add(lblIdioma);
            gbDatos.Controls.Add(txtTitulo);
            gbDatos.Controls.Add(txtDirector);
            gbDatos.Controls.Add(dtpFechaEstreno);
            gbDatos.Controls.Add(txtGenero);
            gbDatos.Controls.Add(txtDuracion);
            gbDatos.Controls.Add(txtRating);
            gbDatos.Controls.Add(txtIdioma);

            Button btnAgregar = new Button() { Name = "btnAgregar", Text = "Agregar", Location = new Point(20, 30), Width = 100, Height = 30 };
            Button btnLeer = new Button() { Name = "btnLeer", Text = "Leer", Location = new Point(20, 70), Width = 100, Height = 30 };
            Button btnActualizar = new Button() { Name = "btnActualizar", Text = "Actualizar", Location = new Point(20, 110), Width = 100, Height = 30 };
            Button btnEliminar = new Button() { Name = "btnEliminar", Text = "Eliminar", Location = new Point(20, 150), Width = 100, Height = 30 };

            gbAcciones.Controls.Add(btnAgregar);
            gbAcciones.Controls.Add(btnLeer);
            gbAcciones.Controls.Add(btnActualizar);
            gbAcciones.Controls.Add(btnEliminar);

            DataGridView dgvPeliculas = new DataGridView() { Name = "dgvPeliculas", Location = new Point(10, 320), Width = 760, Height = 220 };
            this.Controls.Add(dgvPeliculas);

            // Event Handlers
            btnAgregar.Click += BtnAgregar_Click;
            btnLeer.Click += BtnLeer_Click;
            btnActualizar.Click += BtnActualizar_Click;
            btnEliminar.Click += BtnEliminar_Click;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO peliculas (titulo, director, fecha_estreno, genero, duracion, rating, idioma) VALUES (@titulo, @director, @fecha_estreno, @genero, @duracion, @rating, @idioma)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text);
                    cmd.Parameters.AddWithValue("@director", txtDirector.Text);
                    cmd.Parameters.AddWithValue("@fecha_estreno", dtpFechaEstreno.Value);
                    cmd.Parameters.AddWithValue("@genero", txtGenero.Text);
                    cmd.Parameters.AddWithValue("@duracion", int.Parse(txtDuracion.Text));
                    cmd.Parameters.AddWithValue("@rating", decimal.Parse(txtRating.Text));
                    cmd.Parameters.AddWithValue("@idioma", txtIdioma.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Película agregada exitosamente.");
                    LeerPeliculas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void BtnLeer_Click(object sender, EventArgs e)
        {
            LeerPeliculas();
        }

        private void LeerPeliculas()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM peliculas";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvPeliculas.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvPeliculas.SelectedRows.Count > 0)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "UPDATE peliculas SET titulo = @titulo, director = @director, fecha_estreno = @fecha_estreno, genero = @genero, duracion = @duracion, rating = @rating, idioma = @idioma WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", int.Parse(dgvPeliculas.SelectedRows[0].Cells[0].Value.ToString()));
                        cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("@director", txtDirector.Text);
                        cmd.Parameters.AddWithValue("@fecha_estreno", dtpFechaEstreno.Value);
                        cmd.Parameters.AddWithValue("@genero", txtGenero.Text);
                        cmd.Parameters.AddWithValue("@duracion", int.Parse(txtDuracion.Text));
                        cmd.Parameters.AddWithValue("@rating", decimal.Parse(txtRating.Text));
                        cmd.Parameters.AddWithValue("@idioma", txtIdioma.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Película actualizada exitosamente.");
                        LeerPeliculas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila para actualizar.");
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPeliculas.SelectedRows.Count > 0)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM peliculas WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", int.Parse(dgvPeliculas.SelectedRows[0].Cells[0].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Película eliminada exitosamente.");
                        LeerPeliculas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }
    }
}
