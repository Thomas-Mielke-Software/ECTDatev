using AxEASYCTXLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECTDatev.Data
{
    public class DatevPropertyItems
    {
        public DatevPropertyItems(AxDokument axDokument, AxEinstellung axEinstellung)
        {
            this.m_axDokument = axDokument;
            this.m_axEinstellung = axEinstellung;
            this.InitValues();
        }

        [Browsable(false)]
        private AxDokument m_axDokument;
        [Browsable(false)]
        private AxEinstellung m_axEinstellung;
        
        [Browsable(true)]
        [ReadOnly(true)] 
        [Description("Das Jahr der Buchungen.")]
        [Category("Basisdaten")]
        [DisplayName("Buchungsjahr")]
        public int BookingsYear { get => this.m_axDokument.Jahr; }

        private bool m_ExportSelected;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Wenn Wahr bzw. True, dann werden nur die per Selektion ausgewählten Buchungen exportiert.")]
        [Category("Einstellungen")]
        [DisplayName("Export per Selektion")]
        public bool ExportSelected { get => this.m_ExportSelected; set => this.m_ExportSelected = value; }

        private bool m_ShortenOverlongTextValues;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Überlange Textwerte werden auf die erlaubten Länge gekürzt, ohne jeweils dabei eine Exception auszulösen. Die Kürzung erfolgt am Ende des Textes. Manche Textwerte werden unabhängig von dieser Einstellung immer gekürzt. Werden die Texte nicht gekürzt, dann wird der Export mit einem Fehler abgebrochen, wenn ein überlanger Text gefunden wird. Wird dieser Wert auf 'False' gesetzt, dann wird 'Überlange Textwerte speichern' automatisch mit auf 'False' gesetzt.")]
        [Category("Einstellungen")]
        [DisplayName("Textwerte kürzen")]
        public bool ShortenOverlongTextValues { get => this.m_ShortenOverlongTextValues; set => this.m_ShortenOverlongTextValues = value; }

        private bool m_SaveOverlongTextValues;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("20 Bereiche mit jeweils 210 Zeichen können verwendet werden, um abgeschnittene Texte zu übertragen. Geöffnete Bereiche können nicht für unterschiedliche Texte verwendet werden, auch dann nicht, wenn diese nicht voll befüllt sind. Die Bereiche werden nach Badarf verwendet und somit ist es nicht garantiert, dass alle abgeschnittenen Texte übertragen werden können.")]
        [Category("Einstellungen")]
        [DisplayName("Überlange Textwerte speichern")]
        public bool SaveOverlongTextValues { get => this.m_SaveOverlongTextValues; set => this.m_SaveOverlongTextValues = value; }

        private string m_Origin;
        /// <summary>
        /// s. Header 8
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Herkunfts-Kennzeichen (2 Zeichen).")]
        [Category("Optionale Daten")]
        [DisplayName("Herkunft")]
        public string Origin { get => this.m_Origin; set => this.m_Origin = value; }

        private string exportedBy;
        /// <summary>
        ///  s. Header 9
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Der Benutzername des Users, der den Export durchgeführt hat (max. 25 Zeichen).")]
        [Category("Optionale Daten")]
        [DisplayName("Exportiert von")]
        public string ExportedBy
        {
            get
            {
                if (exportedBy == null)
                {
                    ExportedBy = m_axEinstellung.HoleEinstellung("[ECTDatev]ExportedBy");
                    if (exportedBy == "")   // noch nicht extra für das Plugin gespeichert?
                    {                       // dann aus privaten Einstellungen holen
                        string vorname = m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]vorname");
                        string nachname = m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]name");
                        ExportedBy = (vorname.Length > 0 ? vorname + " " : "") + nachname;
                    }
                }

                return exportedBy;
            }
            set
            {
                if (exportedBy != value)
                {
                    if (value != null)
                    {
                        string vorname = m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]vorname");
                        string nachname = m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]name");
                        if (exportedBy == (vorname.Length > 0 ? vorname + " " : "") + nachname)   // entspricht dem Namen unter pers. Einstellungen?
                            m_axEinstellung.SpeichereEinstellung("[ECTDatev]ExportedBy", "");  // dann nicht extra speichern
                        else
                            m_axEinstellung.SpeichereEinstellung("[ECTDatev]ExportedBy", value);
                    }

                    exportedBy = value;
                }
            }
        }

        private string m_DatevSKR;
        /// <summary>
        /// s. Header 27
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Datev-Kontenrahmen (2 Zeichen).")]
        [Category("Optionale Daten")]
        [DisplayName("Kontenrahmen")]
        public string DatevSKR { get => this.m_DatevSKR; set => this.m_DatevSKR = value; }

        private DateTime m_FromDate;
        /// <summary>
        /// s. Header 15
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Das Datum im Buchungsjahr, von dem die Buchungsdaten hier angezeigt werden.")]
        [Category("Datumsbereich der Buchungen")]
        [DisplayName("Anfang")]
        public DateTime FromDate { get => this.m_FromDate; set => this.m_FromDate = value; }

        private DateTime m_UntilDate;
        /// <summary>
        /// s. Header 16
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Das Datum im Buchungsjahr, bis dem die Buchungsdaten hier angezeigt werden.")]
        [Category("Datumsbereich der Buchungen")]
        [DisplayName("Ende")]
        public DateTime UntilDate { get => this.m_UntilDate; set => this.m_UntilDate = value; }

        /// <summary>
        /// s. Header 22
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Die Währung der Buchungen.")]
        [Category("Basisdaten")]
        [DisplayName("Währung")]
        public string CurrencyCode { get => this.m_axDokument.Waehrung; }

        private int? consultantID;
        /// <summary>
        /// s. Header 11
        /// Consultant number, value between 1001 and 9999999
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Beraternummer, Wert zwischen 1001 und 9999999")]
        [Category("Mussdaten")]
        [DisplayName("Beraternummer")]
        public int? ConsultantID
        {
            get
            {
                if (this.consultantID == null)
                {
                    int n;
                    if (Int32.TryParse(m_axEinstellung.HoleEinstellung("[ECTDatev]ConsultantID"), out n))
                        ConsultantID = n;
                }
                return this.consultantID;
            }
            set
            {
                if (value != consultantID)
                {
                    this.consultantID = value;
                    if (value != null)
                        m_axEinstellung.SpeichereEinstellung("[ECTDatev]ConsultantID", value.ToString());
                }
            }
        }

        private int? clientID;
        /// <summary>
        /// s. Header 12
        /// Client number, value between 1 and 99999
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Mandantennummer, Wert zwischen 1 und 99999. Diese Angabe befindet sich normalerweise auf der Rechnung der Steuerberater.")]
        [Category("Mussdaten")]
        [DisplayName("Mandantennummer")]
        public int? ClientID
        {
            get
            {
                if (this.clientID == null)
                {
                    int n;
                    if (Int32.TryParse(m_axEinstellung.HoleEinstellung("[ECTDatev]ClientID"), out n))
                        ClientID = n;
                }
                return this.clientID;
            }
            set
            {
                if (value != clientID)
                {
                    clientID = value;
                    if (value != null)
                        m_axEinstellung.SpeichereEinstellung("[ECTDatev]ClientID", value.ToString());
                }
            }
        }

        private DateTime m_BeginningOfFiscalYear;
        /// <summary>
        /// s. Header 13
        /// Beginning of fiscal year
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Beginn des Wirtschaftsjahres")]
        [Category("Mussdaten")]
        [DisplayName("Beginn des Wirtschaftsjahres")]
        public DateTime BeginningOfFiscalYear { get => this.m_BeginningOfFiscalYear; set => this.m_BeginningOfFiscalYear = value; }

        private string m_LabelEntryBatch;
        /// <summary>
        /// s. Header 17
        /// Name of the entry batch
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Bezeichnung des Buchungsstapels (30 Zeichen)")]
        [Category("Optionale Daten")]
        [DisplayName("Buchungsstapel")]
        public string LabelEntryBatch { get => this.m_LabelEntryBatch; set => this.m_LabelEntryBatch = value; }

        private string initials;
        /// <summary>
        /// s. Header 18
        /// the initials are used from the exported entry batch
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Das Diktatkürzel wird aus dem exportierten Buchungsstapel verwendet. (2 Zeichen)")]
        [Category("Optionale Daten")]
        [DisplayName("Diktatkürzel")]
        public string Initials
        {
            get
            {
                if (initials == null)
                {
                    Initials = m_axEinstellung.HoleEinstellung("[ECTDatev]Initials");
                    if (initials == "")
                    {
                        string vorname = this.m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]vorname");
                        string nachname = this.m_axEinstellung.HoleEinstellung("[Persoenliche_Daten]name");
                        Initials = (vorname.Length > 0 ? vorname.Substring(0, 1) : "X") + (nachname.Length > 0 ? nachname.Substring(0, 1) : "X");
                    }
                }

                return initials;
            }
            set
            {
                if (value != initials)
                {
                    if (value != null)
                        m_axEinstellung.SpeichereEinstellung("[ECTDatev]Initials", value);
                    initials = value;
                }
            }
        }

        private string m_ApplicationInformation;
        /// <summary>
        /// s. Header 31
        /// Processing flag of the transferring application
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Verarbeitungskennzeichen der abgebenden Anwendung. (16 Zeichen)")]
        [Category("Optionale Daten")]
        [DisplayName("Anwendungsinformation")]
        public string ApplicationInformation { get => this.m_ApplicationInformation; set => this.m_ApplicationInformation = value; }

        [Browsable(false)]
        private void InitValues()
        {
            this.Origin = Constants.OriginDefault;
            this.FromDate = new DateTime(this.BookingsYear, 1, 1);
            this.UntilDate = new DateTime(this.BookingsYear, 12, 31);
            this.BeginningOfFiscalYear = new DateTime(this.BookingsYear, 1, 1);
            this.LabelEntryBatch = "Belege";
            this.ShortenOverlongTextValues = true;
            this.SaveOverlongTextValues = true;
            this.ClientID = null;
            this.ConsultantID = null;
        }

        /// <summary>
        /// Checks, whether all must-have-fields have value set:
        /// <list type="bullet">
        /// <item>int</item><term>must be different than 0</term>
        /// <item>string</item><term>must be not empty</term>
        /// </list>
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public bool DataValidator()
        {
            // TODO: There shall be a nice and automatic way to do it with PropertyDescriptorCollection 
            return
            (
                this.ConsultantID.HasValue && this.ConsultantID.Value > 0 &&
                this.ClientID.HasValue && this.ClientID.Value > 0 &&
                this.BeginningOfFiscalYear != null &&
                this.FromDate != null && this.UntilDate != null && this.FromDate <= this.UntilDate
            );
        }

        /// <summary>
        /// Returns the property name as string via introspection
        /// </summary>
        private string GetPropertyName()
        {
            StackTrace callStackTrace = new StackTrace();
            StackFrame propertyFrame = callStackTrace.GetFrame(2); // 2: below GetPropertyName frame
            return propertyFrame.GetMethod().Name;
        }
    }
}
