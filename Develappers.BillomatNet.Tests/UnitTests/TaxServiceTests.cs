﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Develappers.BillomatNet.Api.Net;
using Develappers.BillomatNet.Types;
using FakeItEasy;
using Xunit;

namespace Develappers.BillomatNet.Tests.UnitTests
{
    public class TaxServiceTests : UnitTestBase<TaxService>
    {
        [Fact]
        public async Task CreateTax_WithValidData_ShouldCreateTaxItemAndReturnCorrectValues()
        {
            // arrange
            const string name = "xUnit Test";
            var taxItem = new Tax { Name = name, Rate = 1.0f, IsDefault = false };

            const string expectedRequestBody =
                "{\"tax\":{\"id\":null,\"created\":null,\"updated\":null,\"name\":\"xUnit Test\",\"rate\":\"1\",\"is_default\":\"0\"}}";
            var expectedRequestUri = new Uri("/api/taxes", UriKind.Relative);
            const string responseBody =
                "{\"tax\":{\"id\":\"119547\",\"created\":\"2020-07-26T09:17:20+02:00\",\"updated\":\"2020-07-26T09:17:20+02:00\",\"name\":\"xUnit Test\",\"rate\":\"1\",\"is_default\":\"0\",\"customfield\":\"\"}}";

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.PostAsync(expectedRequestUri, expectedRequestBody, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(responseBody));

            var sut = GetSystemUnderTest(http);

            // act
            var result = await sut.CreateAsync(taxItem);

            // assert
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();

            Assert.Equal(name, result.Name);
            Assert.Equal(119547, result.Id);
            Assert.Equal(DateTime.Parse("2020-07-26T09:17:20+02:00", CultureInfo.InvariantCulture), result.Created);
            Assert.Equal(DateTime.Parse("2020-07-26T09:17:20+02:00", CultureInfo.InvariantCulture), result.Updated);
            Assert.Equal(1f, result.Rate);
            Assert.False(result.IsDefault);
        }

        [Fact]
        public async Task CreateTax_WithInvalidApiKey_ShouldThrowNotAuthorizedException()
        {
            // arrange
            const string name = "xUnit Test";
            var taxItem = new Tax { Name = name, Rate = 1.0f, IsDefault = false };

            var expectedRequestUri = new Uri("/api/taxes", UriKind.Relative);

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.PostAsync(expectedRequestUri, A<string>.Ignored, A<CancellationToken>.Ignored))
                .ThrowsAsync(ExceptionFactory.CreateNotAuthorizedException);


            var sut = GetSystemUnderTest(http);

