using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
