using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PromoCodeFactory.WebHost.Controllers
{

    /// <summary>
    ///  Список предпочтений
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить данные всех предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PrefernceResponse>> GetPrefernceAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferencesModelList = preferences.Select(x =>
                new PrefernceResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CustomerPreferences = x.CustomerPreferences.Select(x => new CustomerPreference()
                    {
                        CustomerId = x.CustomerId,
                        Customer = x.Customer,
                        PreferenceId = x.PreferenceId,
                        Preference = x.Preference
                    }).ToList(),
                    PromoCodes = x.PromoCodes.Select(x => new PromoCode()
                    {
                        Code = x.Code,
                        ServiceInfo = x.ServiceInfo,
                        BeginDate = x.BeginDate,
                        EndDate = x.EndDate,
                        PartnerName = x.PartnerName,
                        PartnerManager = x.PartnerManager,
                        Preference = x.Preference,
                        Customer = x.Customer
                    }).ToList()
                }).ToList();

            return preferencesModelList;
        }
    }
}
