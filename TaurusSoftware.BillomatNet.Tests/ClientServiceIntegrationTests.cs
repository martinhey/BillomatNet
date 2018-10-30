﻿using System.Threading;
using System.Threading.Tasks;
using TaurusSoftware.BillomatNet.Queries;
using TaurusSoftware.BillomatNet.Types;
using Xunit;

namespace TaurusSoftware.BillomatNet.Tests
{
    public class ClientServiceIntegrationTests
    {
        [Fact]
        public async Task GetClientsByName()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var query = new Query<Client, ClientFilter>()
                .AddFilter(x => x.Name, "Regiofaktur")
                .AddSort(x => x.City, SortOrder.Ascending);

            var result = await service.GetListAsync(query, CancellationToken.None);

            Assert.True(result.List.Count > 0);
        }

        [Fact]
        public async Task GetClients()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetListAsync(CancellationToken.None);

            Assert.True(result.List.Count > 0);
        }

        [Fact]
        public async Task GetClientById()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetById(1227912);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetClientByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetById(1));
        }

        [Fact]
        public async Task GetContacts()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetContactListAsync(1227912, CancellationToken.None);

            Assert.True(result.List.Count > 0);
        }

        [Fact]
        public async Task GetContactById()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetContactById(35641);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetContactByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetContactById(1);
            Assert.Null(result);
        }

    }
}