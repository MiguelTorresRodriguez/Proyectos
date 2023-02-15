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
    public partial class FrmVenta : Form
    {
        public FrmVenta()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Agregar_Click(object sender, EventArgs e)
        {
            string descripcion = txtDescripcion.Text.Trim();
            string precio = txtPrecio.Text.Trim();
            string cantidad = txtCantidad.Text.Trim();
            string importe = (decimal.Parse(cantidad) * decimal.Parse(precio)).ToString();

            dgvVentas.Rows.Add(new object[] { descripcion , precio , cantidad , importe , "Eliminar" } );

            txtDescripcion.Text= "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            txtDescripcion.Focus();

            CalcularTotal();
        }

        private void dgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex<0 || e.ColumnIndex != dgvVentas.Columns["Op"].Index)
                return;

            dgvVentas.Rows.RemoveAt(e.RowIndex);

            CalcularTotal();
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CalcularTotal()
        {
            decimal TOTAL = 0;
            foreach (DataGridViewRow dr in dgvVentas.Rows)
            {
                decimal importe = decimal.Parse(dr.Cells[3].Value.ToString());
                TOTAL += importe;
            }
            lblTotal.Text = TOTAL.ToString();
        }

        private void GenerarVenta_Click(object sender, EventArgs e)
        {
            using (Models.MaestroDetalleEntities db= new Models.MaestroDetalleEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction()) 
                {
                    try
                    {
                        Models.venta oVenta = new Models.venta();
                        oVenta.fecha = DateTime.Now;
                        oVenta.total = decimal.Parse(lblTotal.Text.ToString());

                        db.venta.Add(oVenta);
                        db.SaveChanges();

                        foreach (DataGridViewRow dr in dgvVentas.Rows)
                        {
                            Models.concepto oConcepto = new Models.concepto();
                            oConcepto.cantidad = int.Parse(dr.Cells[2].Value.ToString());
                            oConcepto.descripcion = dr.Cells[0].Value.ToString();
                            oConcepto.precioUnitario = decimal.Parse(dr.Cells[1].Value.ToString());
                            oConcepto.importe = decimal.Parse(dr.Cells[3].Value.ToString());
                            oConcepto.id_venta = oVenta.id;

                            db.concepto.Add(oConcepto);
                        }
                        db.SaveChanges();

                        dbContextTransaction.Commit();

                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}