            // act and assert
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.CreateAsync(taxItem));
            A.CallTo(() => http.PostAsync(expectedRequestUri, A<string>.Ignored, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateTax_WithInvalidInputValue_ShouldThrowArgumentException()
        {
            // arrange
            var http = A.Fake<IHttpClient>();
            var sut = GetSystemUnderTest(http);

            // act and assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.CreateAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(new Tax()));
            await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(new Tax { Id = 999 }));
        }

        [Fact]
        public async Task GetTaxById_WithInvalidApiKey_ShouldThrowNotAuthorizedException()
        {
            // arrange
            const int id = 1;
            var expectedRequestUri = new Uri("/api/taxes/1", UriKind.Relative);

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .ThrowsAsync(ExceptionFactory.CreateNotAuthorizedException);

            var sut = GetSystemUnderTest(http);

            // act and assert
            await Assert.ThrowsAsync<NotAuthorizedException>(() => sut.GetByIdAsync(id));
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetTaxById_WithInvalidId_ShouldReturnNull()
        {
            // arrange
            const int id = 1;
            var expectedRequestUri = new Uri("/api/taxes/1", UriKind.Relative);

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .ThrowsAsync(ExceptionFactory.CreateNotFoundException);

            var sut = GetSystemUnderTest(http);

            // act and assert
            var result = await sut.GetByIdAsync(id);
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTaxById_WithValidData_ShouldReturnCorrectValues()
        {
            // arrange
            const int id = 21281;
            var expectedRequestUri = new Uri("/api/taxes/21281", UriKind.Relative);
            const string responseBody =
                "{\"tax\":{\"id\":\"21281\",\"created\":\"2015-11-05T13:16:49+01:00\",\"updated\":\"2016-11-27T21:57:21+01:00\",\"name\":\"MwSt\",\"rate\":\"19\",\"is_default\":\"1\",\"customfield\":\"\"}}";

            var expectedResult = new Tax
            {
                Id = id,
                Name = "MwSt",
                Created = DateTime.Parse("2015-11-05T13:16:49+01:00", CultureInfo.InvariantCulture),
                Updated = DateTime.Parse("2016-11-27T21:57:21+01:00", CultureInfo.InvariantCulture),
                Rate = 19f,
                IsDefault = true
            };

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(responseBody));

            var sut = GetSystemUnderTest(http);

            // act
            var result = await sut.GetByIdAsync(id);

            // assert
            A.CallTo(() => http.GetAsync(expectedRequestUri, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();

            DomainAssert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetList_ShouldReturnCorrectValues()
        {
            // arrange
            var expectedRequestUri = new Uri("/api/taxes", UriKind.Relative);
            const string responseBody = "{\"taxes\":{\"tax\":[{\"id\":\"21281\",\"created\":\"2015-11-05T13:16:49+01:00\",\"updated\":\"2016-11-27T21:57:21+01:00\",\"name\":\"MwSt\",\"rate\":\"19\",\"is_default\":\"0\",\"customfield\":\"\"},{\"id\":\"21282\",\"created\":\"2015-11-05T13:17:12+01:00\",\"updated\":\"2016-11-27T19:05:35+01:00\",\"name\":\"MwSt\",\"rate\":\"7\",\"is_default\":\"0\",\"customfield\":\"\"},{\"id\":\"21436\",\"created\":\"2015-11-10T10:55:59+01:00\",\"updated\":\"2015-11-10T10:55:59+01:00\",\"name\":\"Mehrwertsteuer\",\"rate\":\"19\",\"is_default\":\"0\",\"customfield\":\"\"},{\"id\":\"32362\",\"created\":\"2018-02-24T09:52:54+01:00\",\"updated\":\"2020-07-14T14:28:43+02:00\",\"name\":\"blups\",\"rate\":\"4\",\"is_default\":\"1\",\"customfield\":\"\"}],\"@page\":\"1\",\"@per_page\":\"100\",\"@total\":\"4\"}}";
            var expectedResult = new List<Tax>
            {
                new Tax
                {
                    Id = 21281,
                    Created = DateTime.Parse("2015-11-05T13:16:49+01:00", CultureInfo.InvariantCulture),
                    Updated = DateTime.Parse("2016-11-27T21:57:21+01:00", CultureInfo.InvariantCulture),
                    Name = "MwSt",
                    Rate = 19f,
                    IsDefault = false
                },
                new Tax
                {
                    Id = 21282,
                    Created = DateTime.Parse("2015-11-05T13:17:12+01:00", CultureInfo.InvariantCulture),
                    Updated = DateTime.Parse("2016-11-27T19:05:35+01:00", CultureInfo.InvariantCulture),
                    Name = "MwSt",
                    Rate = 7f,
                    IsDefault = false
                },
                new Tax
                {
                    Id = 21436,
                    Created = DateTime.Parse("2015-11-10T10:55:59+01:00", CultureInfo.InvariantCulture),
                    Updated = DateTime.Parse("2015-11-10T10:55:59+01:00", CultureInfo.InvariantCulture),
                    Name = "Mehrwertsteuer",
                    Rate = 19f,
                    IsDefault = false
                },
                new Tax
                {
                    Id = 32362,
                    Created = DateTime.Parse("2018-02-24T09:52:54+01:00", CultureInfo.InvariantCulture),
                    Updated = DateTime.Parse("2020-07-14T14:28:43+02:00", CultureInfo.InvariantCulture),
                    Name = "blups",
                    Rate = 4f,
                    IsDefault = true
                }
            };

            var http = A.Fake<IHttpClient>();
            A.CallTo(() => http.GetAsync(expectedRequestUri, null, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(responseBody));

            var sut = GetSystemUnderTest(http);

            // act
            var result = await sut.GetListAsync();

            // assert
            A.CallTo(() => http.GetAsync(expectedRequestUri, null, A<CancellationToken>.Ignored))
                .MustHaveHappenedOnceExactly();

            Assert.Equal(4, result.TotalItems);

            result.List.AssertWith(expectedResult, DomainAssert.Equal);
        }
    }
}
