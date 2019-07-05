using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using PickUpSports.DAL;
using PickUpSports.DAL.Repositories;
using PickUpSports.GoogleApi;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Services;
using RestSharp;

namespace PickUpSports
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            // Register all controllers in our MVC project 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register RestClient for Places API with base URL 
            builder.Register(x =>
                    new RestClient($"https://maps.googleapis.com/maps/api/place"))
                .Keyed<IRestClient>("GooglePlaces");

            // Get Google API key from app settings
            var placesKey = System.Web.Configuration.WebConfigurationManager.AppSettings["GooglePlacesApiKey"];

            // Register API client class with the RestClient and API key as a parameter
            builder.Register(x => new PlacesApiClient(
                x.ResolveKeyed<IRestClient>("GooglePlaces"), placesKey
            )).As<IPlacesApiClient>();
            
            //Grabbing email credentials from app settings 
            var emailAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["GMailUsername"];
            var emailPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["GMailPassword"];

            //Registering Network credentials with email and password
            builder.Register(x => new NetworkCredential(emailAddress, emailPassword))
                .AsSelf()
                .InstancePerRequest();

            //Setting email host and port number for Google
            var host = "smtp.gmail.com";
            var port = 587;

            //Register SMTP Client with the values 
            builder.Register(x => new SmtpClient(host, port))
                .AsSelf()
                .InstancePerRequest();

            // Register PickUpContext for database
            builder.Register(context => new PickUpContext())
                .AsSelf()
                .InstancePerRequest();

            // Register services
            builder.RegisterType<VenueService>().As<IVenueService>();
            builder.RegisterType<ContactService>().As<IContactService>();
            builder.RegisterType<GMailService>().As<IGMailService>();
            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<VenueOwnerServices>().As<IVenueOwnerService>();

            // Register repositories
            builder.RegisterType<ContactRepository>().As<IContactRepository>();
            builder.RegisterType<TimePreferenceRepository>().As<ITimePreferenceRepository>();
            builder.RegisterType<SportPreferenceRepository>().As<ISportPreferenceRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<SportRepository>().As<ISportRepository>();
            builder.RegisterType<PickUpGameRepository>().As<IPickUpGameRepository>();
            builder.RegisterType<GameRepository>().As<IGameRepository>();
            builder.RegisterType<VenueRepository>().As<IVenueRepository>();
            builder.RegisterType<BusinessHoursRepository>().As<IBusinessHoursRepository>();
            builder.RegisterType<LocationRepository>().As<ILocationRepository>();
            builder.RegisterType<VenueOwnerRepository>().As<IVenueOwnerRepository>();
            builder.RegisterType<GameStatusRepository>().As<IGameStatusRepository>();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();

            var container = builder.Build();

            // Resolve all registrations above 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
