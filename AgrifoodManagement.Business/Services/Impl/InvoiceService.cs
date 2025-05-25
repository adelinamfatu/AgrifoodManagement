using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private static readonly HttpClient _http = new HttpClient();

        public InvoiceService()
        {
        }

        public async Task<byte[]> GenerateInvoiceAsync(InvoiceDataDto data)
        {
            using var document = new PdfDocument();
            var page = document.Pages.Add();
            var gfx = page.Graphics;

            var titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
            var headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            var bodyFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            var brush = PdfBrushes.Black;

            gfx.DrawString("Harvestica S.R.L.", titleFont, brush, new PointF(0, 0));
            gfx.DrawString($"Invoice: {data.OrderId}", headerFont, brush, new PointF(0, 30));
            gfx.DrawString($"Date: {data.OrderDate:MM/dd/yyyy}", bodyFont, brush, new PointF(0, 50));

            float y = 80;
            gfx.DrawString("Bill to:", headerFont, brush, new PointF(0, y));
            y += 15;
            gfx.DrawString(data.ConsumerName, bodyFont, brush, new PointF(0, y));
            y += 12;
            gfx.DrawString(data.ConsumerAddress, bodyFont, brush, new PointF(0, y));

            string F(decimal v) => $"{v:0.00} lei";

            float sellerX = page.Graphics.ClientSize.Width / 2 + 10;

            foreach (var sec in data.Sellers)
            {
                y += 30;
                gfx.DrawString("Seller:", headerFont, brush, new PointF(sellerX, y));
                y += 15;
                gfx.DrawString(sec.SellerName, bodyFont, brush, new PointF(sellerX, y));
                y += 12;
                gfx.DrawString(sec.SellerAddress, bodyFont, brush, new PointF(sellerX, y));

                if (!string.IsNullOrWhiteSpace(sec.SellerSignatureDataUrl))
                {
                    byte[] imgBytes;
                    var sig = sec.SellerSignatureDataUrl.Trim();

                    if (sig.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                    {
                        var comma = sig.IndexOf(',');
                        imgBytes = Convert.FromBase64String(sig[(comma + 1)..]);
                    }
                    else
                    {
                        imgBytes = await _http.GetByteArrayAsync(sig);
                    }

                    using var msImg = new MemoryStream(imgBytes);
                    var bmp = new PdfBitmap(msImg);

                    var sigRect = new RectangleF(sellerX, y + 20, 100, 50);
                    gfx.DrawImage(bmp, sigRect);
                }

                y += 80;

                var grid = new PdfGrid
                {
                    DataSource = sec.Items.Select(i => new
                    {
                        Product = i.ProductName,
                        Qty = i.Quantity,
                        UnitPrice = F(i.UnitPrice),
                        LineTotal = F(i.UnitPrice * i.Quantity)
                    }).ToList()
                };

                grid.Headers.Add(1);
                var hdr = grid.Headers[0];
                hdr.Cells[0].Value = "Product";
                hdr.Cells[1].Value = "Qty";
                hdr.Cells[2].Value = "Unit Price";
                hdr.Cells[3].Value = "Line Total";
                hdr.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
                hdr.Style.BackgroundBrush = PdfBrushes.LightGray;

                var layoutResult = grid.Draw(page, new PointF(0, y));
                y = layoutResult.Bounds.Bottom + 20;
            }

            gfx.DrawString(
                $"Grand Total: {data.GrandTotal:0.00} lei",
                headerFont,
                brush,
                new PointF(page.Graphics.ClientSize.Width - 180, y)
            );

            await using var ms = new MemoryStream();
            document.Save(ms);
            return ms.ToArray();
        }
    }
}
