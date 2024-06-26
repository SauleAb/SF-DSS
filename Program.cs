using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;
using Microsoft.EntityFrameworkCore;
using SF_DSS.Data;
using SF_DSS.Models.Services;
using SF_DSS;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Options;
namespace SF_DSS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

            builder.Services.AddScoped<IChatbotService, ChatbotService>();
            builder.Services.AddScoped<HttpClient>();
            builder.Services.AddRequestTimeouts(options => {
                options.DefaultPolicy =
                    new RequestTimeoutPolicy { Timeout = TimeSpan.FromMilliseconds(15000000) };
            });
            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}
