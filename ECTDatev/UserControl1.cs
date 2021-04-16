using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ECTDatev.Data;
using ECTDatev.Models;

namespace ECTDatev
{
    public partial class UserControl1: UserControl
    {
        private string exportiertVon;  // z.B. "Max Mustermann"
        private Dictionary<int, Buchung> exportedBuchungen = new Dictionary<int, Buchung>();

        // Initialisierung

        private long m_dokID;

        [ComVisible(true)]
        public void Init(long dokID)
        {
            m_dokID = dokID;
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            herkunftTextbox.Text = "EC";
            exportiertVon = axEinstellung1.HoleEinstellung("[Persoenliche_Daten]vorname") + " " + axEinstellung1.HoleEinstellung("[Persoenliche_Daten]name");
            this.tbBuchungsjahr_Init();
            this.InitializeDateTimePicker();
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
            ExportManager.OrderExport(this.exportedBuchungen);
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

        private void tbBuchungsjahr_Init()
        {
            if (m_dokID > 0)
            {
                axDokument1.ID = (int)m_dokID;
                tbBuchungsjahr.Text = System.String.Format("Buchungsjahr: {0}", axDokument1.Jahr);
            }
        }

        // ListView füllen
        private void button2_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();

            axDokument1.ID = (int)m_dokID;
            if (axDokument1.ID == 0) return;

            if (this.exportedBuchungen.Count > 0)
            {
                this.exportedBuchungen.Clear();
            }

            // Einnahmen
            var einnahmen = new EinnahmenBuchungen(axDokument1, axBuchung1);
            foreach (Buchung b in einnahmen)
            {
                if (b.Datum < dtpFrom.Value || b.Datum > dtpUntil.Value)
                {
                    // this is the filtering
                    continue;
                }
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

                this.exportedBuchungen.Add(this.exportedBuchungen.Count + 1, b);
            }

            // Ausgaben
            var ausgaben = new AusgabenBuchungen(axDokument1, axBuchung1);
            foreach (Buchung b in ausgaben)
            {
                if (b.Datum < dtpFrom.Value || b.Datum > dtpUntil.Value)
                {
                    // this is the filtering
                    continue;
                }
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

                this.exportedBuchungen.Add(this.exportedBuchungen.Count + 1, b);
            }

            this.okButton.Enabled = true;

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

        private void InitializeDateTimePicker()
        {
            axDokument1.ID = (int)m_dokID;
            int jahr = (int)axDokument1.Jahr;

            if (jahr == 0)
            {
                jahr = DateTime.Now.Year;
            }

            // Set the MinDate and MaxDate.
            this.dtpFrom.MinDate = new DateTime(jahr, 1, 1);
            this.dtpFrom.MaxDate = new DateTime(jahr, 12, 31);
            this.dtpFrom.Value = new DateTime(jahr, 1, 1);
            this.dtpUntil.MinDate = new DateTime(jahr, 1, 1);
            this.dtpUntil.MaxDate = new DateTime(jahr, 12, 31);
            this.dtpUntil.Value = new DateTime(jahr, 12, 31);
        }

        private void dtpFrom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.dtpUntil.Value < this.dtpFrom.Value || this.dtpFrom.Value > this.dtpUntil.Value)
            {
                this.epFrom.SetError(this.dtpFrom, "Das Von-Datum darf nicht nach dem Bis-Datum liegen!");

                this.okButton.Enabled = false;
                this.button2.Enabled = false;

                e.Cancel = true;
            }
            else
            {
                this.epFrom.SetError(this.dtpFrom, string.Empty);
                this.okButton.Enabled = true;
                this.button2.Enabled = true;

                e.Cancel = false;
            }
        }

        private void dtpUntil_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.dtpUntil.Value < this.dtpFrom.Value || this.dtpFrom.Value > this.dtpUntil.Value)
            {
                this.epUntil.SetError(this.dtpUntil, "Das Von-Datum darf nicht nach dem Bis-Datum liegen!");

                this.okButton.Enabled = false;
                this.button2.Enabled = false;

                e.Cancel = true;
            }
            else
            {
                this.epUntil.SetError(this.dtpUntil, string.Empty);

                this.okButton.Enabled = true;
                this.button2.Enabled = true;

                e.Cancel = false;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = false;
        }

        private void dtpUntil_ValueChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = false;
        }
    }
}
