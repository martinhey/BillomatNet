﻿using Develappers.BillomatNet.Types;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Develappers.BillomatNet.Tests
{
    public class TaxServiceIntegrationTests
    {
        [Fact]
        public async Task GetListOfTaxes()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);
            var result = await service.GetListAsync(CancellationToken.None);
            Assert.True(result.List.Count > 0);
        }

        [Fact]
        public async Task GetByIdTax()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);
            var result = await service.GetByIdAsync(21281);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTaxByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);
            
            var result = await service.GetByIdAsync(21285);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTaxByIdWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new TaxService(config);
            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetByIdAsync(1));
        }

        [Fact]
        public async Task DeleteTaxItem()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);

            #region Initializing to create
            var name = "xUnit Test";

            var taxItem = new Tax
            {
                Name = name,
                Rate = 1.0f,
                IsDefault = false
            };
            #endregion

            var result = await service.CreateAsync(taxItem);
            Assert.Equal(name, result.Name);

            await service.DeleteAsync(result.Id);
            var result2 = await service.GetByIdAsync(result.Id);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteTaxItemNotExisting()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteAsync(21285));
        }

        [Fact]
        public async Task DeleteTaxItemNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new TaxService(config);
            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.DeleteAsync(21285));
        }

        [Fact]
        public async Task CreateTaxItem()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);

            var name = "xUnit Test";

            var taxItem = new Tax
            {
                Name = name,
                Rate = 1.0f,
                IsDefault = false
            };

            var result = await service.CreateAsync(taxItem);
            Assert.Equal(name, result.Name);

            await service.DeleteAsync(result.Id);
        }

        [Fact]
        public async Task CreateTaxItemWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new TaxService(config);

            var name = "xUnit Test";

            var taxItem = new Tax
            {
                Name = name,
                Rate = 1.0f,
                IsDefault = false
            };
            var ex = Assert.ThrowsAsync<NotAuthorizedException>(() => service.CreateAsync(taxItem));
        }

        [Fact]
        public async Task CreateTaxItemWhenNull()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);

            var tax = new Tax { };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(tax));
        }

        [Fact]
        public async Task EditTaxItem()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new TaxService(config);

            var name = "xUnit test";

            var taxItem = new Tax
            {
                Name = name
            };

            var result = await service.CreateAsync(taxItem);

            Assert.Equal(name, result.Name);

            var newName = "xUnit test edited";

            var editedTaxItem = new Tax
            {
                Id = result.Id,
                Name = newName
            };

            var editedResult = await service.EditAsync(editedTaxItem);
            Assert.Equal(newName, editedTaxItem.Name);

            await service.DeleteAsync(editedResult.Id);
        }
    }
}