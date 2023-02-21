using System.Threading;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CottageApi.Data.Context
{
	public interface ICottageDbContext
	{
		DbSet<User> Users { get; set; }

		DbSet<Client> Clients { get; set; }

		DbSet<Comment> Comments { get; set; }

		DbSet<CommunalBill> CommunalBills { get; set; }

		DbSet<Cottage> Cottages { get; set; }

		DbSet<CottageBilling> CottageBillings { get; set; }

		DbSet<CottageNews> CottageNews { get; set; }

		DbSet<Idea> Ideas { get; set; }

		DbSet<PassRequest> PassRequests { get; set; }

		DbSet<UnreadNews> UnreadNews { get; set; }

		DbSet<ResidentType> ResidentTypes { get; set; }

		DbSet<Car> Cars { get; set; }

		DbSet<IdeaVote> IdeaVotes { get; set; }

		DbSet<Card> Cards { get; set; }

		DbSet<IdeaRead> IdeaReads { get; set; }

		DbSet<Device> Devices { get; set; }

		DbSet<PivdenniyPaymentResponse> PivdenniyPaymentResponses { get; set; }

		DbSet<PivdenniyPaymentEffort> PivdenniyPaymentEfforts { get; set; }

		DbSet<NewsRead> NewsReads { get; set; }

		DbSet<NewSideSettings> NewSideSettings { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
