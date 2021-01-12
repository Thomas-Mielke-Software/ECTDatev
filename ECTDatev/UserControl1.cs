using System;
using System.Windows.Forms;
using ECTDatev.Models;

namespace ECTDatev
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        // Buchungsjahr anzeigen
        private void button1_Click(object sender, EventArgs e)
        {
            axDokument1.ID = (int)m_dokID;
            label1.Text = System.String.Format("Buchungsjahr: {0}", axDokument1.Jahr);
        }

        // ListView füllen
        private void button2_Click(object sender, EventArgs e)
        {
            axDokument1.ID = (int)m_dokID;
            if (axDokument1.ID == 0) return;

            // Einnahmen
            var einnahmen = new EinnahmenBuchungen(axDokument1, axBuchung1);
            foreach (Buchung b in einnahmen)
            {
                string[] row = {
                    b.Datum.ToString(),
                    b.Belegnummer,
                    b.Beschreibung,
                    b.HoleBuchungsjahrNetto((int)m_dokID).ToString(),
                    b.MWSt.ToString(),
                    (b.Betrag - b.HoleBuchungsjahrNetto((int)m_dokID)).ToString(),
                    b.Betrag.ToString()
                };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
                this.listView1.Groups["einnahmen"].Items.Add(listViewItem);
            }

            // Ausgaben
            var ausgaben = new AusgabenBuchungen(axDokument1, axBuchung1);
            foreach (Buchung b in ausgaben)
            {
                string[] row = {
                    b.Datum.ToString(),
                    b.Belegnummer,
                    b.Beschreibung,
                    b.HoleBuchungsjahrNetto((int)m_dokID).ToString(),
                    b.MWSt.ToString(),
                    (b.Betrag - b.HoleBuchungsjahrNetto((int)m_dokID)).ToString(),
                    b.Betrag.ToString()
                };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
                this.listView1.Groups["ausgaben"].Items.Add(listViewItem);
            }
        }
    }
}
