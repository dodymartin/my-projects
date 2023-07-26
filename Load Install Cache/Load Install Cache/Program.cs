using Load_Install_Cache;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

internal class Program
{
    static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings()
    {
        DefaultValueHandling = DefaultValueHandling.Ignore,
        Formatting = Newtonsoft.Json.Formatting.Indented
    };

    private static void Main(string[] args)
    {

        OracleConfiguration.NamesDirectoryPath = "(LDAP)";
        OracleConfiguration.TnsAdmin = "..\\..";

        var applications = new Applications()
        {
            new KeyValuePair<string, int>("AppCenter.exe",157),
            new KeyValuePair<string, int>("Beef.Services.MSMQReceive.exe",163),
            new KeyValuePair<string, int>("Beef.UI.ParameterMaintenance.exe",324),
            new KeyValuePair<string, int>("Box_Prod_Comm.exe",186),
            new KeyValuePair<string, int>("Box_Prod_Oper.exe",187),
            new KeyValuePair<string, int>("Box_Ship_Comm.exe",231),
            new KeyValuePair<string, int>("Box_Ship_Oper.exe",230),
            new KeyValuePair<string, int>("Cap.Pfs.Bpf.Condemnation.ServicesLoader.exe",323),
            new KeyValuePair<string, int>("Cap.Pfs.Bpf.UI.Condemnation.exe",322),
            new KeyValuePair<string, int>("Cap.Pfs.Bpf.UI.FabScale.exe",256),
            new KeyValuePair<string, int>("Cap.Pfs.UI.Parameters.exe",327),
            new KeyValuePair<string, int>("CarcassIdentification.exe",161),
            new KeyValuePair<string, int>("Daily Maintenance.exe",219),
            new KeyValuePair<string, int>("Fabuserp.exe",257),
            new KeyValuePair<string, int>("GrdUserP.exe",258),
            new KeyValuePair<string, int>("HarvestFloorInstall.exe",321),
            new KeyValuePair<string, int>("Import Legacy Inventory.exe",289),
            new KeyValuePair<string, int>("Install Application Script.exe",317),
            new KeyValuePair<string, int>("Install Application.exe",297),
            new KeyValuePair<string, int>("Install Oracle XE.exe",312),
            new KeyValuePair<string, int>("Ipfs Box Shipping.exe",285),
            new KeyValuePair<string, int>("Ipfs Cbs Upload.exe",46),
            new KeyValuePair<string, int>("Ipfs Cps Upload.exe",48),
            new KeyValuePair<string, int>("Ipfs Data Transfer.exe",49),
            new KeyValuePair<string, int>("Ipfs Dated Inventory.exe",50),
            new KeyValuePair<string, int>("Ipfs Db Maintenance.exe",193),
            new KeyValuePair<string, int>("Ipfs Device Check.exe",52),
            new KeyValuePair<string, int>("Ipfs Handheld.exe",2),
            new KeyValuePair<string, int>("Ipfs History Upload.exe",53),
            new KeyValuePair<string, int>("Ipfs Import Wcs Tpfs Users.exe",181),
            new KeyValuePair<string, int>("Ipfs Kph Parameters.exe",326),
            new KeyValuePair<string, int>("Ipfs Kph Scale.exe",206),
            new KeyValuePair<string, int>("Ipfs Logical Access Daily.exe",57),
            new KeyValuePair<string, int>("Ipfs Makesheet Upload.exe",180),
            new KeyValuePair<string, int>("Ipfs Master Data Import.exe",59),
            new KeyValuePair<string, int>("Ipfs Master Db Purge.exe",60),
            new KeyValuePair<string, int>("Ipfs Master Rdb Download.exe",224),
            new KeyValuePair<string, int>("Ipfs Master Table Download.exe",61),
            new KeyValuePair<string, int>("Ipfs Master Tpfs Download.exe",288),
            new KeyValuePair<string, int>("Ipfs Par Import.exe",62),
            new KeyValuePair<string, int>("Ipfs Pc Data Import.exe",190),
            new KeyValuePair<string, int>("Ipfs Plant Integration Service Host.exe",101),
            new KeyValuePair<string, int>("Ipfs Prism Upload.exe",68),
            new KeyValuePair<string, int>("Ipfs Product Id Sync.exe",70),
            new KeyValuePair<string, int>("Ipfs Product Import External.exe",71),
            new KeyValuePair<string, int>("Ipfs Roll Production Standards.exe",227),
            new KeyValuePair<string, int>("Ipfs Tagger.exe",296),
            new KeyValuePair<string, int>("Ipfs Task Scheduler Manager.exe",189),
            new KeyValuePair<string, int>("Ipfs Testing Data Load.exe",307),
            new KeyValuePair<string, int>("KillFloorHandheld.exe",320),
            new KeyValuePair<string, int>("LabelMaker.exe",325),
            new KeyValuePair<string, int>("Production_Parameters.exe",188),
            new KeyValuePair<string, int>("SNTissue.exe",318),
            new KeyValuePair<string, int>("Stratos Assign Process Order.exe",299),
            new KeyValuePair<string, int>("Stratos Background Reports.exe",240),
            new KeyValuePair<string, int>("Stratos Box Production UI.exe",315),
            new KeyValuePair<string, int>("Stratos Box Production.exe",303),
            new KeyValuePair<string, int>("Stratos Cbs Corporate Import.exe",195),
            new KeyValuePair<string, int>("Stratos Cbs Corporate Upload.exe",228),
            new KeyValuePair<string, int>("Stratos Cbs Plant Import.exe",196),
            new KeyValuePair<string, int>("Stratos Ccia Import.exe",268),
            new KeyValuePair<string, int>("Stratos Cleanup.exe",292),
            new KeyValuePair<string, int>("Stratos Collection Agent Web Api.exe",271),
            new KeyValuePair<string, int>("Stratos Collection Agent.exe",300),
            new KeyValuePair<string, int>("Stratos Com Corporate Import.exe",197),
            new KeyValuePair<string, int>("Stratos Com Plant Import.exe",198),
            new KeyValuePair<string, int>("Stratos Configuration Web Api Host.exe",210),
            new KeyValuePair<string, int>("Stratos Consumption Allocation.exe",298),
            new KeyValuePair<string, int>("Stratos Corporate Integration Web Api Host.exe",211),
            new KeyValuePair<string, int>("Stratos Corporate Report.exe",301),
            new KeyValuePair<string, int>("Stratos Dam Notification.exe",264),
            new KeyValuePair<string, int>("Stratos Data Validation.exe",266),
            new KeyValuePair<string, int>("Stratos Database Inventory.exe",272),
            new KeyValuePair<string, int>("Stratos Database Worker.exe",308),
            new KeyValuePair<string, int>("Stratos Db Purge.exe",208),
            new KeyValuePair<string, int>("Stratos Device Communication Web Api.exe",314),
            new KeyValuePair<string, int>("Stratos Ear Tag Retirement.exe",269),
            new KeyValuePair<string, int>("Stratos Ear Tag Upload.exe",274),
            new KeyValuePair<string, int>("Stratos Ewm Upload.exe",286),
            new KeyValuePair<string, int>("Stratos Frequent Worker.exe",309),
            new KeyValuePair<string, int>("Stratos Hides Import.exe",262),
            new KeyValuePair<string, int>("Stratos Hides Scale Service Host.exe",261),
            new KeyValuePair<string, int>("Stratos It Export.exe",291),
            new KeyValuePair<string, int>("Stratos It Integration Web Api.exe",281),
            new KeyValuePair<string, int>("Stratos Jobs Manager.exe",290),
            new KeyValuePair<string, int>("Stratos Lims Upload.exe",248),
            new KeyValuePair<string, int>("Stratos Master Pc Table Download.exe",259),
            new KeyValuePair<string, int>("Stratos Mii Upload.exe",287),
            new KeyValuePair<string, int>("Stratos Mobile Web Api.exe",278),
            new KeyValuePair<string, int>("Stratos Outgoing Integration Web Api Host.exe",233),
            new KeyValuePair<string, int>("Stratos PLC Integrated Palletizer.exe",225),
            new KeyValuePair<string, int>("Stratos Plant Integration Web Api.exe",252),
            new KeyValuePair<string, int>("Stratos Plant Report Web Api Host.exe",215),
            new KeyValuePair<string, int>("Stratos Plant Report.exe",302),
            new KeyValuePair<string, int>("Stratos Plant Upload.exe",212),
            new KeyValuePair<string, int>("Stratos Plant Web.dll",311),
            new KeyValuePair<string, int>("Stratos Plc Web Api.exe",316),
            new KeyValuePair<string, int>("Stratos Price Corporate Import.exe",270),
            new KeyValuePair<string, int>("Stratos Prism Corporate Import.exe",199),
            new KeyValuePair<string, int>("Stratos Prism Plant Import.exe",200),
            new KeyValuePair<string, int>("Stratos Process Review Notification.exe",232),
            new KeyValuePair<string, int>("Stratos Procurement Corporate Import.exe",294),
            new KeyValuePair<string, int>("Stratos Procurement Import.exe",293),
            new KeyValuePair<string, int>("Stratos Product Average Weight.exe",263),
            new KeyValuePair<string, int>("Stratos Report Web Api.exe",313),
            new KeyValuePair<string, int>("Stratos Retrotech Corporate Import.exe",247),
            new KeyValuePair<string, int>("Stratos Retrotech Plant Import.exe",239),
            new KeyValuePair<string, int>("Stratos Retrotech Plant Upload.exe",242),
            new KeyValuePair<string, int>("Stratos Sap Plant Import.exe",238),
            new KeyValuePair<string, int>("Stratos Scheduled Reports.exe",236),
            new KeyValuePair<string, int>("Stratos Shared Integration Web Api Host.exe",249),
            new KeyValuePair<string, int>("Stratos Shipping Integration.dll",306),
            new KeyValuePair<string, int>("Stratos Standard Worker.exe",310),
            new KeyValuePair<string, int>("Stratos Stats Import.exe",237),
            new KeyValuePair<string, int>("Stratos Support Paging Web Site",250),
            new KeyValuePair<string, int>("Stratos System Health.exe",218),
            new KeyValuePair<string, int>("Stratos User Notification.exe",277),
            new KeyValuePair<string, int>("Stratos Vemac Plant Import.exe",295),
            new KeyValuePair<string, int>("Stratos Wpf Global.exe",234),
            new KeyValuePair<string, int>("Stratos Wpf Local.exe",235),
            new KeyValuePair<string, int>("Stratos Xlpl Plant Import.exe",260),
            new KeyValuePair<string, int>("Stratos Xlpl Upload.exe",241),
            new KeyValuePair<string, int>("UI.HotScale.exe",160),
            new KeyValuePair<string, int>("VMSIDXferEmail.exe",319),
            new KeyValuePair<string, int>("iPFS Label Designer.exe",305),
            new KeyValuePair<string, int>("iPFS Label Print.exe",7),
            new KeyValuePair<string, int>("iPFS Static Scale.exe",72),
            new KeyValuePair<string, int>("iPFS Web.dll",280)
        };

        var installCacheBaseDirectory = @"\\msuselkgcp5510\PlantSystems\Install Cache";
        var installCachedApplications = GetCacheVersions(installCacheBaseDirectory);
        foreach (var installCachedApplication in installCachedApplications)
        {
            if (!string.IsNullOrEmpty(installCachedApplication.ExeName))
                if (applications.TryGetValue(installCachedApplication.ExeName, out var application))
                {
                    var installedVersions = GetInstalledVersions(application.Value);
                    foreach (var installedVersion in installedVersions)
                        if (!installCachedApplication.Versions.Any(x => x.Version == installedVersion.Version))
                        {
                            Console.WriteLine($"{installCachedApplication.ExeName} {installedVersion.Version} {installedVersion.MachineName}");
                            Stratos.Core.Impersonator
                                .CreateImpersonator()
                                .AsDomain("na")
                                .AndUser("ps302405")
                                .AndPassword("J7i8l9p0")
                                .ForAction(() =>
                                {
                                    CopyApplicationToCache(installCacheBaseDirectory, application.Value, installedVersion.MachineName);
                                })
                                .Run();
                        }
                }
        }
        Console.WriteLine("******** DONE ************");
        Console.ReadKey();
    }

