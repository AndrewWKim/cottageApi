using CottageApi.Core.Configurations;
using CottageApi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CottageApi.Data.Context
{
	public class CottageDbContext : DbContext, ICottageDbContext
	{
		public CottageDbContext()
		{
		}

		public CottageDbContext(DbContextOptions<CottageDbContext> options, IConfiguration configuration)
			: base(options)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<Client> Clients { get; set; }

		public virtual DbSet<Comment> Comments { get; set; }

		public virtual DbSet<CommunalBill> CommunalBills { get; set; }

		public virtual DbSet<Cottage> Cottages { get; set; }

		public virtual DbSet<CottageBilling> CottageBillings { get; set; }

		public virtual DbSet<CottageNews> CottageNews { get; set; }

		public virtual DbSet<Idea> Ideas { get; set; }

		public virtual DbSet<PassRequest> PassRequests { get; set; }

		public virtual DbSet<UnreadNews> UnreadNews { get; set; }

		public virtual DbSet<ResidentType> ResidentTypes { get; set; }

		public virtual DbSet<Car> Cars { get; set; }

		public virtual DbSet<IdeaVote> IdeaVotes { get; set; }

		public virtual DbSet<Card> Cards { get; set; }

		public virtual DbSet<IdeaRead> IdeaReads { get; set; }

		public virtual DbSet<Device> Devices { get; set; }

		public virtual DbSet<PivdenniyPaymentResponse> PivdenniyPaymentResponses { get; set; }

		public virtual DbSet<PivdenniyPaymentEffort> PivdenniyPaymentEfforts { get; set; }

		public virtual DbSet<NewsRead> NewsReads { get; set; }

		public virtual DbSet<NewSideSettings> NewSideSettings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureClient(modelBuilder);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(GetCottageConfig().Connections.CottageConnectionString);
		}

		private Config GetCottageConfig()
		{
			var config = new Config();
			Configuration.GetSection("CottageAPI").Bind(config);

			return config;
		}

		private static void ConfigureClient(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Client>().Property(x => x.FirstName).HasMaxLength(50);
			modelBuilder.Entity<Client>().Property(x => x.LastName).HasMaxLength(50);
		}
	}
}