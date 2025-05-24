using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;

namespace AgrifoodManagement.Web.Mappers
{
    public static class OrderHistoryViewModelMapper
    {
        public static OrderHistoryViewModel Map(List<OrderTreeDto> orders)
        {
            return new OrderHistoryViewModel
            {
                Orders = orders.Select(MapRow).ToList()
            };
        }

        private static OrderTreeRowViewModel MapRow(OrderTreeDto dto)
        {
            return new OrderTreeRowViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Delivery = dto.Delivery,
                BuyerPhone = dto.BuyerPhone,
                SellerPhone = dto.SellerPhone,
                Status = dto.Status,
                Quantity = dto.Quantity,
                Total = dto.Total,
                CanReview = dto.CanReview,
                Children = dto.Children?.Select(MapRow).ToList()
            };
        }
    }
}
