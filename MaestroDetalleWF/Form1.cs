using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaestroDetalleWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmVenta frmVenta = new FrmVenta();
            frmVenta.ShowDialog();
            Refrescar();
        }

        private void Form1_load (object sender, EventArgs e)
        {
            Refrescar(); 
        }

        private void Refrescar()
        {
            using (Models.MaestroDetalleEntities db = new Models.MaestroDetalleEntities())
            {
                dataGridView1.DataSource = db.venta.Select(d => new {d.id,d.fecha,d.total}).ToList();
            }
        }
    }
}
