using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YodaIpsum.Shared;

namespace YodaIpsum.Tweeter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IQuoteGenerator, QuoteGenerator>();
            // services.AddSingleton<>()
        }
    }
}
