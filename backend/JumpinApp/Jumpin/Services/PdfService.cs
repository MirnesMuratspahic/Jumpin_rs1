namespace Jumpin.Services
{
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;
    using System;
    using System.IO;
    using Jumpin.Models;
    using iText.Layout.Properties;
    using iText.Kernel.Font;
    using iText.Kernel.Colors;
    using iText.Kernel.Pdf.Canvas.Draw;
    using iText.Kernel.Geom;

    public class PdfService
    {
        public byte[] CreateRequestPdf(Request requestData)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);

                PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                var title = new Text("JumpinApp")
                        .SetFont(boldFont)
                        .SetFontSize(24)
                        .SetFontColor(ColorConstants.GREEN); 

                document.Add(new Paragraph(title).SetTextAlignment(TextAlignment.CENTER));

                document.Add(new Paragraph("You have sent a request for reservation on the following ad:")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12));

                document.Add(new Paragraph("\n"));

                Table table = new Table(2);
                table.SetWidth(500);

                table.AddCell("Passenger Email:");
                table.AddCell(requestData.PassengerEmail);

                if (requestData is RouteRequest route)
                {
                    table.AddCell("Route Name:");
                    table.AddCell(route.UserRoute.Route.Name);
                    table.AddCell("Seats:");
                    table.AddCell(route.UserRoute.Route.SeatsNumber.ToString());
                    table.AddCell("Date and Time:");
                    table.AddCell(route.UserRoute.Route.DateAndTime);
                    table.AddCell("Price:");
                    table.AddCell(route.UserRoute.Route.Price.ToString());
                    table.AddCell("Route Type:");
                    table.AddCell(route.UserRoute.Route.Type);
                    table.AddCell("Description:");
                    table.AddCell(route.UserRoute.Route.Description);
                    table.AddCell("Status:");
                    table.AddCell(route.Status);
                }
                else if (requestData is CarRequest car)
                {
                    table.AddCell("Car Name:");
                    table.AddCell(car.UserCar.Car.Name);
                    table.AddCell("Date and Time:");
                    table.AddCell(car.UserCar.Car.DateAndTime);
                    table.AddCell("Price:");
                    table.AddCell(car.UserCar.Car.Price.ToString());
                    table.AddCell("Car Type:");
                    table.AddCell(car.UserCar.Car.Type);
                    table.AddCell("Description:");
                    table.AddCell(car.UserCar.Car.Description);
                    table.AddCell("Status:");
                    table.AddCell(car.Status);
                }
                else if (requestData is FlatRequest flat)
                {
                    table.AddCell("Flat Name:");
                    table.AddCell(flat.UserFlat.Flat.Name);
                    table.AddCell("Date and Time:");
                    table.AddCell(flat.UserFlat.Flat.DateAndTime.ToShortDateString());
                    table.AddCell("Price:");
                    table.AddCell(flat.UserFlat.Flat.Price.ToString());
                    table.AddCell("Flat Type:");
                    table.AddCell(flat.UserFlat.Flat.Type);
                    table.AddCell("Description:");
                    table.AddCell(flat.UserFlat.Flat.Description);
                    table.AddCell("Status:");
                    table.AddCell(flat.Status);
                }

                document.Add(table);


                float pageHeight = pdf.GetDefaultPageSize().GetHeight();
                float tableHeight = table.GetNumberOfRows() * 20f;


                table.SetFixedPosition(0, (pageHeight - tableHeight) / 2, pdf.GetDefaultPageSize().GetWidth());

                document.Add(new Paragraph("Request is currently being processed")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12));

                document.Add(new Paragraph("Thank you for using our app, Jumpin team")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12)
                                .SetFixedPosition(0, 20, pdf.GetDefaultPageSize().GetWidth()));

                document.Add(new LineSeparator(new SolidLine())
                                .SetFixedPosition(0, 0, pdf.GetDefaultPageSize().GetWidth()));

                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                document.Add(new Paragraph(currentDateTime)
                                .SetFontSize(10)
                                .SetTextAlignment(TextAlignment.RIGHT)
                                .SetFixedPosition(pdf.GetDefaultPageSize().GetWidth() - 100, 20, 100));

                document.Close();

                return ms.ToArray();
            }


        }

        public byte[] AnswerRequestPdf(int answer, Request requestData)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);

                PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                var title = new Text("JumpinApp")
                        .SetFont(boldFont)
                        .SetFontSize(24)
                        .SetFontColor(ColorConstants.GREEN);

                document.Add(new Paragraph(title).SetTextAlignment(TextAlignment.CENTER));

                document.Add(new Paragraph("You have sent a request for reservation on the following ad:")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12));

                document.Add(new Paragraph("\n"));

                Table table = new Table(2);
                table.SetWidth(500);

                table.AddCell("Passenger Email:");
                table.AddCell(requestData.PassengerEmail);

                if (requestData is RouteRequest route)
                {
                    table.AddCell("Route Name:");
                    table.AddCell(route.UserRoute.Route.Name);
                    table.AddCell("Seats:");
                    table.AddCell(route.UserRoute.Route.SeatsNumber.ToString());
                    table.AddCell("Date and Time:");
                    table.AddCell(route.UserRoute.Route.DateAndTime);
                    table.AddCell("Price:");
                    table.AddCell(route.UserRoute.Route.Price.ToString());
                    table.AddCell("Route Type:");
                    table.AddCell(route.UserRoute.Route.Type);
                    table.AddCell("Description:");
                    table.AddCell(route.UserRoute.Route.Description);
                    table.AddCell("Status:");
                    table.AddCell(route.Status);
                }
                else if (requestData is CarRequest car)
                {
                    table.AddCell("Car Name:");
                    table.AddCell(car.UserCar.Car.Name);
                    table.AddCell("Date and Time:");
                    table.AddCell(car.UserCar.Car.DateAndTime);
                    table.AddCell("Price:");
                    table.AddCell(car.UserCar.Car.Price.ToString());
                    table.AddCell("Car Type:");
                    table.AddCell(car.UserCar.Car.Type);
                    table.AddCell("Description:");
                    table.AddCell(car.UserCar.Car.Description);
                    table.AddCell("Status:");
                    table.AddCell(car.Status);
                }
                else if (requestData is FlatRequest flat)
                {
                    table.AddCell("Flat Name:");
                    table.AddCell(flat.UserFlat.Flat.Name);
                    table.AddCell("Date and Time:");
                    table.AddCell(flat.UserFlat.Flat.DateAndTime.ToShortDateString());
                    table.AddCell("Price:");
                    table.AddCell(flat.UserFlat.Flat.Price.ToString());
                    table.AddCell("Flat Type:");
                    table.AddCell(flat.UserFlat.Flat.Type);
                    table.AddCell("Description:");
                    table.AddCell(flat.UserFlat.Flat.Description);
                    table.AddCell("Status:");
                    table.AddCell(flat.Status);
                }

                document.Add(table);


                float pageHeight = pdf.GetDefaultPageSize().GetHeight();
                float tableHeight = table.GetNumberOfRows() * 20f;


                table.SetFixedPosition(0, (pageHeight - tableHeight) / 2, pdf.GetDefaultPageSize().GetWidth());


                if (answer == 0)
                {
                    document.Add(new Paragraph("ACCEPTED")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(17)
                        .SetFontColor(ColorConstants.GREEN));
                }
                else if (answer == 1)
                {
                    document.Add(new Paragraph("DECLINED")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(17)
                        .SetFontColor(ColorConstants.RED));
                }

                document.Add(new Paragraph("Thank you for using our app, Jumpin team")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12)
                                .SetFixedPosition(0, 20, pdf.GetDefaultPageSize().GetWidth()));

                document.Add(new LineSeparator(new SolidLine())
                                .SetFixedPosition(0, 0, pdf.GetDefaultPageSize().GetWidth()));

                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                document.Add(new Paragraph(currentDateTime)
                                .SetFontSize(10)
                                .SetTextAlignment(TextAlignment.RIGHT)
                                .SetFixedPosition(pdf.GetDefaultPageSize().GetWidth() - 100, 20, 100));

                document.Close();

                return ms.ToArray();
            }


        }
    }


}
