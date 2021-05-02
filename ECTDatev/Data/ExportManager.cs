﻿using ECTDatev.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Data
{
    /// <summary>
    /// Managing the export
    /// </summary>
    public class ExportManager
    {
        /// <summary>
        /// With this call will the export be ordered.
        /// </summary>
        /// <param name="buchungen">The data to be exported.</param>
        /// <param name="propertyGridData">The data of the property grid</param>
        public void Export(Collection<Buchung> buchungen, DatevPropertyItems propertyGridData)
        {
            this.m_Buchungen = buchungen;
            this.m_PropertyGridData = propertyGridData;

            this.m_dataCategoryList = new List<int>() { 21, 20 }; // list of needed actions for above data

            foreach (int dataCategory in this.m_dataCategoryList)
            {
                CreateExportFile(dataCategory, this.m_Buchungen, this.m_PropertyGridData);
            }
        }

        private Collection<Buchung> m_Buchungen; // the bookings
        private DatevPropertyItems m_PropertyGridData; // additional data (in property grid)
        private List<int> m_dataCategoryList; // the list of action for the given data

        public void CreateExportFile(int dataCategory, Collection<Buchung> buchungen, DatevPropertyItems propertyGridData)
        {
            StringBuilder fileContent;
            DatevHeader header;

            try
            {
                switch (dataCategory)
                {
                    case 21:
                        fileContent = new StringBuilder();

                        header = new DatevHeader(ToDo.DataCategoryID);
                        fileContent.Append(header.GetHeader());
                        fileContent.Append(DatevHeadline.GetHeadline(ToDo.DataCategoryID));

                        fileContent.Append(Validator.Manager(dataCategory, buchungen, propertyGridData));

                        // TODO save the file here
                        break;
                    case 20:
                        fileContent = new StringBuilder();

                        header = new DatevHeader(ToDo.DataCategoryID);
                        fileContent.Append(header.GetHeader());
                        fileContent.Append(DatevHeadline.GetHeadline(ToDo.DataCategoryID));

                        fileContent.Append(Validator.Manager(dataCategory, buchungen, propertyGridData));

                        // TODO save the file here
                        break;
                    default:
                        throw new NotImplementedException("CreateExportFile: dataCategory: " + dataCategory);
                }
            }
            catch (Exception e)
            {
                // TODO Handle the exception. Something went wrong during the export.
                throw e;
            }
        }
    }
}
