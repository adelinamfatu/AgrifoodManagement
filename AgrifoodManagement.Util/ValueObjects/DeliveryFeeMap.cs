using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public static class DeliveryFeeMap
    {
        public class DeliveryOption
        {
            public decimal Fee { get; set; }
            public string SvgIcon { get; set; }
        }

        public static readonly IReadOnlyDictionary<string, DeliveryOption> Options =
            new Dictionary<string, DeliveryOption>(StringComparer.OrdinalIgnoreCase)
            {
                ["Normal"] = new DeliveryOption
                {
                    Fee = 5.00m,
                    SvgIcon = "<svg width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\r\n<path d=\"M22 12h-4l-3 9L9 3l-3 9H2\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />\r\n</svg>"
                },
                ["SameDay"] = new DeliveryOption
                {
                    Fee = 10.00m,
                    SvgIcon = "<svg width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\r\n<rect x=\"1\" y=\"3\" width=\"15\" height=\"13\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />\r\n<polygon points=\"16 8 20 8 23 11 23 16 20 16 20 13 16 13\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />\r\n<circle cx=\"5.5\" cy=\"18.5\" r=\"2.5\" stroke=\"currentColor\" stroke-width=\"2\" />\r\n<circle cx=\"18.5\" cy=\"18.5\" r=\"2.5\" stroke=\"currentColor\" stroke-width=\"2\" />\r\n</svg>"
                },
                ["Express"] = new DeliveryOption
                {
                    Fee = 20.00m,
                    SvgIcon = "<svg width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\r\n                                <circle cx=\"12\" cy=\"12\" r=\"10\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />\r\n<polyline points=\"12 6 12 12 16 14\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />\r\n</svg>"
                }
            };
    }
}
