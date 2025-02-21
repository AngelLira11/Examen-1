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

            dataGridEditor.DataSource = null;

            dataGridEditor.Rows.Clear();
            dataGridEditor.Columns.Clear();

            dataGridEditor.Columns.Add("CURP", "CURP");
            dataGridEditor.Columns.Add("Promedio", "Promedio");
            dataGridEditor.Columns.Add("Edad", "Edad");
            dataGridEditor.Columns.Add("Sexo", "Sexo");

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                bool primeraLinea = true; 

                while ((line = sr.ReadLine()) != null)
                {
                   
                    if (primeraLinea)
                    {
                        primeraLinea = false;
                        continue;
                    }

                    string[] datos = line.Split(',');

                    if (datos.Length < 2) continue; 

                    string curp = datos[0].Trim();
                    string promedio = datos[1].Trim();
                    int edad = CalcularEdad(curp);
                    string sexo = DeterminarSexo(curp);

                    dataGridEditor.Rows.Add(curp, promedio, edad, sexo);
                }
            }

            dataGridEditor.Refresh();
        }


        private int CalcularEdad(string curp)
        {
            if (curp.Length < 18) return 0;

            string yearStr = curp.Substring(4, 2);
            string monthStr = curp.Substring(6, 2);
            string dayStr = curp.Substring(8, 2);

            int year = int.Parse(yearStr);
            int month = int.Parse(monthStr);
            int day = int.Parse(dayStr);

        
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

            char sexo = curp[10];
            return (sexo == 'H') ? "Hombre" : (sexo == 'M') ? "Mujer" : "Desconocido";
        }




    }
}
