using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ECTDatev.Models;

namespace ECTDatev
{
    public partial class UserControl1: UserControl
    {
        private string exportiertVon;  // z.B. "Max Mustermann"

        // Initialisierung

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            herkunftTextbox.Text = "EC";
            exportiertVon = axEinstellung1.HoleEinstellung("[Persoenliche_Daten]vorname") + " " + axEinstellung1.HoleEinstellung("[Persoenliche_Daten]name");
        }

        // OK- und Cancel-Buttons

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                okButton.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                abbrechenButton.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // hier den Exportvorgang starten
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, String lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        const int WM_COMMAND = 0x0111;
        IntPtr ID_VIEW_JOURNAL_SWITCH = (IntPtr)32788;
        private void abbrechenButton_Click(object sender, EventArgs e)
        {
            IntPtr hwnd = FindWindow(null, "EasyCash&&Tax");
            SendMessage(hwnd, WM_COMMAND, ID_VIEW_JOURNAL_SWITCH, (IntPtr)0);
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

#if DEBUG
            DatevHeader header = new DatevHeader(ToDo.DataCategoryID);
            System.Diagnostics.Debug.WriteLine("Header (" + ToDo.DataCategoryID.ToString() + ": " + Data.DatevFields.DataCategory[ToDo.DataCategoryID] + "):" + Environment.NewLine + header.GetHeader());
            System.Diagnostics.Debug.WriteLine("Headline (" + ToDo.DataCategoryID.ToString() + ": " + Data.DatevFields.DataCategory[ToDo.DataCategoryID] + "):" + Environment.NewLine + DatevHeadline.GetHeadline(ToDo.DataCategoryID));
#endif
        }

        private void herkunftTextbox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Regex.IsMatch(herkunftTextbox.Text, @"^[A-Z][A-Z]$"))
                herkunftErrorprov.SetError(herkunftTextbox, "");
            else
            {
                herkunftErrorprov.SetError(herkunftTextbox, "Bitte zwei Großbuchstaben als Herkunftscode angeben.");
                e.Cancel = true;
            }
        }

    }
}
