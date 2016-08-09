using System.IO;
using OfficeOpenXml;

namespace FIMMonitoring.Domain.Repositories
{
    public class ReportRepository
    {
        private SoftLogsContext context;

        public ReportRepository(SoftLogsContext context)
        {
            this.context = context;
        }

        public byte[] GenerateValidationErrorsReport(int importId)
        {
            var rows = context.GetValidationErrors(importId);
            using (var excel = new ExcelPackage())
            {
                var ws = excel.Workbook.Worksheets.Add("ValidationResults");
                ws.Cells["A1"].LoadFromDataTable(rows, true);

                using (var ms = new MemoryStream())
                {
                    excel.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
