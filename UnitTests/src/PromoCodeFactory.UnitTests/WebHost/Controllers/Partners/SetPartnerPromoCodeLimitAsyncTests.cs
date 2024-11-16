using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        //private readonly Mock<PartnerPromoCodeLimit> _partnerPromoCodeLimitRepositoryMock;
        private readonly PartnersController _partnersController;
        private readonly SetPartnerPromoCodeLimitRequest _setPartnerPromoCodeLimitRequest;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
            _setPartnerPromoCodeLimitRequest  = fixture.Build<SetPartnerPromoCodeLimitRequest>().Create();
        }

        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = true,
                NumberIssuedPromoCodes = 200,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("e00633a5-978a-420e-a7d6-3e1dab116393"),
                        CreateDate = new DateTime(2020, 07, 9),
                        EndDate = new DateTime(2020, 10, 9),
                        Limit = 100
                    }
                }
            };

            return partner;
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFoun()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            Partner partner = null;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsLock_ReturnsBadRequest()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;
            partner.IsActive = false;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act

            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_SetLimitPartners_NumberIssuedPromoCodesSetZerro()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;
            partner.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).EndDate = DateTime.Now.AddDays(1);

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            partner.NumberIssuedPromoCodes.Should().Be((int)0);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitIsEnded_NumberIssuedPromoCodesNotSetZerro()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;

            partner.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).EndDate = DateTime.Now.AddDays(-2);

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            partner.NumberIssuedPromoCodes.Should().NotBe((int)0);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_SetLimitPartners_PrevLimitSetCanselDate()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;

            //partner.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).EndDate = DateTime.Now.AddDays(-2);

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            partner.PartnerLimits.LastOrDefault(x => x.CancelDate.HasValue).Should().NotBeNull();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitOverZero_ReturnCreatedAtActionResult()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            _setPartnerPromoCodeLimitRequest.Limit = 150;

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SetPartnerPromoCodeLimitAsync_LimitNotOverZero_ReturnBadRequest(int _limit)
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            _setPartnerPromoCodeLimitRequest.Limit = _limit;

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_LimitPartnersSaveInDB_ReturnPartnerLimit()
        {
            // Arrange
            var partner = CreateBasePartner();
            var partnerId = partner.Id;

            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _setPartnerPromoCodeLimitRequest);

            // Assert
            _partnersRepositoryMock.Object.GetByIdAsync(partnerId).Result
                .PartnerLimits.FirstOrDefault(l => l.Id.Equals(((CreatedAtActionResult)result).RouteValues["limitId"]))
                .Should().BeEquivalentTo(_setPartnerPromoCodeLimitRequest, options => options.Including(x => x.EndDate).Including(x => x.Limit));
        }

    }
}