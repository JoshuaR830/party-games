using System;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Chat.Balderdash;
using Chat.GameManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chat.Hubs;
using Chat.Pixenary;
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
            
            // var chain = new CredentialProfileStoreChain();
            // if (chain.TryGetProfile("aarrgghh", out var basicProfile))
            // {
            //     Console.WriteLine(basicProfile);
            // }
            
            // services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            // services.AddAWSService<IAmazonLambda>();
            
            // services.AddAWSService<IAmazonLambda>(new AWSOptions
            // {
            //     
            // });
            var thing = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
            var thing2 = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var thing3 = Environment.GetEnvironmentVariable("DOCKER_CERT_PATH");
            services.AddAWSService<IAmazonLambda>(new AWSOptions
            {
                Credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"), Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY")),
                Region = RegionEndpoint.EUWest2
            });
            
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
            services.AddSingleton<IShuffleHelper<WordData>, ShuffleHelper<WordData>>();
            services.AddSingleton<IShuffleHelper<BalderdashHub.GuessMade>, ShuffleHelper<BalderdashHub.GuessMade>>();
            services.AddSingleton<IWordCategoryHelper, WordCategoryHelper>();
            services.AddSingleton<IBalderdashScoreCalculator, BalderdashScoreCalculator>();
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
                routes.MapHub<BalderdashHub>("/balderdashHub");
            });

            app.UseMvc();
        }
    }
}
