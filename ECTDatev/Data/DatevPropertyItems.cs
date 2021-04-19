using AxEASYCTXLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Data
{
    public class DatevPropertyItems
    {
        public DatevPropertyItems(AxDokument axDokument)
        {
            this.m_axDokument = axDokument;
            this.InitValues();
        }

        [Browsable(false)]
        private AxDokument m_axDokument;
        
        [Browsable(true)]
        [ReadOnly(true)] 
        [Description("Das Jahr der Buchungen")]
        [Category("Basisdaten")]
        [DisplayName("Buchungsjahr")]
        public int BookingsYear { get => this.m_axDokument.Jahr; }

        private string m_Origin;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Herkunftzeichen für die exportierten Daten (2 Zeichen)")] 
        [Category("Mussfelder")]
        [DisplayName("Herkunft")]
        public string Origin { get => this.m_Origin; set => this.m_Origin = value; }

        private string m_DatevSKR;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Datev-Kontenrahmen (2 Zeichen)")]
        [Category("Optionale Felder")]
        [DisplayName("Kontenrahmen")]
        public string DatevSKR { get => this.m_DatevSKR; set => this.m_DatevSKR = value; }

        private bool m_ExportSelected;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Wenn 'wahr', dann werden nur die ausgewählten Buchungen exportiert")]
        [Category("Basisdaten")]
        [DisplayName("ExportSelected")]
        public bool ExportSelected { get => this.m_ExportSelected; set => this.m_ExportSelected = value; }

        private DateTime m_FromDate;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Das Datum im Buchungsjahr von dem die Buchungsdaten hier angezeigt werden")]
        [Category("Basisdaten")]
        [DisplayName("Von")]
        public DateTime FromDate { get => this.m_FromDate; set => this.m_FromDate = value; }

        private DateTime m_UntilDate;
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Das Datum im Buchungsjahr bis dem die Buchungsdaten hier angezeigt werden")]
        [Category("Basisdaten")]
        [DisplayName("Bis")]
        public DateTime UntilDate { get => this.m_UntilDate; set => this.m_UntilDate = value; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Die Währung der Buchungen")]
        [Category("Basisdaten")]
        [DisplayName("Währung")]
        public string CurrencyCode { get => this.m_axDokument.Waehrung; }

        [Browsable(false)]
        private void InitValues()
        {
            this.m_Origin = Constants.OriginDefault;
            this.m_FromDate = new DateTime(this.BookingsYear, 1, 1);
            this.m_UntilDate = new DateTime(this.BookingsYear, 12, 31);
        }
    }
}
