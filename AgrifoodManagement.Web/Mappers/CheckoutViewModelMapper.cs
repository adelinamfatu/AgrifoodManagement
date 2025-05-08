using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models.Shop;

namespace AgrifoodManagement.Web.Mappers
{
    public static class CheckoutViewModelMapper
    {
        public static CheckoutViewModel Map(CartDto cart, string email, string deliveryMethod, decimal deliveryFee, DiscountCodeDto? discount)
        {
            var (discountAmount, discountPercentage) = CalculateDiscount(cart.SubTotal, discount);

            return new CheckoutViewModel
            {
                OrderId = cart.OrderId,
                Email = email,
                DeliveryMethod = deliveryMethod,

                ItemCount = cart.Items.Sum(i => i.QuantityOrdered),
                Subtotal = cart.SubTotal,
                DiscountCode = discount != null ? discount.Code : null,
                Discount = discountAmount,
                DiscountPercentage = discountPercentage,
                DeliveryFee = deliveryFee
            };
        }

        private static (decimal discountAmount, int discountPercentage) CalculateDiscount(decimal subtotal, DiscountCodeDto? discount)
        {
            if (discount == null)
                return (0m, 0);

            return discount.Type switch
            {
                DiscountType.Percentage => (
                    discountAmount: subtotal * discount.Value / 100m,
                    discountPercentage: (int)discount.Value
                ),
                DiscountType.FixedAmount => (
                    discountAmount: discount.Value,
                    discountPercentage: 0
                ),
                _ => (0m, 0)
            };
        }
    }
}
