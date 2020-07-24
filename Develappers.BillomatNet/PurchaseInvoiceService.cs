﻿using System;
using Develappers.BillomatNet.Api.Net;

namespace Develappers.BillomatNet
{
    public class PurchaseInvoiceService : ServiceBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="PurchaseInvoiceService"/>.
        /// </summary>
        /// <param name="configuration">The service configuration.</param>
        public PurchaseInvoiceService(Configuration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PurchaseInvoiceService"/> for unit tests.
        /// </summary>
        /// <param name="httpClientFactory">The function which creates a new <see cref="IHttpClient" /> implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the parameter is null.</exception>
        internal PurchaseInvoiceService(Func<IHttpClient> httpClientFactory) : base(httpClientFactory)
        {
        }
    }
}