    static void CopyApplicationToCache(string installCacheBaseDirectory, int applicationId, string machineName)
    {
        using var cn = new OracleConnection("Data Source=ppfsn2;User Id=dadams;Password=J7i8l9p0;");
        cn.Open();
        using var rdr = new OracleCommand($@"select file_nm, mdfyd_dt_tm, file_sz, file_ver, min(dir_nm) dir_nm from cmn.vs_devc_invty where apln_id = {applicationId} and mach_nm = '{machineName}' group by file_nm, mdfyd_dt_tm, file_sz, file_ver", cn).ExecuteReader();
        while (rdr.Read())
        {
            var installCacheDirectory = Path.Combine(installCacheBaseDirectory,
                rdr["file_nm"].ToString().Replace(".exe", ""),
                "New",
                rdr["file_ver"].ToString());
            try
            {
                CopyFilesRecursively(rdr["dir_nm"].ToString().Replace("C:", $@"\\{machineName}\C$"), installCacheDirectory);
                var versionInfo = new BuildVersionInfo
                {
                    ApplicationExeName = rdr["file_nm"].ToString(),
                    ApplicationName = rdr["file_nm"].ToString().Replace(".exe", ""),
                    BuildDateTime = Convert.ToDateTime(rdr["mdfyd_dt_tm"]),
                    BuildType = "Release",
                    BuildUserId = "ScheduledBuild",
                    BuildUserName = "Scheduled Build",
                    IsPublished = Convert.ToDecimal(rdr["file_sz"]) > 250000,
                    Version = rdr["file_ver"].ToString()
                };
                SerializeToJsonFile(versionInfo, Path.Combine(installCacheDirectory, "_buildVersionInfo.json"));
            }
            catch
            { }
        }
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            if (!dirPath.ToLower().EndsWith(@"\logs") &&
                !dirPath.ToLower().EndsWith(@"\devicelogs") &&
                !dirPath.ToLower().EndsWith(@"\masterdata") &&
                !dirPath.ToLower().EndsWith(@"\productiondata"))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            if (!newPath.ToLower().Contains(@"\logs\") &&
                !newPath.ToLower().Contains(@"\devicelogs\") &&
                !newPath.ToLower().EndsWith(@"\masterdata\") &&
                !newPath.ToLower().EndsWith(@"\productiondata\") &&
                !newPath.Contains(".old") &&
                !newPath.Contains(".v") &&
                !newPath.Contains(".serialized"))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), false);
                var fi = new FileInfo(newPath.Replace(sourcePath, targetPath));
                if (fi.IsReadOnly)
                    fi.IsReadOnly = false;
            }
        }
    }

