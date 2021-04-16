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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Einnahmen", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Ausgaben", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.lBuchungsjahr = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.datum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beleg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beschreibung = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.netto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ust = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ustBetrag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.brutto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.exportStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            this.herkunftLabel = new System.Windows.Forms.Label();
            this.herkunftTextbox = new System.Windows.Forms.TextBox();
            this.herkunftErrorprov = new System.Windows.Forms.ErrorProvider(this.components);
            this.okButton = new System.Windows.Forms.Button();
            this.abbrechenButton = new System.Windows.Forms.Button();
            this.gbBuchungen = new System.Windows.Forms.GroupBox();
            this.dtpUntil = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lUntil = new System.Windows.Forms.Label();
            this.lFrom = new System.Windows.Forms.Label();
            this.epFrom = new System.Windows.Forms.ErrorProvider(this.components);
            this.epUntil = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbBuchungsjahr = new System.Windows.Forms.TextBox();
            this.axDokument1 = new AxEASYCTXLib.AxDokument();
            this.axBuchung1 = new AxEASYCTXLib.AxBuchung();
            this.axEinstellung1 = new AxEASYCTXLib.AxEinstellung();
            ((System.ComponentModel.ISupportInitialize)(this.herkunftErrorprov)).BeginInit();
            this.gbBuchungen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epUntil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung1)).BeginInit();
            this.SuspendLayout();
            // 
            // lBuchungsjahr
            // 
            this.lBuchungsjahr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lBuchungsjahr.AutoSize = true;
            this.lBuchungsjahr.Location = new System.Drawing.Point(755, 38);
            this.lBuchungsjahr.Name = "lBuchungsjahr";
            this.lBuchungsjahr.Size = new System.Drawing.Size(72, 13);
            this.lBuchungsjahr.TabIndex = 1;
            this.lBuchungsjahr.Text = "Buchungsjahr";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.datum,
            this.beleg,
            this.beschreibung,
            this.netto,
            this.ust,
            this.ustBetrag,
            this.brutto,
            this.exportStatus});
            listViewGroup1.Header = "Einnahmen";
            listViewGroup1.Name = "einnahmen";
            listViewGroup2.Header = "Ausgaben";
            listViewGroup2.Name = "ausgaben";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(10, 276);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(743, 311);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            // 
            // ust
            // 
            this.ust.Text = "USt";
            // 
            // ustBetrag
            // 
            this.ustBetrag.Text = "USt-Betrag";
            // 
            // brutto
            // 
            this.brutto.Text = "Brutto";
            // 
            // exportStatus
            // 
            this.exportStatus.Text = "Export-Status";
            this.exportStatus.Width = 200;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Location = new System.Drawing.Point(5, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "<- Liste füllen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // herkunftLabel
            // 
            this.herkunftLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.herkunftLabel.AutoSize = true;
            this.herkunftLabel.Location = new System.Drawing.Point(756, 67);
            this.herkunftLabel.Name = "herkunftLabel";
            this.herkunftLabel.Size = new System.Drawing.Size(48, 13);
            this.herkunftLabel.TabIndex = 7;
            this.herkunftLabel.Text = "Herkunft";
            // 
            // herkunftTextbox
            // 
            this.herkunftTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.herkunftTextbox.Location = new System.Drawing.Point(849, 64);
            this.herkunftTextbox.MaxLength = 2;
            this.herkunftTextbox.Name = "herkunftTextbox";
            this.herkunftTextbox.Size = new System.Drawing.Size(32, 20);
            this.herkunftTextbox.TabIndex = 8;
            this.herkunftTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.herkunftTextbox_Validating);
            // 
            // herkunftErrorprov
            // 
            this.herkunftErrorprov.ContainerControl = this;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(759, 564);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(55, 23);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // abbrechenButton
            // 
            this.abbrechenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abbrechenButton.Location = new System.Drawing.Point(817, 564);
            this.abbrechenButton.Name = "abbrechenButton";
            this.abbrechenButton.Size = new System.Drawing.Size(67, 23);
            this.abbrechenButton.TabIndex = 10;
            this.abbrechenButton.Text = "Abbrechen";
            this.abbrechenButton.UseVisualStyleBackColor = true;
            this.abbrechenButton.Click += new System.EventHandler(this.abbrechenButton_Click);
            // 
            // gbBuchungen
            // 
            this.gbBuchungen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBuchungen.Controls.Add(this.dtpUntil);
            this.gbBuchungen.Controls.Add(this.dtpFrom);
            this.gbBuchungen.Controls.Add(this.lUntil);
            this.gbBuchungen.Controls.Add(this.lFrom);
            this.gbBuchungen.Controls.Add(this.button2);
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
            // epFrom
            // 
            this.epFrom.ContainerControl = this;
            // 
            // epUntil
            // 
            this.epUntil.ContainerControl = this;
            // 
            // tbBuchungsjahr
            // 
            this.tbBuchungsjahr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBuchungsjahr.Location = new System.Drawing.Point(833, 35);
            this.tbBuchungsjahr.Name = "tbBuchungsjahr";
            this.tbBuchungsjahr.ReadOnly = true;
            this.tbBuchungsjahr.Size = new System.Drawing.Size(48, 20);
            this.tbBuchungsjahr.TabIndex = 12;
            // 
            // axDokument1
            // 
            this.axDokument1.Enabled = true;
            this.axDokument1.Location = new System.Drawing.Point(754, 340);
            this.axDokument1.Name = "axDokument1";
            this.axDokument1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDokument1.OcxState")));
            this.axDokument1.Size = new System.Drawing.Size(33, 29);
            this.axDokument1.TabIndex = 4;
            // 
            // axBuchung1
            // 
            this.axBuchung1.Enabled = true;
            this.axBuchung1.Location = new System.Drawing.Point(793, 340);
            this.axBuchung1.Name = "axBuchung1";
            this.axBuchung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBuchung1.OcxState")));
            this.axBuchung1.Size = new System.Drawing.Size(34, 29);
            this.axBuchung1.TabIndex = 3;
            // 
            // axEinstellung1
            // 
            this.axEinstellung1.Enabled = true;
            this.axEinstellung1.Location = new System.Drawing.Point(833, 340);
            this.axEinstellung1.Name = "axEinstellung1";
            this.axEinstellung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEinstellung1.OcxState")));
            this.axEinstellung1.Size = new System.Drawing.Size(32, 29);
            this.axEinstellung1.TabIndex = 2;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbBuchungsjahr);
            this.Controls.Add(this.gbBuchungen);
            this.Controls.Add(this.abbrechenButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.herkunftTextbox);
            this.Controls.Add(this.herkunftLabel);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.axDokument1);
            this.Controls.Add(this.axBuchung1);
            this.Controls.Add(this.axEinstellung1);
            this.Controls.Add(this.lBuchungsjahr);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(884, 723);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.herkunftErrorprov)).EndInit();
            this.gbBuchungen.ResumeLayout(false);
            this.gbBuchungen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.epFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epUntil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lBuchungsjahr;
        private AxEASYCTXLib.AxEinstellung axEinstellung1;
        private AxEASYCTXLib.AxBuchung axBuchung1;
        private AxEASYCTXLib.AxDokument axDokument1;

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader datum;
        private System.Windows.Forms.ColumnHeader beleg;
        private System.Windows.Forms.ColumnHeader beschreibung;
        private System.Windows.Forms.ColumnHeader netto;
        private System.Windows.Forms.ColumnHeader ust;
        private System.Windows.Forms.ColumnHeader ustBetrag;
        private System.Windows.Forms.ColumnHeader brutto;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label herkunftLabel;
        private System.Windows.Forms.TextBox herkunftTextbox;
        private System.Windows.Forms.ErrorProvider herkunftErrorprov;
        private System.Windows.Forms.ColumnHeader exportStatus;
        private System.Windows.Forms.Button abbrechenButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox gbBuchungen;
        private System.Windows.Forms.Label lFrom;
        private System.Windows.Forms.Label lUntil;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpUntil;
        private System.Windows.Forms.ErrorProvider epFrom;
        private System.Windows.Forms.ErrorProvider epUntil;
        private System.Windows.Forms.TextBox tbBuchungsjahr;
    }
}
