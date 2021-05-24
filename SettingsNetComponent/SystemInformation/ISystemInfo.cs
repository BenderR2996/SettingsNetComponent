namespace SettingsNetComponent
{
    internal interface ISystemInfo
    {
        string HostName { get; }
        string IpAddress { get; }
        string MacAddress { get; }
    }
}
