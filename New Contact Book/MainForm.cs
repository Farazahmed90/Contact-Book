using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_Contact_Book
{
    public partial class MainForm : MaterialSkin.Controls.MaterialForm
    {
        string fileName = string.Format("{0}//data.dat", Application.StartupPath);
        public MainForm()
        {
            InitializeComponent();
        }
        private void Newbutton_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true;
                App.PhoneBook.AddPhoneBookRow(App.PhoneBook.NewPhoneBookRow());
                phoneBookBindingSource.MoveLast();
                nametextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Contact Book ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.PhoneBook.RejectChanges();
            }
        }
        private void Editbutton_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            nametextBox.Focus();
        }
        private void nametextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            nametextBox.CharacterCasing = CharacterCasing.Lower;
        }
        private void numbertextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                MessageBox.Show("Enter Numbers Only", "Contact Book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numbertextBox.Text = "";
            }
        }
        private void savebutton_Click(object sender, EventArgs e)
        {
            if (nametextBox.Text == "" || numbertextBox.Text == "")
            {
                MessageBox.Show("Action can not be Completed ", "Contact Book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nametextBox.Focus();
            }
            else
            {
                try
                {
                    phoneBookBindingSource.EndEdit();
                    App.PhoneBook.AcceptChanges();
                    App.PhoneBook.WriteXml(fileName);
                    MessageBox.Show("Contact Saved Successfully!!", "Contact Book", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panel1.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Contact Book", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    App.PhoneBook.RejectChanges();
                }
            }
        }
        static AppData db;
        protected static AppData App
        {
            get
            {
                if (db == null)
                    db = new AppData();
                return db;
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(fileName))
            App.PhoneBook.ReadXml(fileName);
            phoneBookBindingSource.DataSource = App.PhoneBook;
            panel1.Enabled = false;
        }    
        private void searchtextBox_TextChanged(object sender, EventArgs e)
        {
            searchtextBox.CharacterCasing = CharacterCasing.Lower;
            Search_Function();
        } 
        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                Delete_Function();
            }
        }
        private void Delbutton_Click(object sender, EventArgs e)
        {
            Delete_Function();
        }
        private void Clearallbutton_Click(object sender, EventArgs e)
        {
            DialogResult question;
            question = MessageBox.Show("Confirm you want to delete all record?", "Contact Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.Yes)
            {
                phoneBookBindingSource.EndEdit();
                App.PhoneBook.AcceptChanges();
                App.PhoneBook.Clear();
                App.PhoneBook.WriteXml(fileName);
            }
        }
        private void Cancelbutton_Click(object sender, EventArgs e)
        {

            phoneBookBindingSource.ResetBindings(false);
            phoneBookBindingSource.RemoveCurrent();
            panel1.Enabled = false;
        } 

        private void Delete_Function()         //method for delete (starts)
        {
            DialogResult question;
            question = MessageBox.Show("Confirm you want to delete this record?", "Contact Book", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.Yes)
            {
                phoneBookBindingSource.EndEdit();
                App.PhoneBook.AcceptChanges();
                phoneBookBindingSource.RemoveCurrent();
                App.PhoneBook.WriteXml(fileName);
            }
        }                                      //method for delete (Ends)

        private void Search_Function()         //method for search (starts)
        {
            if (!string.IsNullOrEmpty(searchtextBox.Text))
            {
                var query = from o in App.PhoneBook
                            where o.PhoneNumber.Contains(searchtextBox.Text) || o.Name.Contains(searchtextBox.Text)
                            select o;
                dataGridView.DataSource = query.ToList();
            }
            else
                dataGridView.DataSource = phoneBookBindingSource;
        }                                      //method for search (Ends)                                      

        private void nametextBox_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
