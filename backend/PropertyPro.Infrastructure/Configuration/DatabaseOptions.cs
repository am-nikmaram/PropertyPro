namespace PropertyPro.Infrastructure.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool ApplyMigrations { get; set; }

    public bool SeedSystemData { get; set; }

    public bool SeedDemoData { get; set; }
}