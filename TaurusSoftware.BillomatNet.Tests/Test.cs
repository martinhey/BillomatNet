﻿using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TaurusSoftware.BillomatNet.Model;
using Xunit;

namespace TaurusSoftware.BillomatNet.Tests
{
    public class Test
    {
        [Fact]
        public async Task Testit()
        {
            var config = Helpers.GetTestConfiguration();
           
            var s = new ClientService(config);
            var r = await s.GetListAsync(new ClientFilterSortOptions
            {
                Filter = new ClientFilter
                {
                    Name = "Regiofaktur"
                },
                Sort = new ClientSortSettings
                {
                    new ClientSortItem{ Order = SortOrder.Ascending, Property = y => y.City}
                }
            });

            var c = await s.GetById(1227912);
            Assert.True(true);
        }
    }
}
