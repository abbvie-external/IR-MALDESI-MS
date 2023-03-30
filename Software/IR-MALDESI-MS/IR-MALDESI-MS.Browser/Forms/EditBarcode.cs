using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IR_MALDESI.Databasing;

namespace IR_MALDESI.Browser.Forms
{
    public partial class EditBarcode : Form
    {
        private Database db;
        private MALDESImethod method;

        public EditBarcode(Database database)
        {
            InitializeComponent();
            db = database;
        }

        private void EditBarcode_Load(object sender, EventArgs e)
        {
            method = db.GetMaldesiMethod();
            barcode.Text = method.Barcode;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            method.Barcode = barcode.Text;

            if (db.CurrentFile != null)
            {
                db.SetMaldesiMethod(method);

                barcode.ForeColor = default;
            }
        }

        private void ColorText(object sender, EventArgs e)
        {
            var temp = (TextBox)sender;
            temp.ForeColor = Color.Red;
        }
    }
}