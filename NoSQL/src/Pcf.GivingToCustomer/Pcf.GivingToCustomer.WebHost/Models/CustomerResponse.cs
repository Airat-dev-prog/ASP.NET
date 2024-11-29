using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.WebHost.Models
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<PreferenceResponse> Preferences { get; set; }
        public List<PromoCodeShortResponse> PromoCodes { get; set; }

        public CustomerResponse()
        {
            
        }

        public CustomerResponse(Customer customer)//, Preference preference )
        {            
            Id = customer.Id;
            Email = customer.Email;
            FirstName = customer.FirstName;
            LastName = customer.LastName;

            if (customer.Preferences != null)
                Preferences = customer.Preferences.Select(x => new PreferenceResponse()
                {
                    Id = x.Preference.Id,
                    Name = x.Preference.Name
                }).ToList();
            if (customer.PromoCodes != null)
                PromoCodes = customer.PromoCodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.PromoCode.Id,
                    Code = x.PromoCode.Code,
                    BeginDate = x.PromoCode.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = x.PromoCode.EndDate.ToString("yyyy-MM-dd"),
                    PartnerId = x.PromoCode.PartnerId,
                    ServiceInfo = x.PromoCode.ServiceInfo
                }).ToList();
            
        }
    }
}