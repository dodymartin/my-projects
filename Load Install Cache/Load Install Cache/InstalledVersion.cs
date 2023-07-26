namespace Load_Install_Cache
{
    internal class InstalledVersion
    {
        public string MachineName { get; set; }
        public string Version { get; set; }

        public InstalledVersion(string machineName, string version)
        {
            MachineName = machineName;
            Version = version;
        }
    }
}
