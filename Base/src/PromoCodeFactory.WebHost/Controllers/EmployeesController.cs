using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create([FromBody] Employee employee)
        {
            employee.Id = Guid.NewGuid();
            var newEmployee = await _employeeRepository.Create(employee);

            if (newEmployee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = newEmployee.Id,
                Email = newEmployee.Email,
                Roles = newEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = newEmployee.FullName,
                AppliedPromocodesCount = newEmployee.AppliedPromocodesCount
            };
            return employeeModel;
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> Update(Guid id, [FromBody] Employee employee)
        {
            employee.Id = id;
            var newEmployee = await _employeeRepository.Update(employee);
            if (newEmployee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = newEmployee.Id,
                Email = newEmployee.Email,
                Roles = newEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = newEmployee.FullName,
                AppliedPromocodesCount = newEmployee.AppliedPromocodesCount
            };
            return employeeModel;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> Delete(Guid id)
        {
            var employee = await _employeeRepository.Delete(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };
            return employeeModel;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }
    }
}