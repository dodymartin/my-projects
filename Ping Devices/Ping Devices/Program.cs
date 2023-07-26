using System.Net.NetworkInformation;

Console.WriteLine("Ping Machines");

var failureFileName = $"failed_{Environment.MachineName}.txt";

Console.WriteLine("Compare Y/N?");
var key = Console.ReadKey();
if (key.KeyChar == 'Y' || key.KeyChar == 'y')
{
    Console.Write("Compare machine: ");
    var compareMachineName = Console.ReadLine();

    using var writer = new StreamWriter($"mismatches_{Environment.MachineName}_{compareMachineName}", false);
    var compareFailures = File.ReadAllLines($"failed_{compareMachineName}.txt");
    foreach (var failure in File.ReadAllLines(failureFileName))
    {
        if (!compareFailures.Contains(failure))
        {
            Console.WriteLine($"Mismatch {failure}");
            writer.WriteLine(failure);
        }
    }
}
else
{
    var ping = new Ping();
    using var writer = new StreamWriter(failureFileName, false);
    foreach (var machineName in File.ReadAllLines("machines.txt"))
    {
        Console.WriteLine($"Pinging {machineName}!");

        try
        {
            var pingResult = ping.Send(machineName);
            if (pingResult.Status != IPStatus.Success)
            {
                Console.WriteLine($"Machine {machineName} failed!");
                writer.WriteLine(machineName);
            }
        }
        catch
        {
            Console.WriteLine($"Machine {machineName} failed!");
            writer.WriteLine(machineName);
        }
    }
}
Console.WriteLine("Done");
Console.ReadKey();