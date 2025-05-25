using AgrifoodManagement.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<byte[]> GenerateInvoiceAsync(InvoiceDataDto data);
    }
}
