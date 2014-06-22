using System;
using System.Collections.Generic;
using System.IO;
using Fredis;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ServiceImplementations.Common {
    public class AppHost : AppHostBase {
        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("ServiceStack minimal template", typeof(YoService).Assembly) { }

        public override void Configure(Funq.Container container) {
            //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            Plugins.Add(new RequestLogsFeature());
            Plugins.Add(new CorsFeature());
            Plugins.Add(new PostmanFeature());

            //Load environment config from text file if exists
            var liveSettings = "~/appsettings.txt".MapHostAbsolutePath();
            var appSettings = File.Exists(liveSettings)
                ? (IAppSettings)new TextFileSettings(liveSettings)
                : new AppSettings();

            SetConfig(new HostConfig {
                DebugMode = appSettings.Get("DebugMode", true),
                StripApplicationVirtualPath = appSettings.Get("StripApplicationVirtualPath", false),
                AdminAuthSecret = appSettings.GetString("AuthSecret"),
            });

            var mainConnection = appSettings.GetString("MainConnectionString");
            var shardConnections = appSettings.GetString("ShardsConnectionStrings");

            try { // TODO regex multiline
                // "0 => Server=localhost;Database=fredis.0;Uid=test;Pwd=test;";
                var shards = shardConnections
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(line => {
                        var pair = line.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                        return new KeyValuePair<ushort, string>(ushort.Parse(pair[0].Trim()), pair[1].Trim());
                    });

                // TODO Poco persitor checks the main db abd every shard
                // When that fails there is no clear message what is happening
                // just a 500 Internal server error - need to do a better error handling/logging
                var pocoPersistor = new BasePocoPersistor(MySqlDialect.Provider, mainConnection, shards);
                container.Register<IPocoPersistor>(pocoPersistor);
                container.Register<IDbConnectionFactory>(pocoPersistor.DbFactory);
            } catch (Exception) {
                throw new InvalidOperationException("conenction strings");
            }


            // TODO Blob persistor + Redis
            // TODO Cache - look up new SS's impls or implement 
            // TODO on top of .NET's built+Redis (two layers)
            container.Register<ICacheClient>(new MemoryCacheClient());

            // T <-> byte[] serializer
            container.Register<ISerializer>(new JsonSerializer());
            var redis = new Redis("localhost", "Yo.NET") {
                Serializer = container.Resolve<ISerializer>()
            };
            container.Register(redis);

            Plugins.Add(new AuthFeature(() => new UserSession(),
                new IAuthProvider[] {
                    new CredentialsAuthProvider(appSettings)
                }) {
                    HtmlRedirect = "~/",
                    IncludeRegistrationService = true
                });


            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()) {
                    MaxLoginAttempts = appSettings.Get("MaxLoginAttempts", 5)
                });

            //TODO
            container.Resolve<IUserAuthRepository>().InitSchema();

            //Register all your dependencies
            container.Register(new TodoRepository());

        }

        public static void Start() {
            new AppHost().Init();
        }
    }

    public class AppStarter {
        public static void Start() {
            AppHost.Start();
        }
    }
}