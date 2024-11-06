using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.DataAccess.Helper
{
	public class DatabaseInitializationHostedService : IHostedService
	{
		private readonly DatabaseInitializerService _dbInitializer;

		public DatabaseInitializationHostedService(
			DatabaseInitializerService dbInitializer
			)
		{
			_dbInitializer = dbInitializer;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_dbInitializer.CreateOrAlterStoredProcedure();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
