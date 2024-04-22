using AdminHalloDoc.Controllers.Login;
using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Controllers.AdminControllers
{
    public class ReportController : Controller
    {
        #region Constructor
        private readonly ApplicationDbContext _context;
        private IRequestRepository _requestRepository;
        private IRecordsRepository _recordsRepository;

        public ReportController(ApplicationDbContext context, IRequestRepository requestRepository, IRecordsRepository recordsRepository)
        {
            _context = context;
            _requestRepository = requestRepository;
            _recordsRepository = recordsRepository;
        }
        #endregion 

        #region Dashboerd_excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadExcel(PaginatedViewModel details, string status)
        {
            try
            {

                details.PageSize = -1;
                details.CurrentPage = 1;
                var data = await _requestRepository.GetContactAsync(status, details);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");

                // Set border color and style for the entire worksheet
                worksheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Style.Border.OutsideBorderColor = XLColor.Gray;
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
                foreach (var item in data.DashboardList)
                {
                    var currentRow = worksheet.Row(row);
                    worksheet.Cell(row, 1).Value = item.PatientName;

                    worksheet.Cell(row, 2).Value = item.Dob.ToString();
                    worksheet.Cell(row, 3).Value = item.Requestor;

                    worksheet.Cell(row, 4).Value = item.Physician;

                    worksheet.Cell(row, 5).Value = "123";
                    worksheet.Cell(row, 6).Value = item.RequestedDate;
                    worksheet.Cell(row, 7).Value = item.PhoneNumber;
                    worksheet.Cell(row, 8).Value = item.Address;
                    worksheet.Cell(row, 9).Value = item.Notes;
                    if (item.RequestTypeID == 2)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                        }
                    }else if (item.RequestTypeID == 3)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightCoral;
                        }
                    }
                    else if (item.RequestTypeID == 4)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                    }
                    else
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightPink;
                        }
                    }
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
        #endregion

        #region SearchRecords_excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadExcelForSearchRecords(RecordsModel rm)
        {
            try
            {

                rm.PageSize = -1;
                rm.CurrentPage = 1;
                rm.Status = 0;
                rm.RequestType = 0;
                var data = await _recordsRepository.GetRequestsbyfilterForRecords(rm);
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Data");

                // Set border color and style for the entire worksheet
                worksheet.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Style.Border.OutsideBorderColor = XLColor.Gray;
                worksheet.Cell(1, 1).Value = "Patient Name";
                worksheet.Cell(1, 2).Value = "Requestor";
                worksheet.Cell(1, 3).Value = "Date Of service";
                worksheet.Cell(1, 4).Value = "Close Case Date";
                worksheet.Cell(1, 5).Value = "Email";
                worksheet.Cell(1, 6).Value = "Phone number";
                worksheet.Cell(1, 7).Value = "Address";
                worksheet.Cell(1, 8).Value = "Zip";
                worksheet.Cell(1, 9).Value = "Request Status";
                worksheet.Cell(1, 10).Value = "Physician";
                worksheet.Cell(1, 11).Value = "Physician Notes";
                worksheet.Cell(1, 12).Value = "Cancel By Provider Note";
                worksheet.Cell(1, 13).Value = "Admin Notes";
                worksheet.Cell(1, 14).Value = "Patient Notes";

                int row = 2;
                foreach (var item in data.SearchRecordList)
                {
                    var currentRow = worksheet.Row(row);
                    worksheet.Cell(row, 1).Value = item.PatientName;
                    worksheet.Cell(row, 2).Value = Enum.GetName(typeof(RequestType), item.RequestTypeID);
                    worksheet.Cell(row, 3).Value = item.DateOfService;
                    worksheet.Cell(row, 4).Value = item.CloseCaseDate != null ? item.CloseCaseDate : "-";
                    worksheet.Cell(row, 5).Value = item.Email;
                    worksheet.Cell(row, 6).Value = item.PhoneNumber;
                    worksheet.Cell(row, 7).Value = item.Address;
                    worksheet.Cell(row, 8).Value = item.Zip;
                    worksheet.Cell(row, 9).Value = Enum.GetName(typeof(Status), item.Status);
                    worksheet.Cell(row, 10).Value = item.PhysicianName;
                    worksheet.Cell(row, 11).Value = item.PhysicianNote;
                    worksheet.Cell(row, 12).Value = !string.IsNullOrEmpty(item.CancelByProviderNote) ? item.CancelByProviderNote : "-";
                    worksheet.Cell(row, 13).Value = item.AdminNote;
                    worksheet.Cell(row, 14).Value = item.PatientNote;

                    if (item.RequestTypeID == 2)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                        }
                    }
                    else if (item.RequestTypeID == 3)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightCoral;
                        }
                    }
                    else if (item.RequestTypeID == 4)
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                    }
                    else
                    {
                        foreach (var cell in currentRow.Cells())
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightPink;
                        }
                    }
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
        #endregion
    }
}
