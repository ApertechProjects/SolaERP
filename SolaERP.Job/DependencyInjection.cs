using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SolaERP.Job.Cbar;
using SolaERP.Job.EmailIsSent;
using SolaERP.Job.EmailIsSent1;
using SolaERP.Job.EmailIsSent2;
using SolaERP.Job.EmailIsSent3;

namespace SolaERP.Job
{
    public static class DependencyInjection
    {
        [Obsolete]
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

            services.ConfigureOptions<EmailSetupIsSent>();
        }

        [Obsolete]
        public static void AddRequestMailsForIsSentForAssignedBuyer(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<EmailSetupIsSentForAssignedBuyer>();
        }

        [Obsolete]
        public static void AddRequestMailsForIsSent2(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<EmailBackgroundJobSetupIsSent2>();
        }

        [Obsolete]
        public static void AddRequestMailsForIsSent3(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<EmailBackgroundJobSetupIsSent3>();
        }

        public static void AddCbarData(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<CbarBackgroundJobSetup>();
        }
    }
}
