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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.axEinstellung1 = new AxEASYCTXLib.AxEinstellung();
            this.axBuchung1 = new AxEASYCTXLib.AxBuchung();
            this.axDokument1 = new AxEASYCTXLib.AxDokument();
            ((System.ComponentModel.ISupportInitialize)(this.axEinstellung1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axBuchung1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axDokument1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(391, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // axEinstellung1
            // 
            this.axEinstellung1.Enabled = true;
            this.axEinstellung1.Location = new System.Drawing.Point(700, 391);
            this.axEinstellung1.Name = "axEinstellung1";
            this.axEinstellung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axEinstellung1.OcxState")));
            this.axEinstellung1.Size = new System.Drawing.Size(32, 29);
            this.axEinstellung1.TabIndex = 2;
            // 
            // axBuchung1
            // 
            this.axBuchung1.Enabled = true;
            this.axBuchung1.Location = new System.Drawing.Point(660, 391);
            this.axBuchung1.Name = "axBuchung1";
            this.axBuchung1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axBuchung1.OcxState")));
            this.axBuchung1.Size = new System.Drawing.Size(34, 29);
            this.axBuchung1.TabIndex = 3;
            // 
            // axDokument1
            // 
            this.axDokument1.Enabled = true;
            this.axDokument1.Location = new System.Drawing.Point(621, 391);
            this.axDokument1.Name = "axDokument1";
            this.axDokument1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDokument1.OcxState")));
            this.axDokument1.Size = new System.Drawing.Size(33, 29);
            this.axDokument1.TabIndex = 4;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axDokument1);
            this.Controls.Add(this.axBuchung1);
            this.Controls.Add(this.axEinstellung1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(800, 450);
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
    }
}
