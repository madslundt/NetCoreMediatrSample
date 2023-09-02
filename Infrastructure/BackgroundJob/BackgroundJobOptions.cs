namespace Infrastructure.BackgroundJob;

public class BackgroundJobOptions
{
    public static readonly string ConnectionString = "BackgroundJobConnectionString";
    public string DashboardPath { get; } = null!;
}