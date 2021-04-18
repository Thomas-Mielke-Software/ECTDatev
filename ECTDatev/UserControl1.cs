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
            axDokument.ID = (int)m_dokID;
            if (axDokument.ID == 0) return;

            tbOrigin.Text = "EC";
            exportiertVon = axEinstellung.HoleEinstellung("[Persoenliche_Daten]vorname") + " " + axEinstellung.HoleEinstellung("[Persoenliche_Daten]name");
            this.tbBookingsyear_Init();
            this.InitializeDateTimePicker();
            this.ValidateButtons();
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        // OK- und Cancel-Buttons

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                bExport.PerformClick();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                bCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void bExport_Click(object sender, EventArgs e)
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

        private void bCancel_Click(object sender, EventArgs e)
        {
            IntPtr hwnd = FindWindow(null, "EasyCash&&Tax");
            SendMessage(hwnd, WM_COMMAND, ID_VIEW_JOURNAL_SWITCH, (IntPtr)0);
        }

        private void tbBookingsyear_Init()
        {
            if (m_dokID > 0)
            {
                //axDokument1.ID = (int)m_dokID;
                tbBookingsyear.Text = System.String.Format("{0}", axDokument.Jahr);
            }
        }

        // ListView füllen
        private void bFillList_Click(object sender, EventArgs e)
        {
            this.lvBookings.Items.Clear();

            if (this.exportedBuchungen.Count > 0)
            {
                this.exportedBuchungen.Clear();
            }

            var culture = new System.Globalization.CultureInfo("de-DE");  // wir brauchen DE-Format für DATEV -- unabhängig von den
                                                                          // 
                                                                          // Einnahmen
            var einnahmen = new EinnahmenBuchungen(axDokument, axBuchung);
            foreach (Buchung b in einnahmen)
            {
                // filtering by date
                if (b.Datum < this.dtpFrom.Value || this.dtpUntil.Value < b.Datum)
                {
                    continue; 
                }
                string[] row = {
                    b.Datum.ToString("dd.MM.yyyy", culture),
                    b.Belegnummer,
                    b.Beschreibung,
                    b.HoleBuchungsjahrNetto((int)m_dokID).ToString("N", culture),
                    (b.MWSt/100).ToString("0%", culture),
                    (b.Betrag - b.HoleBuchungsjahrNetto((int)m_dokID)).ToString("N", culture),
                    b.Betrag.ToString("N", culture)
                };
                var listViewItem = new ListViewItem(row);
                lvBookings.Items.Add(listViewItem);
                this.lvBookings.Groups["einnahmen"].Items.Add(listViewItem);
            }

            // Ausgaben
            var ausgaben = new AusgabenBuchungen(axDokument, axBuchung);
            foreach (Buchung b in ausgaben)
            {
                // filtering by date
                if (b.Datum < this.dtpFrom.Value || this.dtpUntil.Value < b.Datum)
                {
                    continue;
                }
                string[] row = {
                    b.Datum.ToString("dd.MM.yyyy", culture),
                    b.Belegnummer,
                    b.Beschreibung,
                    b.HoleBuchungsjahrNetto((int)m_dokID).ToString("N", culture),
                    (b.MWSt/100).ToString("0%", culture),
                    (b.Betrag - b.HoleBuchungsjahrNetto((int)m_dokID)).ToString("N", culture),
                    b.Betrag.ToString("N", culture)
                };
                var listViewItem = new ListViewItem(row);
                lvBookings.Items.Add(listViewItem);
                this.lvBookings.Groups["ausgaben"].Items.Add(listViewItem);
            }

            this.bExport.Enabled = this.lvBookings.Items.Count > 0;

#if DEBUG
            DatevHeader header = new DatevHeader(ToDo.DataCategoryID);
            System.Diagnostics.Debug.WriteLine("Header (" + ToDo.DataCategoryID.ToString() + ": " + Data.DatevFields.DataCategory[ToDo.DataCategoryID] + "):" + Environment.NewLine + header.GetHeader());
            System.Diagnostics.Debug.WriteLine("Headline (" + ToDo.DataCategoryID.ToString() + ": " + Data.DatevFields.DataCategory[ToDo.DataCategoryID] + "):" + Environment.NewLine + DatevHeadline.GetHeadline(ToDo.DataCategoryID));
#endif
        }

        private void tbBookingsyear_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Regex.IsMatch(tbOrigin.Text, @"^[A-Z][A-Z]$"))
                epOrigin.SetError(tbOrigin, "");
            else
            {
                epOrigin.SetError(tbOrigin, "Bitte zwei Großbuchstaben als Herkunftscode angeben.");
                e.Cancel = true;
            }
        }

        private void InitializeDateTimePicker()
        {
            //axDokument.ID = (int)m_dokID;
            int jahr = (int)axDokument.Jahr;

            if (jahr == 0)
            {
                jahr = DateTime.Now.Year;
            }

            // Set the MinDate, MaxDate and Value of the DTPs.
            this.dtpFrom.MinDate = new DateTime(jahr, 1, 1);
            this.dtpFrom.MaxDate = new DateTime(jahr, 12, 31);
            this.dtpFrom.Value = new DateTime(jahr, 1, 1);
            this.dtpUntil.MinDate = new DateTime(jahr, 1, 1);
            this.dtpUntil.MaxDate = new DateTime(jahr, 12, 31);
            this.dtpUntil.Value = new DateTime(jahr, 12, 31);
        }

        private void ValidateButtons(bool oneOfTheDTPsChanged = true)
        {
            if (this.dtpUntil.Value < this.dtpFrom.Value || this.dtpFrom.Value > this.dtpUntil.Value)
            {
                this.bFillList.Enabled = false;
                this.bExport.Enabled = false;
            }
            else
            {
                this.bFillList.Enabled = oneOfTheDTPsChanged;
                this.bExport.Enabled = !oneOfTheDTPsChanged && this.lvBookings.Items.Count > 0;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            this.ValidateButtons();
            this.bExport.Enabled = false;
        }

        private void dtpUntil_ValueChanged(object sender, EventArgs e)
        {
            this.ValidateButtons();
            this.bExport.Enabled = false;
        }

        private void dtpFrom_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ValidateButtons();
        }

        private void dtpUntil_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ValidateButtons();
        }

    }
}
