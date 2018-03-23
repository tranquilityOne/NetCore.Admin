using System;
using System.Collections.Generic;
using StackExchange.Redis.Extensions.Binary;
using StackExchange.Redis.Extensions.Newtonsoft;
using Core.Common;
using Redis.Core;

namespace CoreApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RedisHelper redishelper = new RedisHelper();
                redishelper.StringSet("test", "11");
            }
            catch (Exception ex)
            {
                throw;
            }                      
            Console.ReadKey();
        }
    }
}
