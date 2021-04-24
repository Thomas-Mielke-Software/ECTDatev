﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ECTDatev.Data;
using ECTDatev.Models;

namespace ECTDatev
{
    public partial class UserControl1: UserControl
    {
        private Collection<Buchung> BookingList = new Collection<Buchung>();
        private long m_dokID;
        //private DataManager m_dataManager;

        [ComVisible(true)]
        public void Init(long dokID)
        {
            m_dokID = dokID;
            axDokument.ID = (int)m_dokID;
            if (axDokument.ID == 0) return;

            //this.m_dataManager = new DataManager(axDokument);

            
            this.InitializePG();    // PropertyGrid initialisieren

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
            if (this.BookingList.Count > 0)
            {
                this.BookingList.Clear();
            }

            // hier den Exportvorgang starten
            if (this.m_pgData.ExportSelected)
            {
                // only the selected bookings will be exported
                foreach (ListViewItem lvi in this.lvBookings.SelectedItems)
                {
                    if (lvi.Selected)
                    {
                        this.BookingList.Add((Buchung)lvi.Tag);
                    }
                }
            }
            else
            {
                // all show bookings will be exported
                foreach (ListViewItem lvi in this.lvBookings.Items)
                {
                    this.BookingList.Add((Buchung)lvi.Tag);
                }
            }
            ExportManager.Export(this.BookingList, this.m_pgData);
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

        

        // ListView füllen
        private void bFillList_Click(object sender, EventArgs e)
        {
            // find out the from and until date values

            this.lvBookings.Items.Clear();

            var culture = new System.Globalization.CultureInfo("de-DE");  // wir brauchen DE-Format für DATEV -- unabhängig von den
                                                                          // 
                                                                          // Einnahmen
            var einnahmen = new EinnahmenBuchungen(axDokument, axBuchung);
            foreach (Buchung b in einnahmen)
            {
                // filtering by date
                if (b.Datum < this.m_pgData.FromDate || this.m_pgData.UntilDate < b.Datum)
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
                listViewItem.Tag = b;
                lvBookings.Items.Add(listViewItem);
                this.lvBookings.Groups["einnahmen"].Items.Add(listViewItem);
            }

            // Ausgaben
            var ausgaben = new AusgabenBuchungen(axDokument, axBuchung);
            foreach (Buchung b in ausgaben)
            {
                // filtering by date
                if (b.Datum < this.m_pgData.FromDate || this.m_pgData.UntilDate < b.Datum)
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
                listViewItem.Tag = b;
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

        DatevPropertyItems m_pgData;
        private void InitializePG()
        {
            this.m_pgData = new DatevPropertyItems(axDokument, axEinstellung);
            this.pgDatevHeader.SelectedObject = this.m_pgData;
            this.lvBookings.MultiSelect = this.m_pgData.ExportSelected;
        }

        private void ValidateButtons(bool oneOfThedateValuesChanged = true)
        {
            if (this.m_pgData.UntilDate < this.m_pgData.FromDate || this.m_pgData.FromDate > this.m_pgData.UntilDate)
            {
                this.bFillList.Enabled = false;
                this.bExport.Enabled = false;
            }
            else
            {
                this.bFillList.Enabled = oneOfThedateValuesChanged;
                this.bExport.Enabled = !oneOfThedateValuesChanged && this.lvBookings.Items.Count > 0;
            }
        }

        private void pgDatevHeader_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.PropertyDescriptor.DisplayName)
            {
                case "Von":
                    this.ValidateButtons();
                    this.bExport.Enabled = false;
                    break;
                case "Bis":
                    this.ValidateButtons();
                    this.bExport.Enabled = false;
                    break;
                case "ExportPerSelektion":
                    this.lvBookings.MultiSelect = this.m_pgData.ExportSelected;
                    if (!this.m_pgData.ExportSelected)
                    {
                        foreach (ListViewItem lvi in this.lvBookings.SelectedItems)
                        {
                            lvi.Selected = false;
                        }
                    }
                    break;
            }
        }

        private void lvBookings_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                if (!this.m_pgData.ExportSelected)
                {
                    e.Item.Selected = false;
                }
            }
        }
    }
}
