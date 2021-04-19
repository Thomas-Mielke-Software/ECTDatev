using AxEASYCTXLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Data
{
    public class DataManager
    {
        private AxDokument m_axDokument;

        public DataManager(AxDokument axDokument)
        {
            this.AXDokument = axDokument;
        }

        public AxDokument AXDokument { get => this.m_axDokument; set => this.m_axDokument = value; }

        
    }
}
