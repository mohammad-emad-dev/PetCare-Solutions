using System.Configuration;

namespace bitcINTERFACE
{
    internal static class DatabaseConfig
    {
        private const string ConnectionName = "bitcINTERFACE.Properties.Settings.PetCareSolutionsConnectionString";

        internal static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionName]?.ConnectionString
                    ?? "Data Source=localhost;Initial Catalog=PetCareSolutions;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            }
        }
    }
}
