using System.Data.Entity;

namespace ProjectsBaseShared.Data
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
    }
}
