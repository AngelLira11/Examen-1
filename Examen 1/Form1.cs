using System.Data;
using System.Windows.Forms;

namespace Examen_1
{
    public partial class Form1 : Form
    {
        //string filePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog1.FileName;
                    CargarCSV(filePath);
                }
            
        }

        private void CargarCSV(string filePath)
        {

            // Asegurar que no haya un DataSource previamente asignado
            dataGridEditor.DataSource = null;

            // Limpiar filas y columnas antes de cargar nuevos datos
            dataGridEditor.Rows.Clear();
            dataGridEditor.Columns.Clear();

            // Agregar columnas al DataGridView
            dataGridEditor.Columns.Add("CURP", "CURP");
            dataGridEditor.Columns.Add("Promedio", "Promedio");
            dataGridEditor.Columns.Add("Edad", "Edad");
            dataGridEditor.Columns.Add("Sexo", "Sexo");

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                bool primeraLinea = true; // Para omitir encabezados si los hay

                while ((line = sr.ReadLine()) != null)
                {
                    // Omitir la primera línea si es un encabezado
                    if (primeraLinea)
                    {
                        primeraLinea = false;
                        continue;
                    }

                    string[] datos = line.Split(',');

                    if (datos.Length < 2) continue; // Evita errores si faltan datos

                    string curp = datos[0].Trim();
                    string promedio = datos[1].Trim();
                    int edad = CalcularEdad(curp);
                    string sexo = DeterminarSexo(curp);

                    // Agregar los datos al DataGridView
                    dataGridEditor.Rows.Add(curp, promedio, edad, sexo);
                }
            }

            // Forzar el refresco del DataGridView
            dataGridEditor.Refresh();
        }


        private int CalcularEdad(string curp)
        {
            if (curp.Length < 18) return 0; // CURP incompleta

            string yearStr = curp.Substring(4, 2);
            string monthStr = curp.Substring(6, 2);
            string dayStr = curp.Substring(8, 2);

            int year = int.Parse(yearStr);
            int month = int.Parse(monthStr);
            int day = int.Parse(dayStr);

            // Determinar si es del 1900 o 2000
            year += (year < 30) ? 2000 : 1900;

            DateTime fechaNacimiento = new DateTime(year, month, day);
            int edad = DateTime.Today.Year - fechaNacimiento.Year;

            if (DateTime.Today < fechaNacimiento.AddYears(edad))
                edad--;

            return edad;
        }

        private string DeterminarSexo(string curp)
        {
            if (curp.Length < 11) return "Desconocido";

            char sexo = curp[10]; // Posición 11 (índice 10)
            return (sexo == 'H') ? "Hombre" : (sexo == 'M') ? "Mujer" : "Desconocido";
        }




    }
}
