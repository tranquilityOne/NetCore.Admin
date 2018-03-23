using StackExchange.Redis;
using System.Collections.Generic;
using System.Net;

namespace CoreApp.extentions
{
	internal static class RedisDatabaseExtensions
	{
		public static void FlushDatabase(this IDatabase db)
		{
			var endPoints = db.Multiplexer.GetEndPoints();

			foreach (EndPoint endpoint in endPoints)
			{
				var server = db.Multiplexer.GetServer(endpoint);

				server.FlushDatabase();
			}
		}
	}
}
