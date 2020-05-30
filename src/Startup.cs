using Chat.GameManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chat.Hubs;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IGameManager, GameManager.GameManager>();
            services.AddSingleton<IScoreHelper, ScoreHelper>();
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IFilenameHelper, FilenameHelper>();
            services.AddSingleton<IWebDictionaryRequestHelper, WebDictionaryRequestHelper>();
            services.AddSingleton<IWebRequestHelper, WebRequestHelper>();
            services.AddSingleton<IWordDefinitionHelper, WordDefinitionHelper>();
            services.AddSingleton<IWordExistenceHelper, WordExistenceHelper>();
            services.AddSingleton<IWordHelper, WordHelper>();
            services.AddSingleton<IWordService, WordService>();
            services.AddSingleton<ITemporaryDefinitionHelper, TemporaryDefinitionHelper>();
            services.AddSingleton<IJoinRoomHelper, JoinRoomHelper>();
            services.AddSingleton<IShuffleHelper<string>, ShuffleHelper<string>>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*");
                    });
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<LettersHub>("/lettersHub");
                routes.MapHub<PixenaryHub>("/pixenaryHub");
            });

            app.UseMvc();
        }
    }
}
