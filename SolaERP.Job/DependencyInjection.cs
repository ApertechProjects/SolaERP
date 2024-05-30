using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace SolaERP.Job
{
    public static class DependencyInjection
    {
        public static void AddRequestMailsForIsSent(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<EmailBackgroundJobSetupIsSent>();
        }

        //public static void AddRequestMailsForIsSent2(this IServiceCollection services)
        //{
        //    services.AddQuartz(options =>
        //    {
        //        options.UseMicrosoftDependencyInjectionJobFactory();
        //    });

        //    services.AddQuartzHostedService(options =>
        //    {
        //        options.WaitForJobsToComplete = true;
        //    });

        //    services.ConfigureOptions<EmailBackgroundJobSetupIsSent>();
        //}
    }
}
