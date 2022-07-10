using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;

namespace Customers.Api.Config;

public static class PrometheusConfig
{
    public static void UsePrometheus(this IApplicationBuilder app)
    {
        app.UseMetricServer();
    }
}
