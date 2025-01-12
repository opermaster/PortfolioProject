namespace PortfolioProject
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			builder.Services.AddMemoryCache(); // Если используете кеширование
			builder.Services.AddHttpClient();  // Регистрируем IHttpClientFactory
			var app = builder.Build();

			if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404) {
                    context.Response.Redirect("/Home/Index");
                }
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
