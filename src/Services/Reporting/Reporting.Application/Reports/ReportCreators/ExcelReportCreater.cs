using ClosedXML.Excel;
using Contacting.Dto.Persons.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Reporting.Core.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Reports.ReportCreators
{
    public class ExcelReportCreator : IExcelReportCreater
    {
        private IHostingEnvironment _environment;
        private AppSettings _appSettings;

        public ExcelReportCreator(
            IHostingEnvironment environment,
            IOptions<AppSettings> appSettings)
        {
            _environment = environment;
            _appSettings = appSettings.Value;
        }


        public string CreateLocationReport(string fileName, List<LocationReport> reportData)
        {
            var reportTable = GetReportTable("LocationReport", reportData);

            fileName = $"{fileName}.xlsx";
            var basePath = $"reports";
            var fullBasePath = $"{_environment.WebRootPath}/{basePath}";
            var fullFilePath = $"{fullBasePath}/{fileName}";

            if (!Directory.Exists(fullBasePath))
            {
                Directory.CreateDirectory(fullBasePath);
            }

            var ds = new DataSet();
            var wb = new XLWorkbook();

            ds.Tables.Add(reportTable);
            wb.Worksheets.Add(ds);
            wb.SaveAs(fullFilePath);

            return $"{_appSettings.ApplicationUrl}/{basePath}/{fileName}";
        }



        private DataTable GetReportTable(string tableName, List<LocationReport> reportData)
        {
            DataTable table = new DataTable { TableName = tableName };

            table.Columns.Add("Location", typeof(String));
            table.Columns.Add("PersonCount", typeof(int));
            table.Columns.Add("PhoneCount", typeof(int));

            reportData.ForEach(x =>
            {
                table.Rows.Add(x.Location, x.PersonCount, x.PhoneCount);
            });

            return table;
        }

    }
}
