using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ReportController : Controller
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private IRequestRepository _requestRepository;

        public ReportController(ApplicationDbContext context, IRequestRepository requestRepository)
        {
            _context = context;
            _requestRepository = requestRepository;

        }
        #endregion 

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public FileResult ExporttoExcel(string HtmlTable)
        {
            var v = Encoding.ASCII.GetBytes(HtmlTable);
            return File(Encoding.ASCII.GetBytes(HtmlTable), "application/vnd.ms-excel", "htmltable.xls");
        }
        public async Task<IActionResult> DownloadExcel(string status)
        {
            try
            {

                var data = await _requestRepository.GetContactAsync(status);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Date Of Birth";
                worksheet.Cell(1, 3).Value = "Requestor";
                worksheet.Cell(1, 4).Value = "Physician Name";
                worksheet.Cell(1, 5).Value = "Date of Service";
                worksheet.Cell(1, 6).Value = "Requested Date";
                worksheet.Cell(1, 7).Value = "Phone Number";
                worksheet.Cell(1, 8).Value = "Address";
                worksheet.Cell(1, 9).Value = "Notes";

                int row = 2;
                foreach (var item in data)
                {

                    worksheet.Cell(row, 1).Value = item.PatientName;

                    worksheet.Cell(row, 2).Value = item.Dob.ToString();
                    worksheet.Cell(row, 3).Value = item.Requestor;

                    worksheet.Cell(row, 4).Value = item.Physician;

                    worksheet.Cell(row, 5).Value = "123";
                    worksheet.Cell(row, 6).Value = item.RequestedDate;
                    worksheet.Cell(row, 7).Value = item.PhoneNumber;
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 9).Value = item.Notes;
                    row++;
                }
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                //_notyf.Success("data.xlsx file downloaded ...");
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
            }
            catch (Exception ex)
            {
                //_notyf.Warning(ex.Message);
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
