namespace MyWebApp;

public class MyServiceSettings
{
	public int MaximumRetries { get; set; }

	public string[] KnownUsers { get; set; } = [];

	public MyNestedServiceSettings NestedSettings { get; set; } = new();
}

public class MyNestedServiceSettings
{
	public string ConfiguredHost { get; set; } = "localhost";
}