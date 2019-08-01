using System.Data.Entity;
using ProjectsBaseShared.Data;

namespace ProjectsBaseSharedTests.Mock
{
    internal class DatabaseInitializerMock : DropCreateDatabaseAlways<Context>
    {
    }
}
