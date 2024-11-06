using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models
{
    public class PrefernceResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<CustomerPreference> CustomerPreferences { get; set; }

        public List<PromoCode> PromoCodes { get; set; }
    }
}
