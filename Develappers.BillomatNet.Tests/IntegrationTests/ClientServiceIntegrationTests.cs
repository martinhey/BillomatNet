﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Develappers.BillomatNet.Queries;
using Develappers.BillomatNet.Types;
using Xunit;

namespace Develappers.BillomatNet.Tests.IntegrationTests
{
    [Trait(TraitNames.Category, CategoryNames.IntegrationTest)]
    public class ClientServiceIntegrationTests
    {
        #region Client

        [Fact]
        public async Task GetClientsByName()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);
            // ReSharper disable once RedundantArgumentDefaultValue
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

            var result = await service.GetByIdAsync(1227912);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetClientByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetByIdAsync(1));
        }

        #endregion

        #region Property

        [Fact]
        public async Task GetClientPropertyList()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var result = await service.GetPropertyListAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetClientPropertyById()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var result = await service.GetPropertyById(3075686);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetClientPropertyByIdWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var result = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetPropertyById(3075686));
        }

        [Fact]
        public async Task GetClientPropertyByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var result = await service.GetPropertyById(3000000);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateClientPropertyWhenCheckBox()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var clientProp = new ClientProperty
            {
                ClientId = 796659,
                ClientPropertyId = 7804,
                Value = "1"
            };

            var result = await service.EditAsync(clientProp);
            Assert.True((bool)result.Value);

            clientProp.Value = "0";
            await service.EditAsync(clientProp);
        }

        [Fact]
        public async Task EditClientPropertyWhenText()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var clientProp = new ClientProperty
            {
                ClientId = 796659,
                ClientPropertyId = 13027,
                Value = "Hello"
            };

            var result = await service.EditAsync(clientProp);
            Assert.Equal(clientProp.Value, (string)result.Value);

            clientProp.Value = "";
            await service.EditAsync(clientProp);
        }

        [Fact]
        public async Task EditClientPropertyWhenArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var clientProp = new ClientProperty{};

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.EditAsync(clientProp));
        }

        [Fact]
        public async Task EditClientPropertyWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var clientProp = new ClientProperty
            {
                ClientId = 796659,
                ClientPropertyId = 13027,
                Value = "Hello"
            };

            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.EditAsync(clientProp));
        }

        [Fact]
        public async Task EditClientPropertyWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var clientProp = new ClientProperty
            {
                ClientId = 796659,
                ClientPropertyId = 13029,
                Value = "Hello"
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.EditAsync(clientProp));
        }

        #endregion

        #region Tag

        [Fact]
        public async Task GetClientTagCloud()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetTagCloudAsync(CancellationToken.None);

            Assert.True(true);
        }

        [Fact]
        public async Task GetTagListAsync()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var query = new Query<ClientTag, ClientTagFilter>()
                .AddFilter(x => x.ClientId, 796659);

            var result = await service.GetTagListAsync(query);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTagListAsyncWhenArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetTagListAsync(null));
        }

        [Fact]
        public async Task GetTagListAsyncWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var query = new Query<ClientTag, ClientTagFilter>()
                .AddFilter(x => x.ClientId, 796659);

            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetTagListAsync(query));
        }

        [Fact]
        public async Task GetTagListAsyncWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var query = new Query<ClientTag, ClientTagFilter>()
                .AddFilter(x => x.ClientId, 1);

            var result = await service.GetTagListAsync(query);
            Assert.Null(result.List);
        }

        [Fact]
        public async Task GetTagById()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var result = await service.GetTagById(188156);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTagByIdWhenArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetTagById(0));
        }

        [Fact]
        public async Task GetTagByIdWhenNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();

            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.GetTagById(188156));
        }

        [Fact]
        public async Task GetTagByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var result = await service.GetTagById(100000);
            Assert.Null(result);
        }

        #endregion

        #region Contact

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

            var result = await service.GetContactByIdAsync(35641);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetContactAvatarById()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetContactAvatarByIdAsync(35641, 100);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetContactByIdWhenNotFound()
        {
            var config = Helpers.GetTestConfiguration();

            var service = new ClientService(config);

            var result = await service.GetContactByIdAsync(1);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteContact()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                ClientId = 485054,
                FirstName = "Test",
                LastName = "Testermann"
            };

            var result = await service.CreateAsync(contact);
            Assert.NotNull(await service.GetContactByIdAsync(result.Id));

            await service.DeleteContactAsync(result.Id);

            var result2 = await service.GetContactByIdAsync(result.Id);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteContactArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.DeleteContactAsync(0));
        }

        [Fact]
        public async Task DeleteContactNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.DeleteContactAsync(1));
        }

        [Fact]
        public async Task DeleteContactNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteContactAsync(1));
        }

        [Fact]
        public async Task CreateContact()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                ClientId = 485054,
                FirstName = "Test",
                LastName = "Testermann"
            };

            var result = await service.CreateAsync(contact);
            Assert.NotNull(await service.GetContactByIdAsync(result.Id));

            await service.DeleteContactAsync(result.Id);
        }

        [Fact]
        public async Task CreateContactArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact { };

            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(contact));
        }

        [Fact]
        public async Task CreateContactNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var contact = new Contact
            {
                ClientId = 485054,
                FirstName = "Test",
                LastName = "Testermann"
            };

            var result = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.CreateAsync(contact));
        }

        [Fact]
        public async Task CreateContactNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                ClientId = 1,
                FirstName = "Test",
                LastName = "Testermann"
            };

            var result = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(contact));
        }

        [Fact]
        public async Task EditContact()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                ClientId = 485054,
                FirstName = "Testt",
                LastName = "Testermann"
            };

            var result = await service.CreateAsync(contact);
            Assert.NotNull(await service.GetContactByIdAsync(result.Id));

            var editedContact = new Contact
            {
                Id = result.Id,
                ClientId = 485054,
                FirstName = "Test",
                LastName = result.LastName
            };

            var editedResult = await service.EditAsync(editedContact);
            Assert.Equal(editedContact.FirstName, editedResult.FirstName);
            await service.DeleteContactAsync(result.Id);
        }

        [Fact]
        public async Task EditContactArgumentException()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                Id = 0,
                ClientId = 485054,
                FirstName = "Testt",
                LastName = "Testermann"
            };

            var editedResult = await Assert.ThrowsAsync<ArgumentException>(() => service.EditAsync(contact));
        }

        [Fact]
        public async Task EditContactNotAuthorized()
        {
            var config = Helpers.GetTestConfiguration();
            config.ApiKey = "ajfkjeinodafkejlkdsjklj";
            var service = new ClientService(config);

            var contact = new Contact
            {
                Id = 500,
                ClientId = 485054,
                FirstName = "Testt",
                LastName = "Testermann"
            };

            var editedResult = await Assert.ThrowsAsync<NotAuthorizedException>(() => service.EditAsync(contact));
        }

        [Fact]
        public async Task EditContactNotFound()
        {
            var config = Helpers.GetTestConfiguration();
            var service = new ClientService(config);

            var contact = new Contact
            {
                Id = 1,
                ClientId = 485054,
                FirstName = "Testt",
                LastName = "Testermann"
            };

            var editedResult = await Assert.ThrowsAsync<NotFoundException>(() => service.EditAsync(contact));
        }

        #endregion
    }
}