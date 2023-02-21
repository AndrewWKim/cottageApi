﻿// <auto-generated />
using System;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CottageApi.Data.Migrations
{
    [DbContext(typeof(CottageDbContext))]
    [Migration("20210608125759_RemoveIdeaTitle")]
    partial class RemoveIdeaTitle
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarLicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CardPan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("PurchaseTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("CanPay")
                        .HasColumnType("bit");

                    b.Property<bool>("CanVote")
                        .HasColumnType("bit");

                    b.Property<int>("ClientType")
                        .HasColumnType("int");

                    b.Property<int>("CottageId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ITN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Passport")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PayCount")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistrationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ResidentTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CottageId");

                    b.HasIndex("ResidentTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CottageNewsId")
                        .HasColumnType("int");

                    b.Property<int?>("IdeaId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CottageNewsId");

                    b.HasIndex("IdeaId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.CommunalBill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BillGUID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommunalType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CottageBillingId")
                        .HasColumnType("int");

                    b.Property<double>("MeterData")
                        .HasColumnType("float");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CottageBillingId");

                    b.ToTable("CommunalBills");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Cottage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Area")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CottageNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MainSecurityContactId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cottages");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.CottageBilling", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BillingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CottageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CottageId");

                    b.ToTable("CottageBillings");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.CottageNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CottageNews");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Idea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VoteCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.IdeaRead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("IdeaReads");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.IdeaVote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CottageId")
                        .HasColumnType("int");

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.Property<int>("VoteType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CottageId");

                    b.HasIndex("IdeaId");

                    b.ToTable("IdeaVotes");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.NewSideSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SecurityPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NewSideSettings");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.NewsRead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CottageNewsId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CottageNewsId");

                    b.HasIndex("UserId");

                    b.ToTable("NewsReads");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.PassRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarBrand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarLicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarModel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("PassRequestType")
                        .HasColumnType("int");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VisitTime")
                        .HasColumnType("int");

                    b.Property<string>("VisitorName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("PassRequests");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.PivdenniyPaymentEffort", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("CommunalBillId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommunalBillId");

                    b.ToTable("PivdenniyPaymentEfforts");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.PivdenniyPaymentResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApprovalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Delay")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MerchantID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<string>("ProxyPan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseAction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rrn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Signature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TerminalID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalAmount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TranCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPCToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPCTokenExp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("XID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PivdenniyPaymentResponses");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.ResidentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ResidentTypes");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.UnreadNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CottageNewsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CottageNewsId");

                    b.ToTable("UnreadNews");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Car", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany("Cars")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Card", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany("Cards")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Client", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Cottage", "Cottage")
                        .WithMany("Clients")
                        .HasForeignKey("CottageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CottageApi.Core.Domain.Entities.ResidentType", "ResidentType")
                        .WithMany()
                        .HasForeignKey("ResidentTypeId");

                    b.HasOne("CottageApi.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Comment", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CottageApi.Core.Domain.Entities.CottageNews", "CottageNews")
                        .WithMany("Comments")
                        .HasForeignKey("CottageNewsId");

                    b.HasOne("CottageApi.Core.Domain.Entities.Idea", "Idea")
                        .WithMany("Comments")
                        .HasForeignKey("IdeaId");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.CommunalBill", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.CottageBilling", "CottageBilling")
                        .WithMany("CommunalBills")
                        .HasForeignKey("CottageBillingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.CottageBilling", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Cottage", "Cottage")
                        .WithMany()
                        .HasForeignKey("CottageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Device", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.Idea", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.IdeaRead", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Idea", "Idea")
                        .WithMany()
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.IdeaVote", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Cottage", "Cottage")
                        .WithMany()
                        .HasForeignKey("CottageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CottageApi.Core.Domain.Entities.Idea", "Idea")
                        .WithMany("IdeaVotes")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.NewsRead", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.CottageNews", "CottageNews")
                        .WithMany()
                        .HasForeignKey("CottageNewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CottageApi.Core.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.PassRequest", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.PivdenniyPaymentEffort", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.CommunalBill", "CommunalBill")
                        .WithMany("PivdenniyPaymentEfforts")
                        .HasForeignKey("CommunalBillId");
                });

            modelBuilder.Entity("CottageApi.Core.Domain.Entities.UnreadNews", b =>
                {
                    b.HasOne("CottageApi.Core.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CottageApi.Core.Domain.Entities.CottageNews", "CottageNews")
                        .WithMany()
                        .HasForeignKey("CottageNewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
