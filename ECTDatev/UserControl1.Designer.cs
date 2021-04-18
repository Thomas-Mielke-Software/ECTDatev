using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ECTDatev
{
    [ProgId("ECTDatev.UserControl1")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("EB018E1F-1E82-448F-AFC7-90E509A93ACB")]
    [ComVisible(true)]
    partial class UserControl1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        ///<summary>
        ///Register the class as a control and set its CodeBase entry
        ///</summary>
        ///<param name="key">The registry key of the control</param>
        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            // System.Windows.Forms.MessageBox.Show("Registriert!");
        }

        ///<summary>
        ///Called to unregister the control
        ///</summary>
        ///<param name="key">The registry key</param>
        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            // System.Windows.Forms.MessageBox.Show("Deregistriert!");
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Einnahmen", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Ausgaben", System.Windows.Forms.HorizontalAlignment.Left);
            this.lBookingsyear = new System.Windows.Forms.Label();
            this.axEinstellung = new AxEASYCTXLib.AxEinstellung();
            this.axBuchung = new AxEASYCTXLib.AxBuchung();
            this.axDokument = new AxEASYCTXLib.AxDokument();
            this.lvBookings = new System.Windows.Forms.ListView();
            this.datum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beleg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beschreibung = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.netto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ust = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ustBetrag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.brutto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exportStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bFillList = new System.Windows.Forms.Button();
            this.lOrigin = new System.Windows.Forms.Label();
            this.tbOrigin = new System.Windows.Forms.TextBox();
            this.epOrigin = new System.Windows.Forms.ErrorProvider(this.components);
            this.bExport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.gbBuchungen = new System.Windows.Forms.GroupBox();
            this.dtpUntil = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lUntil = new System.Windows.Forms.Label();
            this.lFrom = new System.Windows.Forms.Label();
            this.tbBookingsyear = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epOrigin)).BeginInit();
            this.gbBuchungen.SuspendLayout();
            this.SuspendLayout();
            // 
            // lBookingsyear
            // 
            this.lBookingsyear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lBookingsyear.AutoSize = true;
            this.lBookingsyear.Location = new System.Drawing.Point(755, 38);
            this.lBookingsyear.Name = "lBookingsyear";
            this.lBookingsyear.Size = new System.Drawing.Size(72, 13);
            this.lBookingsyear.TabIndex = 1;
            this.lBookingsyear.Text = "Buchungsjahr";
            // 
            // axEinstellung
            // 
            this.axEinstellung.Enabled = true;
            this.axEinstellung.Location = new System.Drawing.Point(833, 340);
            this.axEinstellung.Name = "axEinstellung";
            this.axEinstellung.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEinstellung.OcxState")));
            this.axEinstellung.Size = new System.Drawing.Size(32, 29);
            this.axEinstellung.TabIndex = 2;
            // 
            // axBuchung
            // 
            this.axBuchung.Enabled = true;
            this.axBuchung.Location = new System.Drawing.Point(793, 340);
            this.axBuchung.Name = "axBuchung";
            this.axBuchung.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBuchung.OcxState")));
            this.axBuchung.Size = new System.Drawing.Size(34, 29);
            this.axBuchung.TabIndex = 3;
            // 
            // axDokument
            // 
            this.axDokument.Enabled = true;
            this.axDokument.Location = new System.Drawing.Point(754, 340);
            this.axDokument.Name = "axDokument";
            this.axDokument.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDokument.OcxState")));
            this.axDokument.Size = new System.Drawing.Size(33, 29);
            this.axDokument.TabIndex = 4;
            // 
            // lvBookings
            // 
            this.lvBookings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBookings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.datum,
            this.beleg,
            this.beschreibung,
            this.netto,
            this.ust,
            this.ustBetrag,
            this.brutto,
            this.exportStatus});
            listViewGroup3.Header = "Einnahmen";
            listViewGroup3.Name = "einnahmen";
            listViewGroup4.Header = "Ausgaben";
            listViewGroup4.Name = "ausgaben";
            this.lvBookings.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup3,
            listViewGroup4});
            this.lvBookings.HideSelection = false;
            this.lvBookings.Location = new System.Drawing.Point(10, 276);
            this.lvBookings.Name = "lvBookings";
            this.lvBookings.Size = new System.Drawing.Size(743, 311);
            this.lvBookings.TabIndex = 5;
            this.lvBookings.UseCompatibleStateImageBehavior = false;
            this.lvBookings.View = System.Windows.Forms.View.Details;
            // 
            // datum
            // 
            this.datum.Text = "Datum";
            // 
            // beleg
            // 
            this.beleg.Text = "Beleg";
            // 
            // beschreibung
            // 
            this.beschreibung.Text = "Beschreibung";
            this.beschreibung.Width = 300;
            // 
            // netto
            // 
            this.netto.Text = "Netto";
            this.netto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ust
            // 
            this.ust.Text = "USt";
            this.ust.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ustBetrag
            // 
            this.ustBetrag.Text = "USt-Betrag";
            this.ustBetrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // brutto
            // 
            this.brutto.Text = "Brutto";
            this.brutto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // exportStatus
            // 
            this.exportStatus.Text = "Export-Status";
            this.exportStatus.Width = 200;
            // 
            // bFillList
            // 
            this.bFillList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFillList.AutoSize = true;
            this.bFillList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bFillList.Location = new System.Drawing.Point(5, 136);
            this.bFillList.Name = "bFillList";
            this.bFillList.Size = new System.Drawing.Size(79, 23);
            this.bFillList.TabIndex = 6;
            this.bFillList.Text = "<- Liste füllen";
            this.bFillList.UseVisualStyleBackColor = true;
            this.bFillList.Click += new System.EventHandler(this.bFillList_Click);
            // 
            // lOrigin
            // 
            this.lOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lOrigin.AutoSize = true;
            this.lOrigin.Location = new System.Drawing.Point(756, 67);
            this.lOrigin.Name = "lOrigin";
            this.lOrigin.Size = new System.Drawing.Size(48, 13);
            this.lOrigin.TabIndex = 7;
            this.lOrigin.Text = "Herkunft";
            // 
            // tbOrigin
            // 
            this.tbOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOrigin.Location = new System.Drawing.Point(849, 64);
            this.tbOrigin.MaxLength = 2;
            this.tbOrigin.Name = "tbOrigin";
            this.tbOrigin.Size = new System.Drawing.Size(32, 20);
            this.tbOrigin.TabIndex = 8;
            this.tbOrigin.Validating += new System.ComponentModel.CancelEventHandler(this.tbBookingsyear_Validating);
            // 
            // epOrigin
            // 
            this.epOrigin.ContainerControl = this;
            // 
            // bExport
            // 
            this.bExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bExport.Location = new System.Drawing.Point(759, 564);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(55, 23);
            this.bExport.TabIndex = 9;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(817, 564);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(67, 23);
            this.bCancel.TabIndex = 10;
            this.bCancel.Text = "Abbrechen";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // gbBuchungen
            // 
            this.gbBuchungen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBuchungen.Controls.Add(this.dtpUntil);
            this.gbBuchungen.Controls.Add(this.dtpFrom);
            this.gbBuchungen.Controls.Add(this.lUntil);
            this.gbBuchungen.Controls.Add(this.lFrom);
            this.gbBuchungen.Controls.Add(this.bFillList);
            this.gbBuchungen.Location = new System.Drawing.Point(753, 140);
            this.gbBuchungen.Name = "gbBuchungen";
            this.gbBuchungen.Size = new System.Drawing.Size(128, 166);
            this.gbBuchungen.TabIndex = 11;
            this.gbBuchungen.TabStop = false;
            this.gbBuchungen.Text = "Buchungen";
            // 
            // dtpUntil
            // 
            this.dtpUntil.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUntil.Location = new System.Drawing.Point(6, 92);
            this.dtpUntil.Name = "dtpUntil";
            this.dtpUntil.Size = new System.Drawing.Size(87, 20);
            this.dtpUntil.TabIndex = 3;
            this.dtpUntil.ValueChanged += new System.EventHandler(this.dtpUntil_ValueChanged);
            this.dtpUntil.Validating += new System.ComponentModel.CancelEventHandler(this.dtpUntil_Validating);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(6, 38);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(87, 20);
            this.dtpFrom.TabIndex = 2;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.Validating += new System.ComponentModel.CancelEventHandler(this.dtpFrom_Validating);
            // 
            // lUntil
            // 
            this.lUntil.AutoSize = true;
            this.lUntil.Location = new System.Drawing.Point(3, 76);
            this.lUntil.Name = "lUntil";
            this.lUntil.Size = new System.Drawing.Size(24, 13);
            this.lUntil.TabIndex = 1;
            this.lUntil.Text = "Bis:";
            // 
            // lFrom
            // 
            this.lFrom.AutoSize = true;
            this.lFrom.Location = new System.Drawing.Point(2, 22);
            this.lFrom.Name = "lFrom";
            this.lFrom.Size = new System.Drawing.Size(29, 13);
            this.lFrom.TabIndex = 0;
            this.lFrom.Text = "Von:";
            // 
            // tbBookingsyear
            // 
            this.tbBookingsyear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBookingsyear.Location = new System.Drawing.Point(833, 35);
            this.tbBookingsyear.Name = "tbBookingsyear";
            this.tbBookingsyear.ReadOnly = true;
            this.tbBookingsyear.Size = new System.Drawing.Size(48, 20);
            this.tbBookingsyear.TabIndex = 12;
            // 
            // ucForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbBookingsyear);
            this.Controls.Add(this.gbBuchungen);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.tbOrigin);
            this.Controls.Add(this.lOrigin);
            this.Controls.Add(this.lvBookings);
            this.Controls.Add(this.axDokument);
            this.Controls.Add(this.axBuchung);
            this.Controls.Add(this.axEinstellung);
            this.Controls.Add(this.lBookingsyear);
            this.Name = "ucForm";
            this.Size = new System.Drawing.Size(884, 723);
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epOrigin)).EndInit();
            this.gbBuchungen.ResumeLayout(false);
            this.gbBuchungen.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lBookingsyear;
        private AxEASYCTXLib.AxEinstellung axEinstellung;
        private AxEASYCTXLib.AxBuchung axBuchung;
        private AxEASYCTXLib.AxDokument axDokument;

        private System.Windows.Forms.ListView lvBookings;
        private System.Windows.Forms.ColumnHeader datum;
        private System.Windows.Forms.ColumnHeader beleg;
        private System.Windows.Forms.ColumnHeader beschreibung;
        private System.Windows.Forms.ColumnHeader netto;
        private System.Windows.Forms.ColumnHeader ust;
        private System.Windows.Forms.ColumnHeader ustBetrag;
        private System.Windows.Forms.ColumnHeader brutto;
        private System.Windows.Forms.Button bFillList;
        private System.Windows.Forms.Label lOrigin;
        private System.Windows.Forms.TextBox tbOrigin;
        private System.Windows.Forms.ErrorProvider epOrigin;
        private System.Windows.Forms.ColumnHeader exportStatus;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.GroupBox gbBuchungen;
        private System.Windows.Forms.Label lFrom;
        private System.Windows.Forms.Label lUntil;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpUntil;
        private System.Windows.Forms.TextBox tbBookingsyear;
    }
}
