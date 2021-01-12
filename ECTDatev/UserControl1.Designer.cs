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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Einnahmen", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Ausgaben", System.Windows.Forms.HorizontalAlignment.Left);
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.axEinstellung1 = new AxEASYCTXLib.AxEinstellung();
            this.axBuchung1 = new AxEASYCTXLib.AxBuchung();
            this.axDokument1 = new AxEASYCTXLib.AxDokument();
            this.listView1 = new System.Windows.Forms.ListView();
            this.datum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beleg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.beschreibung = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.netto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ust = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ustBetrag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.brutto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.Location = new System.Drawing.Point(651, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Buchungsjahr anzeigen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(653, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 1;
            // 
            // axEinstellung1
            // 
            this.axEinstellung1.Enabled = true;
            this.axEinstellung1.Location = new System.Drawing.Point(747, 391);
            this.axEinstellung1.Name = "axEinstellung1";
            this.axEinstellung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEinstellung1.OcxState")));
            this.axEinstellung1.Size = new System.Drawing.Size(32, 29);
            this.axEinstellung1.TabIndex = 2;
            // 
            // axBuchung1
            // 
            this.axBuchung1.Enabled = true;
            this.axBuchung1.Location = new System.Drawing.Point(707, 391);
            this.axBuchung1.Name = "axBuchung1";
            this.axBuchung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBuchung1.OcxState")));
            this.axBuchung1.Size = new System.Drawing.Size(34, 29);
            this.axBuchung1.TabIndex = 3;
            // 
            // axDokument1
            // 
            this.axDokument1.Enabled = true;
            this.axDokument1.Location = new System.Drawing.Point(668, 391);
            this.axDokument1.Name = "axDokument1";
            this.axDokument1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDokument1.OcxState")));
            this.axDokument1.Size = new System.Drawing.Size(33, 29);
            this.axDokument1.TabIndex = 4;
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
            this.brutto});
            listViewGroup1.Header = "Einnahmen";
            listViewGroup1.Name = "einnahmen";
            listViewGroup2.Header = "Ausgaben";
            listViewGroup2.Name = "ausgaben";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 7);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(642, 413);
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
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Location = new System.Drawing.Point(651, 61);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "<- Liste füllen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.axDokument1);
            this.Controls.Add(this.axBuchung1);
            this.Controls.Add(this.axEinstellung1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(782, 423);
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private AxEASYCTXLib.AxEinstellung axEinstellung1;
        private AxEASYCTXLib.AxBuchung axBuchung1;
        private AxEASYCTXLib.AxDokument axDokument1;

        private long m_dokID;

        [ComVisible(true)]
        public void Init(long dokID)
        {
            m_dokID = dokID;
        }

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader datum;
        private System.Windows.Forms.ColumnHeader beleg;
        private System.Windows.Forms.ColumnHeader beschreibung;
        private System.Windows.Forms.ColumnHeader netto;
        private System.Windows.Forms.ColumnHeader ust;
        private System.Windows.Forms.ColumnHeader ustBetrag;
        private System.Windows.Forms.ColumnHeader brutto;
        private System.Windows.Forms.Button button2;
    }
}
