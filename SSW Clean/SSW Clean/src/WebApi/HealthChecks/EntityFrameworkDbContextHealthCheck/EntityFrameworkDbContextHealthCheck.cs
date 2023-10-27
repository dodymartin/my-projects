﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace SSW_Clean.WebApi.HealthChecks.EntityFrameworkDbContextHealthCheck;
public sealed class EntityFrameworkDbContextHealthCheck<TContext> : IHealthCheck where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IOptionsMonitor<EntityFrameworkDbContextHealthCheckOptions<TContext>> _options;

    public EntityFrameworkDbContextHealthCheck(TContext dbContext, IOptionsMonitor<EntityFrameworkDbContextHealthCheckOptions<TContext>> options)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var options = _options.Get(context.Registration.Name);
        var testQuery = options.TestQuery;
        var data = new Dictionary<string, object>();

        // Always make sure we can at least connect to the database
        var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
        if (!canConnect)
        {
            return new HealthCheckResult(HealthStatus.Unhealthy, "Failed to connect to Database - Please check Connection String details or Network configuration.");
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            // Run the custom DbContext Query
            if (testQuery == null)
            {
                return new HealthCheckResult(HealthStatus.Healthy);
            }

            var result = await testQuery(_dbContext, cancellationToken);
            return new HealthCheckResult(result.Success ? HealthStatus.Healthy : HealthStatus.Unhealthy, result.Message, exception: result.Exception, data);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(HealthStatus.Unhealthy, "Failed to execute Custom DbContext Query", ex, data);
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}