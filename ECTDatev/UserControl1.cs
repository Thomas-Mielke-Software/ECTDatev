using System;
using System.Windows.Forms;

namespace ECTDatev
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axDokument1.ID = (int)m_dokID;
            label1.Text = System.String.Format("Buchungsjahr: {0}", axDokument1.Jahr);
        }
    }
}
