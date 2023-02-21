using System.Linq;
using AutoMapper;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Dto.Ideas;
using CottageApi.Core.Domain.Dto.News;
using CottageApi.Core.Domain.Dto.Users;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Helpers;
using CottageApi.Models.Billing;
using CottageApi.Models.Cards;
using CottageApi.Models.Cars;
using CottageApi.Models.Clients;
using CottageApi.Models.Cottages;
using CottageApi.Models.Devices;
using CottageApi.Models.Ideas;
using CottageApi.Models.News;
using CottageApi.Models.PassRequests;
using CottageApi.Models.Users;

namespace CottageApi.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			MapClient();
			MapCottage();
			MapCar();
			MapUser();
			MapIdea();
			MapBilling();
			MapCard();
			MapPassRequest();
			MapDevice();
			MapNews();
		}

		private void MapClient()
		{
			CreateMap<Client, ClientViewModel>().ForMember(
					dest => dest.LoginName,
					opt => opt.MapFrom(src => src.User.Name))
                .ForMember(
                    dest => dest.BiometricsSignature,
                    opt => opt.MapFrom(src => src.User.BiometricsSignature));

			CreateMap<Client, EditClientModel>();
			CreateMap<EditClientModel, Client>();

			CreateMap<CreateClientModel, Client>();
		}

		private void MapNews()
		{
			CreateMap<CreateNewsModel, CottageNews>();
			CreateMap<EditNewsModel, CottageNews>();
			CreateMap<CottageNews, NewsViewModel>();
			CreateMap<CottageNews, ClientNewsViewDto>();
		}

		private void MapDevice()
		{
			CreateMap<CreateDeviceModel, Device>();
		}

		private void MapCottage()
		{
			CreateMap<Cottage, CottageViewModel>().ForMember(
					dest => dest.MainSecurityContactId,
					opt => opt.MapFrom(src => src.MainSecurityContactId.HasValue ? src.MainSecurityContactId : 0));
		}

		private void MapCar()
		{
			CreateMap<Car, CarViewModel>().ForMember(dest => dest.ClientFullName, opt => opt.MapFrom(src => src.Client.GetFullName()));
			CreateMap<CarViewModel, Car>();
		}

		private void MapUser()
		{
			CreateMap<CreateUserDto, CreateUserModel>();
			CreateMap<CreateUserModel, CreateUserDto>();

			CreateMap<CreateUserDto, User>()
				.ForMember(
					dest => dest.Name,
					opt => opt.MapFrom(
						src => src.Username));
		}

		private void MapIdea()
		{
			CreateMap<IdeaViewDto, IdeaViewModel>();
			CreateMap<CreateIdeaModel, Idea>();
			CreateMap<ClientIdeaViewDto, ClientIdeaViewModel>();
			CreateMap<Idea, ClientIdeaViewDto>().ForMember(
				dest => dest.IsVoted,
				opt => opt.Ignore());
			CreateMap<Idea, IdeaViewDto>();
			CreateMap<Idea, CreatorIdeaViewDto>();
			CreateMap<CreatorIdeaViewDto, CreatorIdeaViewModel>();
		}

		private void MapBilling()
		{
			CreateMap<CottageBilling, CottageBillingViewModel>()
				.ForMember(
					dest => dest.TotalPrice,
					opt => opt.MapFrom(src =>
						src.CommunalBills.Where(cb => cb.PaymentStatus == PaymentStatus.Unpaid).Sum(cb => cb.Price)));
			CreateMap<CommunalBill, CommunalBillViewModel>()
				.ForMember(
					dest => dest.CommunalType,
					opt => opt.MapFrom(src =>
						src.CommunalType == CommunalTypesUA.GetCommunalType("ok")
							? "Обслуживание кооператива"
							: src.CommunalType == CommunalTypesUA.GetCommunalType("water")
								? "Водоснабжение"
								: src.CommunalType == CommunalTypesUA.GetCommunalType("sewerage")
									? "Канализация"
									: src.CommunalType == CommunalTypesUA.GetCommunalType("electricity")
										? "Електроэнергия"
										: "Название не задано"));
			CreateMap<CommunalBill, AdminCommunalBillViewModel>()
				.ForMember(
					dest => dest.CommunalType,
					opt => opt.MapFrom(src =>
						src.CommunalType == CommunalTypesUA.GetCommunalType("ok")
							? "Обслуживание кооператива"
							: src.CommunalType == CommunalTypesUA.GetCommunalType("water")
								? "Водоснабжение"
								: src.CommunalType == CommunalTypesUA.GetCommunalType("sewerage")
									? "Канализация"
									: src.CommunalType == CommunalTypesUA.GetCommunalType("electricity")
										? "Електроэнергия"
										: "Название не задано"))
				.ForMember(dest => dest.CottageNumber, opt => opt.MapFrom(src => src.CottageBilling.Cottage.CottageNumber))
				.ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.CottageBilling.BillingDate.Month))
				.ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.CottageBilling.BillingDate.Year));
			CreateMap<CreateCottageBillingModel, CreateCottageBillingDto>();
			CreateMap<CreateCommunalBillModel, CreateCommunalBillDto>()
				.ForMember(dest => dest.MeterData, opt => opt.MapFrom(src => src.MeterDataEnd - src.MeterDataBegin))
				.ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.IsPaid ? PaymentStatus.Paid : PaymentStatus.Unpaid));
			CreateMap<CreateCommunalBillDto, CommunalBill>();
			CreateMap<CommunalBill, PayedCommunalBillsViewModel>();
		}

		private void MapCard()
		{
			CreateMap<Card, CardViewModel>();
			CreateMap<CardViewModel, Card>();
		}

		private void MapPassRequest()
		{
			CreateMap<PassRequest, PassRequestViewModel>()
				.ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.GetFullName()))
				.ForMember(dest => dest.CottageNumber, opt => opt.MapFrom(src => src.Client.Cottage.CottageNumber))
				.ForMember(dest => dest.VisitTime, opt => opt.MapFrom(src =>
					src.VisitTime == VisitTime.Morning
						? "Утром"
						: src.VisitTime == VisitTime.Afternoon
							? "Днем"
							: "Вечером"));
			CreateMap<PassRequest, PassRequestMobileViewModel>().ForMember(dest => dest.VisitTime, opt => opt.MapFrom(src =>
				src.VisitTime == VisitTime.Morning
					? "Утром"
					: src.VisitTime == VisitTime.Afternoon
						? "Днем"
						: "Вечером"));
		}
	}
}