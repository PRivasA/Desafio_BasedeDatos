using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ejemplo_base_de_datos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   public partial class MainWindow : Window
    {
        private List<Alumno> personas;
        private int indiceseleccionado;
        public MainWindow()
        {
            personas = new List<Alumno>();
            indiceseleccionado = -1;
        }
        private void MostrarDatos()
        {
            try
            {
                //Crear una lista para manejar la lista en memoria
                List<Alumno> lista = new List<Alumno>();
                //obtenemos la conexion a la BD
                SqlConnection con = CrearConexion();
                //Creamos un comando para consultar la informacion en la tabla
                SqlCommand cmd = new SqlCommand("Select Id, Nombre, Direccion, Telefono, Carnet, Edad From Alumno", con);
                //abrir conexion
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Alumno alumno = new Alumno
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Direccion = reader.GetString(2),
                            Telefono = reader.GetString(3),
                            Carnet = reader.GetString(4),
                            Edad = reader.GetInt32(5),
                        };
                        lista.Add(alumno);
                    }
                }
                ListItemAlumno.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al consultar datos " + ex.Message);
            }
        }
        private SqlConnection CrearConexion()
        {
            return new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\leslie\\source\\repos\\Base de Datos clase\\Base de Datos clase\\Database1.mdf\";Integrated Security=True");
        }

        private void btnGuardar_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener los valores ingresados en los TextBoxes
                int id = Convert.ToInt32(txtId.Text);
                string nombre = txtNombre.Text;
                string direccion = txtDireccion.Text;
                string telefono = txtTelefono.Text;
                string carnet = txtCarnet.Text;
                int edad = Convert.ToInt32(txtEdad.Text);

                // Crear una conexión a la base de datos
                using (SqlConnection con = CrearConexion())
                {
                    // Crear un comando para insertar los datos en la tabla
                    SqlCommand cmd = new SqlCommand("INSERT INTO Alumno (Id, Nombre, Direccion, Telefono, Carnet, Edad) VALUES (@Id, @Nombre, @Direccion, @Telefono, @Carnet, @Edad)", con);
                    // Asignar los valores de los parámetros
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Direccion", direccion);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@Carnet", carnet);
                    cmd.Parameters.AddWithValue("@Edad", edad);
                    // Abrir la conexión
                    con.Open();
                    // Ejecutar el comando
                    cmd.ExecuteNonQuery();
                }

                // Actualizar la lista en el ListBox
                MostrarDatos();

                // Limpiar los TextBoxes
                txtId.Text = "";
                txtNombre.Text = "";
                txtDireccion.Text = "";
                txtTelefono.Text = "";
                txtCarnet.Text = "";
                txtEdad.Text = "";

                MessageBox.Show("Registro guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el registro: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si hay un elemento seleccionado en el ListBox
                if (ListItemAlumno.SelectedItem != null)
                {
                    // Obtener el objeto Alumno seleccionado en el ListBox
                    Alumno alumnoSeleccionado = (Alumno)ListItemAlumno.SelectedItem;
                    int id = alumnoSeleccionado.Id;

                    // Crear una conexión a la base de datos
                    using (SqlConnection con = CrearConexion())
                    {
                        // Crear un comando para eliminar el registro de la tabla
                        SqlCommand cmd = new SqlCommand("DELETE FROM Alumno WHERE Id = @Id", con);
                        // Asignar el valor del parámetro
                        cmd.Parameters.AddWithValue("@Id", id);
                        // Abrir la conexión
                        con.Open();
                        // Ejecutar el comando
                        cmd.ExecuteNonQuery();
                    }

                    // Actualizar la lista en el ListBox
                    MostrarDatos();

                    MessageBox.Show("Registro eliminado correctamente.");
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un alumno de la lista.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al eliminar el registro: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si hay un elemento seleccionado en el ListBox
                if (ListItemAlumno.SelectedItem != null)
                {
                    // Obtener el objeto Alumno seleccionado en el ListBox
                    Alumno alumnoSeleccionado = (Alumno)ListItemAlumno.SelectedItem;
                    int id = alumnoSeleccionado.Id;

                    // Obtener los nuevos valores ingresados en los TextBoxes
                    string nombre = txtNombre.Text;
                    string direccion = txtDireccion.Text;
                    string telefono = txtTelefono.Text;
                    string carnet = txtCarnet.Text;
                    int edad = Convert.ToInt32(txtEdad.Text);

                    // Crear una conexión a la base de datos
                    using (SqlConnection con = CrearConexion())
                    {
                        // Crear un comando para actualizar el registro en la tabla
                        SqlCommand cmd = new SqlCommand("UPDATE Alumno SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Carnet = @Carnet, Edad = @Edad WHERE Id = @Id", con);
                        // Asignar los valores de los parámetros
                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        cmd.Parameters.AddWithValue("@Direccion", direccion);
                        cmd.Parameters.AddWithValue("@Telefono", telefono);
                        cmd.Parameters.AddWithValue("@Carnet", carnet);
                        cmd.Parameters.AddWithValue("@Edad", edad);
                        cmd.Parameters.AddWithValue("@Id", id);
                        // Abrir la conexión
                        con.Open();
                        // Ejecutar el comando
                        cmd.ExecuteNonQuery();
                    }

                    // Actualizar la lista en el ListBox
                    MostrarDatos();

                    MessageBox.Show("Registro actualizado correctamente.");
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un alumno de la lista.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al actualizar el registro: " + ex.Message);
            }
        }

        private void txtDireccion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtCarnet_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtEdad_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
    }
}