    static IList<InstalledVersion> GetInstalledVersions(int applicationId)
    {
        var installedVersions = new List<InstalledVersion>();
        using var cn = new OracleConnection("Data Source=ppfsn2;User Id=dadams;Password=J7i8l9p0;");
        cn.Open();
        using var rdr = new OracleCommand($@"select file_ver, max(mach_nm) mach_nm from cmn.vs_devc_invty where apln_id = {applicationId} group by file_ver", cn).ExecuteReader();
        while (rdr.Read()) { installedVersions.Add(new InstalledVersion(rdr["mach_nm"].ToString(), rdr["file_ver"].ToString())); }
        return installedVersions;
    }

    static IList<CacheApplication> GetCacheVersions(string buildCacheDirectory)
    {
        var cachedApplications =
            (from bv in GetVersions(buildCacheDirectory)
             group bv by new { bv.ApplicationExeName, bv.ApplicationName } into g
             select new CacheApplication
             {
                 ExeName = g.Key.ApplicationExeName,
                 Name = g.Key.ApplicationName,
                 Versions = g.ToList()
             })
            .ToList();
        if (!cachedApplications.Any())
            return new List<CacheApplication> { };
        return cachedApplications;
    }

    static IEnumerable<BuildVersionInfo> GetVersions(string cacheDirectory)
    {
        if (Directory.Exists(cacheDirectory))
            foreach (var versionInfoFile in Directory.GetFiles(cacheDirectory, BuildVersionInfo.FileName, SearchOption.AllDirectories))
                yield return DeserializeFromJsonFile<BuildVersionInfo>(versionInfoFile);
    }

    static T DeserializeFromJsonFile<T>(string fullFileName, bool required = true)
    {
        if (!File.Exists(fullFileName))
            if (required)
                throw new FileNotFoundException(fullFileName);
            else
                return default;

        T returnValue = default;
        using (var stream = new StreamReader(fullFileName))
        {
            returnValue = JsonConvert.DeserializeObject<T>(stream.ReadToEnd(), JsonSerializerSettings);
        }
        return returnValue;
    }

    static void SerializeToJsonFile(object item, string fullFileName)
    {
        var fi = new FileInfo(fullFileName);
        if (!Directory.Exists(fi.DirectoryName))
            Directory.CreateDirectory(fi.DirectoryName);

        var json = JsonConvert.SerializeObject(item, JsonSerializerSettings);
        using var stream = new StreamWriter(fullFileName);
        stream.Write(json);
    }
}
