   # ASP.NET Core

   Call static class `DbInitializer` with `Initialize()` method:

   ```cs
   public class Startup
   {
       public void Configure(IApplicationBuilder app, IHostingEnvironment env)
       {
           using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
           {
               var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
               DbInitializer.Initialize(context);   // 👈
           }
       }
   }
   ```