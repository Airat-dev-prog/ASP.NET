using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {

        private readonly IRepository<Customer> _сustomerRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<PromoCode> _promoCodeRepository;

        public CustomersController(IRepository<Customer> сustomerRepository, IRepository<Preference> preferenceRepository, IRepository<PromoCode> promoCodeRepository)
        {
            _сustomerRepository = сustomerRepository;
            _preferenceRepository = preferenceRepository;
            _promoCodeRepository = promoCodeRepository;
        }

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            //TODO: Добавить получение списка клиентов
            var сustomers = await _сustomerRepository.GetAllAsync();

            var сustomersModelList = сustomers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                }).ToList();

            return сustomersModelList;
        }

        /// <summary>
        /// Получение клиента вместе с выданными ему промомкодами
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var сustomer = await _сustomerRepository.GetByIdAsync(id);

            if (сustomer == null)
                return NotFound();

            var сustomerModel = new CustomerResponse()
            {
                Id = сustomer.Id,
                FirstName = сustomer.FirstName,
                LastName = сustomer.LastName,
                Email = сustomer.Email,
                PromoCodes = сustomer.Promocodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    PartnerName = x.PartnerName
                }).ToList()
            };

            return сustomerModel;
        }

        /// <summary>
        /// Создание нового клиента вместе с его предпочтениями
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            foreach(Guid preferenceId in request.PreferenceIds)
            {
                customer.CustomerPreferences.Add(new CustomerPreference()
                {
                    Customer = customer,
                    Preference = (await _preferenceRepository.GetByIdAsync(preferenceId))
                });
            }

            await _сustomerRepository.AddAsync(customer);

            var сustomerModel = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PromoCodes = customer.Promocodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    PartnerName = x.PartnerName
                }).ToList()
            };
            return сustomerModel;
        }

        /// <summary>
        /// Обновить данные клиента вместе с его предпочтениями
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponse>> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            var customer = await _сustomerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            customer.CustomerPreferences.Clear();
            foreach (Guid preferenceId in request.PreferenceIds)
            {
                customer.CustomerPreferences.Add(new CustomerPreference()
                {
                    Customer = customer,
                    Preference = (await _preferenceRepository.GetByIdAsync(preferenceId))
                });
            }

            await _сustomerRepository.UpdateAsync(customer);

            var сustomerModel = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PromoCodes = customer.Promocodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    PartnerName = x.PartnerName
                }).ToList()
            };
            return сustomerModel;
        }

        /// <summary>
        /// Удаление клиента вместе с выданными ему промокодами
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<CustomerResponse>> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами

            var сustomer = await _сustomerRepository.GetByIdAsync(id);

            if (сustomer == null)
                return NotFound();

            var сustomerModel = new CustomerResponse()
            {
                Id = сustomer.Id,
                FirstName = сustomer.FirstName,
                LastName = сustomer.LastName,
                Email = сustomer.Email,
                PromoCodes = сustomer.Promocodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                    EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                    PartnerName = x.PartnerName
                }).ToList()
            };

            foreach (PromoCode promoCode in сustomer.Promocodes)
            {
                сustomer.Promocodes.Remove(promoCode);
                await _promoCodeRepository.DeleteAsync(promoCode);
            }                

            await _сustomerRepository.DeleteAsync(сustomer);

            return сustomerModel;

        }
    }
}