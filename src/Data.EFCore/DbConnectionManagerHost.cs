using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Data
{
    public class DbConnectionManagerHost : BackgroundService
    {

        protected readonly ILogger logger;
        protected readonly DbConnectionManager mgr;

        public DbConnectionManagerHost(DbConnectionManager mgr, ILogger<DbConnectionManagerHost> logger) : base()
        {
            // FIXME what is causing failure in this class' service runtime?
            this.mgr = mgr;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            logger.LogInformation("ExecuteAsync in {0}", this);
            await mgr.ConfigureDataServicesAsync(token); // FIXME the failure is occurring here            
        }
    }
}
