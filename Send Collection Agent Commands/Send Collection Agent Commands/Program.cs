using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stratos.WebApi.CollectionAgent;

namespace Send_Collection_Agent_Commands
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var groupName = "Administrators";
            //try
            //{
            //    var searcher = new ManagementObjectSearcher(new SelectQuery("Win32_GroupUser", $"GroupComponent=\"Win32_Group.Domain='{Info.SystemName}',Name='{groupName}'\""));
            //    foreach (var mgmtObject in searcher.Get())
            //    {
            //        var path = new ManagementPath(mgmtObject["PartComponent"].ToString());
            //        var obj = new ManagementObject() { Path = path };
            //        obj.Get();

            //        var domain = obj["Domain"].ToString();
            //        var name = obj["Name"].ToString().ToLower();
            //        Console.WriteLine($"{domain}\\{name}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.GetCascadedMessage());
            //}

            var logfile = await GetLogFile(string.Empty, new List<string>() { "MSNKRI21MP" });

            var machineNames = new Dictionary<string, string>
            {
                {"MSNKRI21MP", "pfsnorthkingstownservices"}
            };

            foreach (var passthroughServerGroup in machineNames.GroupBy(x => x.Value))
            {
                logfile = await GetLogFile(passthroughServerGroup.Key, passthroughServerGroup.Select(x => x.Key).ToList());
                //await DeleteService(passthroughServerGroup.Key, passthroughServerGroup.Select(x => x.Key).ToList(), "Stratos Collection Agent");
                //await DeleteService(passthroughServerGroup.Key, passthroughServerGroup.Select(x => x.Key).ToList(), "Pfs Collection Agent");
                //await RunWorker(passthroughServerGroup.Key, passthroughServerGroup.Select(x => x.Key).ToList(), "database", false);
                //await RunWorker(passthroughServerGroup.Key, passthroughServerGroup.Select(x => x.Key).ToList(), "database");
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static async Task<string> GetLogFile(string passthroughServer, List<string> machineNames)
        {
            if (!string.IsNullOrEmpty(passthroughServer))
            {
                var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{passthroughServer}:50201/api/collectionAgentCommand/" });
                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Getting 'standard' worker logfile for {machineName} via {passthroughServer}");
                    var command = $"http://{machineName}:50201/api/collectionAgentCommand/workers/standard/log-files/latest";
                    try
                    {
                        return await collectionAgentCommandRest.PassthroughAsync(machineName, command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Getting 'standard' worker logfile for {machineName}");
                    var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{machineName}:50201/api/collectionAgentCommand/" });
                    try
                    {
                        return await collectionAgentCommandRest.GetLogFileAsync("standard");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return await Task.FromResult(string.Empty);
        }

        private static async Task DeleteService(string passthroughServer, List<string> machineNames, string serviceName)
        {
            if (!string.IsNullOrEmpty(passthroughServer))
            {
                var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{passthroughServer}:50201/api/collectionAgentCommand/" });
                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Sending passthrough delete {serviceName} for {machineName} via {passthroughServer}");
                    var command = $"http://{machineName}:50201/api/collectionAgentCommand/services/{serviceName}/delete";
                    try
                    {
                        await collectionAgentCommandRest.PassthroughAsync(machineName, command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Sending delete {serviceName} for {machineName}");
                    var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{machineName}:50201/api/collectionAgentCommand/" });
                    try
                    {
                        await collectionAgentCommandRest.DeleteServiceAsync(serviceName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private static async Task RunWorker(string passthroughServer, List<string> machineNames, string worker, bool wait = true)
        {
            var tasks = new List<Task>();
            if (!string.IsNullOrEmpty(passthroughServer))
            {
                var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{passthroughServer}:50201/api/collectionAgentCommand/" });

                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Sending passthrough run {worker} for {machineName} via {passthroughServer}");
                    var command = $"http://{machineName}:50201/api/collectionAgentCommand/workers/{worker}/run";
                    try
                    {
                        tasks.Add(collectionAgentCommandRest.PassthroughAsync(machineName, command));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                foreach (var machineName in machineNames)
                {
                    Console.WriteLine($"Sending run {worker} for {machineName}");
                    var collectionAgentCommandRest = new CollectionAgentCommandWebApi(new string[] { $"http://{machineName}:50201/api/collectionAgentCommand/" });
                    try
                    {
                        tasks.Add(collectionAgentCommandRest.RunWorkerAsync(worker));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            if (wait)
                await Task.WhenAll(tasks);
            else
                await Task.WhenAny(tasks);
        }
    }
}
