using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Viatour_Travel.Dtos.ReservationDtos;

namespace Viatour_Travel.Services.ReservationReportService
{
    public class ReservationReportService : IReservationReportService
    {
        public byte[] GenerateExcelReport(string tourTitle, List<ResultReservationDto> reservations)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reservations");

            var generatedAt = DateTime.Now;

            worksheet.Range(1, 1, 1, 9).Merge();
            worksheet.Cell(1, 1).Value = "Viatour Reservation Report";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(2, 1).Value = "Tour";
            worksheet.Cell(2, 2).Value = tourTitle;

            worksheet.Cell(3, 1).Value = "Generated At";
            worksheet.Cell(3, 2).Value = generatedAt.ToString("dd.MM.yyyy HH:mm");

            worksheet.Cell(4, 1).Value = "Total Reservations";
            worksheet.Cell(4, 2).Value = reservations.Count;

            var headerRow = 6;
            var headers = new[]
            {
                "Reservation No",
                "Name Surname",
                "Email",
                "Phone",
                "Person Count",
                "Travel Date",
                "Created Date",
                "Status",
                "Note"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(headerRow, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            var row = headerRow + 1;

            foreach (var item in reservations)
            {
                worksheet.Cell(row, 1).Value = item.ReservationNumber;
                worksheet.Cell(row, 2).Value = item.NameSurname;
                worksheet.Cell(row, 3).Value = item.Email;
                worksheet.Cell(row, 4).Value = item.Phone;
                worksheet.Cell(row, 5).Value = item.PersonCount;
                worksheet.Cell(row, 6).Value = item.TravelDate.ToString("dd.MM.yyyy");
                worksheet.Cell(row, 7).Value = item.CreatedDate.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cell(row, 8).Value = item.Status ? "Approved" : "Pending";
                worksheet.Cell(row, 9).Value = string.IsNullOrWhiteSpace(item.Note) ? "-" : item.Note;

                worksheet.Range(row, 1, row, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(row, 1, row, 9).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                row++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(headerRow);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] GeneratePdfReport(string tourTitle, List<ResultReservationDto> reservations)
        {
            var generatedAt = DateTime.Now;
            var totalCount = reservations.Count;
            var approvedCount = reservations.Count(x => x.Status);
            var pendingCount = reservations.Count(x => !x.Status);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(header =>
                    {
                        header.Spacing(4);

                        header.Item()
                            .Text("Viatour Reservation Report")
                            .SemiBold()
                            .FontSize(18);

                        header.Item().Text($"Tour: {tourTitle}");
                        header.Item().Text($"Generated At: {generatedAt:dd.MM.yyyy HH:mm}");
                        header.Item().Text($"Total: {totalCount} | Approved: {approvedCount} | Pending: {pendingCount}");
                    });

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.6f); // Reservation No
                            columns.RelativeColumn(1.8f); // Name
                            columns.RelativeColumn(2.3f); // Email
                            columns.RelativeColumn(1.6f); // Phone
                            columns.ConstantColumn(55);   // Person Count
                            columns.ConstantColumn(75);   // Travel Date
                            columns.ConstantColumn(85);   // Created Date
                            columns.ConstantColumn(65);   // Status
                            columns.RelativeColumn(2.2f); // Note
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCellStyle).Text("Reservation No").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Name Surname").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Email").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Phone").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Count").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Travel Date").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Created Date").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Status").SemiBold();
                            header.Cell().Element(HeaderCellStyle).Text("Note").SemiBold();
                        });

                        foreach (var item in reservations)
                        {
                            table.Cell().Element(BodyCellStyle).Text(item.ReservationNumber);
                            table.Cell().Element(BodyCellStyle).Text(item.NameSurname);
                            table.Cell().Element(BodyCellStyle).Text(item.Email);
                            table.Cell().Element(BodyCellStyle).Text(item.Phone);
                            table.Cell().Element(BodyCellStyle).Text(item.PersonCount.ToString());
                            table.Cell().Element(BodyCellStyle).Text(item.TravelDate.ToString("dd.MM.yyyy"));
                            table.Cell().Element(BodyCellStyle).Text(item.CreatedDate.ToString("dd.MM.yyyy"));
                            table.Cell().Element(BodyCellStyle).Text(item.Status ? "Approved" : "Pending");
                            table.Cell().Element(BodyCellStyle).Text(string.IsNullOrWhiteSpace(item.Note) ? "-" : item.Note);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated by Viatour Admin - {generatedAt:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        private static IContainer HeaderCellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(Colors.Grey.Lighten2)
                .PaddingVertical(6)
                .PaddingHorizontal(4);
        }

        private static IContainer BodyCellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5)
                .PaddingHorizontal(4);
        }
    }
}