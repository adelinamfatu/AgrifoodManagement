using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public enum ForumCategory
    {
        [Display(Name = "General Discussion")]
        [ForumIcon("<i class='fas fa-comments'></i>", "#4a6da7")]
        GeneralDiscussion,

        [Display(Name = "Crop Management")]
        [ForumIcon("<i class='fas fa-seedling'></i>", "#2ecc71")]
        CropManagement,

        [Display(Name = "Harvesting")]
        [ForumIcon("<i class='fas fa-tractor'></i>", "#e67e22")]
        Harvesting,

        [Display(Name = "Stock Management")]
        [ForumIcon("<i class='fas fa-warehouse'></i>", "#8e44ad")]
        StockManagement,

        [Display(Name = "Market Trends")]
        [ForumIcon("<i class='fas fa-chart-line'></i>", "#3498db")]
        MarketTrends,

        [Display(Name = "Equipment and Technology")]
        [ForumIcon("<i class='fas fa-tools'></i>", "#34495e")]
        EquipmentAndTechnology,

        [Display(Name = "Sustainability")]
        [ForumIcon("<i class='fas fa-leaf'></i>", "#27ae60")]
        Sustainability,

        [Display(Name = "Networking")]
        [ForumIcon("<i class='fas fa-users'></i>", "#9b59b6")]
        Networking,

        [Display(Name = "Legal and Regulatory")]
        [ForumIcon("<i class='fas fa-gavel'></i>", "#c0392b")]
        LegalAndRegulatory,

        [Display(Name = "Farming Events")]
        [ForumIcon("<i class='fas fa-calendar-alt'></i>", "#f39c12")]
        FarmingEvents,

        [Display(Name = "Product Pricing")]
        [ForumIcon("<i class='fas fa-tags'></i>", "#d35400")]
        ProductPricing,

        [Display(Name = "Feedback and Suggestions")]
        [ForumIcon("<i class='fas fa-comment-dots'></i>", "#16a085")]
        FeedbackAndSuggestions,

        [Display(Name = "Sales and Marketing")]
        [ForumIcon("<i class='fas fa-bullhorn'></i>", "#e74c3c")]
        SalesAndMarketing
    }
}
