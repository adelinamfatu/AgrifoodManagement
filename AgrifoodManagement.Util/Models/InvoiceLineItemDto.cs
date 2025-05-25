using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class InvoiceLineItemDto
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }

    public class SellerInvoiceSectionDto
    {
        public string SellerName { get; set; } = "";
        public string SellerAddress { get; set; } = "";
        public string? SellerSignatureDataUrl { get; set; }
        public List<InvoiceLineItemDto> Items { get; set; } = new();
    }

    public class InvoiceDataDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ConsumerName { get; set; } = "";
        public string ConsumerAddress { get; set; } = "";
        public List<SellerInvoiceSectionDto> Sellers { get; set; } = new();
        public decimal GrandTotal => Sellers.Sum(s => s.Items.Sum(i => i.Quantity * i.UnitPrice));
    }
}
