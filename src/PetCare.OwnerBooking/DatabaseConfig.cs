using System.Configuration;

namespace BookingRequests_System
{
    internal static class DatabaseConfig
    {
        private const string ConnectionName = "BookingRequests_System.Properties.Settings.PetCareSolutionsConnectionString";

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
