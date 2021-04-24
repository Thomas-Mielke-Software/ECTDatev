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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Einnahmen", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Ausgaben", System.Windows.Forms.HorizontalAlignment.Left);
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
            this.epOrigin = new System.Windows.Forms.ErrorProvider(this.components);
            this.bExport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.gbBuchungen = new System.Windows.Forms.GroupBox();
            this.pgDatevHeader = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epOrigin)).BeginInit();
            this.gbBuchungen.SuspendLayout();
            this.SuspendLayout();
            // 
            // axEinstellung
            // 
            this.axEinstellung.Enabled = true;
            this.axEinstellung.Location = new System.Drawing.Point(496, 476);
            this.axEinstellung.Name = "axEinstellung";
            this.axEinstellung.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEinstellung.OcxState")));
            this.axEinstellung.Size = new System.Drawing.Size(32, 29);
            this.axEinstellung.TabIndex = 2;
            // 
            // axBuchung
            // 
            this.axBuchung.Enabled = true;
            this.axBuchung.Location = new System.Drawing.Point(456, 476);
            this.axBuchung.Name = "axBuchung";
            this.axBuchung.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBuchung.OcxState")));
            this.axBuchung.Size = new System.Drawing.Size(34, 29);
            this.axBuchung.TabIndex = 3;
            // 
            // axDokument
            // 
            this.axDokument.Enabled = true;
            this.axDokument.Location = new System.Drawing.Point(417, 476);
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
            this.lvBookings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvBookings.FullRowSelect = true;
            listViewGroup1.Header = "Einnahmen";
            listViewGroup1.Name = "einnahmen";
            listViewGroup2.Header = "Ausgaben";
            listViewGroup2.Name = "ausgaben";
            this.lvBookings.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvBookings.HideSelection = false;
            this.lvBookings.Location = new System.Drawing.Point(9, 276);
            this.lvBookings.Name = "lvBookings";
            this.lvBookings.Size = new System.Drawing.Size(871, 532);
            this.lvBookings.TabIndex = 5;
            this.lvBookings.UseCompatibleStateImageBehavior = false;
            this.lvBookings.View = System.Windows.Forms.View.Details;
            this.lvBookings.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvBookings_ItemSelectionChanged);
            // 
            // datum
            // 
            this.datum.Text = "Datum";
            this.datum.Width = 70;
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
            this.netto.Width = 70;
            // 
            // ust
            // 
            this.ust.Text = "USt";
            this.ust.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ust.Width = 50;
            // 
            // ustBetrag
            // 
            this.ustBetrag.Text = "USt-Betrag";
            this.ustBetrag.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ustBetrag.Width = 70;
            // 
            // brutto
            // 
            this.brutto.Text = "Brutto";
            this.brutto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.brutto.Width = 70;
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
            this.bFillList.Location = new System.Drawing.Point(6, 29);
            this.bFillList.Name = "bFillList";
            this.bFillList.Size = new System.Drawing.Size(61, 23);
            this.bFillList.TabIndex = 6;
            this.bFillList.Text = "Anzeigen";
            this.bFillList.UseVisualStyleBackColor = true;
            this.bFillList.Click += new System.EventHandler(this.bFillList_Click);
            // 
            // epOrigin
            // 
            this.epOrigin.ContainerControl = this;
            // 
            // bExport
            // 
            this.bExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bExport.Location = new System.Drawing.Point(752, 197);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(61, 23);
            this.bExport.TabIndex = 9;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(816, 197);
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
            this.gbBuchungen.Controls.Add(this.bFillList);
            this.gbBuchungen.Location = new System.Drawing.Point(752, 25);
            this.gbBuchungen.Name = "gbBuchungen";
            this.gbBuchungen.Size = new System.Drawing.Size(128, 78);
            this.gbBuchungen.TabIndex = 11;
            this.gbBuchungen.TabStop = false;
            this.gbBuchungen.Text = "Buchungen";
            // 
            // pgDatevHeader
            // 
            this.pgDatevHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgDatevHeader.Location = new System.Drawing.Point(9, 0);
            this.pgDatevHeader.Name = "pgDatevHeader";
            this.pgDatevHeader.Size = new System.Drawing.Size(733, 270);
            this.pgDatevHeader.TabIndex = 13;
            this.pgDatevHeader.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgDatevHeader_PropertyValueChanged);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.pgDatevHeader);
            this.Controls.Add(this.gbBuchungen);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.lvBookings);
            this.Controls.Add(this.axDokument);
            this.Controls.Add(this.axBuchung);
            this.Controls.Add(this.axEinstellung);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(892, 819);
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epOrigin)).EndInit();
            this.gbBuchungen.ResumeLayout(false);
            this.gbBuchungen.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
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
        private System.Windows.Forms.ErrorProvider epOrigin;
        private System.Windows.Forms.ColumnHeader exportStatus;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.GroupBox gbBuchungen;
        private System.Windows.Forms.PropertyGrid pgDatevHeader;
    }
}
