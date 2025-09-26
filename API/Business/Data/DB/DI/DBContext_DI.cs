using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace Business.Data.DB.DI
{
    public static class DBContext_DI
    {
        public static void Register<TContext>(WebApplicationBuilder builder)
            where TContext : DbContext
        {
            string dbContextName = typeof(TContext).Name;
            var services = builder.Services;
            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });
            var logger = loggerFactory.CreateLogger("DBContext_DI");


            try
            {
                var envKey = builder.Environment.IsProduction() ? "Prod" : "Dev";
                var dbConnStr = builder.Configuration[$"Config.Local:ConnectionStrings:{dbContextName}:{envKey}"];

                if (string.IsNullOrEmpty(dbConnStr))
                {
                    throw new InvalidOperationException($"Connection string not found for key: {envKey}");
                }

                if (builder.Environment.IsProduction())
                {
                    var missingVars = new List<string>();

                    string? db_host = Environment.GetEnvironmentVariable("DB_HOST");
                    if (string.IsNullOrEmpty(db_host)) missingVars.Add("DB_HOST");

                    string? db_name = Environment.GetEnvironmentVariable("DB_NAME");
                    if (string.IsNullOrEmpty(db_name)) missingVars.Add("DB_NAME");

                    string? db_user = Environment.GetEnvironmentVariable("DB_USER");
                    if (string.IsNullOrEmpty(db_user)) missingVars.Add("DB_USER");

                    string? db_password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                    if (string.IsNullOrEmpty(db_password)) missingVars.Add("DB_PASSWORD");

                    if (missingVars.Any())
                    {
                        throw new InvalidOperationException(
                            $"Missing required DB environment variables: {string.Join(", ", missingVars)}"
                        );
                    }

                    if (string.IsNullOrEmpty(db_host) || string.IsNullOrEmpty(db_name) ||
                        string.IsNullOrEmpty(db_user) || string.IsNullOrEmpty(db_password))
                    {
                        throw new InvalidOperationException("One or more required database environment variables are missing.");
                    }

                    dbConnStr = dbConnStr
                        .Replace("${DB_HOST}", db_host)
                        .Replace("${DB_NAME}", db_name)
                        .Replace("${DB_USER}", db_user)
                        .Replace("${DB_PASSWORD}", db_password);

                    logger.LogInformation("----> Current ENV: {Env} ", builder.Environment.EnvironmentName);
                    logger.LogInformation("--> DB_HOST: {DB_HOST}", db_host);
                    logger.LogInformation("--> DB_NAME: {DB_NAME}", db_name);
                    logger.LogInformation("--> DB_USER: {DB_USER}", db_user);
                    logger.LogInformation("--> DB_PASSWORD: {Masked}", string.IsNullOrEmpty(db_password) ? "NULL" : "***");
                }

                var dbConnStr_Name = $"{dbContextName}_DbConnStr";

                // Remove unused and potentionaly confusing appsettings dev & prod conn strings.
                // Add conn str into higher-priority in-memory collection to override appsettings and config:
                var overrideSettings = new Dictionary<string, string?>
                {
                    { dbConnStr_Name, dbConnStr },
                    { $"Config.Local:ConnectionStrings:{dbContextName}:Prod", string.Empty },
                    { $"Config.Local:ConnectionStrings:{dbContextName}:Dev", string.Empty }
                };
                builder.Configuration.AddInMemoryCollection(overrideSettings);


                logger.LogInformation("Connection string successfully resolved for {Context}", dbContextName);

                services.AddDbContext<TContext>(options =>
                    options.UseSqlServer(
                        builder.Configuration[dbConnStr_Name],
                        sql => sql.EnableRetryOnFailure())
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to register DbContext: {dbContextName}");
                throw;
            }
        }
    }
}

