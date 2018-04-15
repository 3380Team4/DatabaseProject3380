using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ThemeParkApplication.Models
{
    public partial class themeparkdbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Attractions> Attractions { get; set; }
        public virtual DbSet<AttractionStatusTable> AttractionStatusTable { get; set; }
        public virtual DbSet<AttractionTypeTable> AttractionTypeTable { get; set; }
        public virtual DbSet<Closures> Closures { get; set; }
        public virtual DbSet<Concessions> Concessions { get; set; }
        public virtual DbSet<ConcessionStatusTable> ConcessionStatusTable { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<ItemTypeTable> ItemTypeTable { get; set; }
        public virtual DbSet<JobTitleTable> JobTitleTable { get; set; }
        public virtual DbSet<Maintenance> Maintenance { get; set; }
        public virtual DbSet<Merchandise> Merchandise { get; set; }
        public virtual DbSet<OrderTypeTable> OrderTypeTable { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ReasonTable> ReasonTable { get; set; }
        public virtual DbSet<StoreTypeTable> StoreTypeTable { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<WorkStatusTable> WorkStatusTable { get; set; }

        public themeparkdbContext(DbContextOptions<themeparkdbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attractions>(entity =>
            {
                entity.HasKey(e => e.AttractionId);

                entity.HasIndex(e => e.AttractionName)
                    .HasName("UQ__Attracti__7A367D64E2AF49C3")
                    .IsUnique();

                entity.Property(e => e.AttractionId)
                    .HasColumnName("Attraction_ID")
                    .HasColumnType("char(9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AgeRequirement).HasColumnName("Age_Requirement");

                entity.Property(e => e.AttractionName)
                    .IsRequired()
                    .HasColumnName("Attraction_Name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AttractionStatus).HasColumnName("Attraction_Status");

                entity.Property(e => e.AttractionType).HasColumnName("Attraction_Type");

                entity.Property(e => e.HeightRequirement).HasColumnName("Height_Requirement");

                entity.Property(e => e.ManagerId)
                    .IsRequired()
                    .HasColumnName("Manager_ID")
                    .HasColumnType("char(9)");

                entity.HasOne(d => d.AttractionStatusNavigation)
                    .WithMany(p => p.Attractions)
                    .HasForeignKey(d => d.AttractionStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attractions_Attraction_Status_Table");

                entity.HasOne(d => d.AttractionTypeNavigation)
                    .WithMany(p => p.Attractions)
                    .HasForeignKey(d => d.AttractionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attractions_Attraction_Type_Table");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Attractions)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Attractions_Employees");
            });

            modelBuilder.Entity<AttractionStatusTable>(entity =>
            {
                entity.HasKey(e => e.AttractionStatusIndex);

                entity.ToTable("Attraction_Status_Table");

                entity.HasIndex(e => e.AttractionStatusIndex)
                    .HasName("UQ__Attracti__609D8AA99022F52B")
                    .IsUnique();

                entity.Property(e => e.AttractionStatusIndex)
                    .HasColumnName("Attraction_Status_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttractionStatus)
                    .IsRequired()
                    .HasColumnName("Attraction_Status")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AttractionTypeTable>(entity =>
            {
                entity.HasKey(e => e.AttractionTypeIndex);

                entity.ToTable("Attraction_Type_Table");

                entity.HasIndex(e => e.AttractionTypeIndex)
                    .HasName("UQ__Attracti__F658F5634A188E4D")
                    .IsUnique();

                entity.Property(e => e.AttractionTypeIndex)
                    .HasColumnName("Attraction_Type_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttractionType)
                    .IsRequired()
                    .HasColumnName("Attraction_Type")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Closures>(entity =>
            {
                entity.HasKey(e => e.ClosureId);

                entity.Property(e => e.ClosureId)
                    .HasColumnName("Closure_ID")
                    .HasColumnType("char(9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttrId)
                    .HasColumnName("Attr_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.ConcId)
                    .HasColumnName("Conc_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.DateClosure)
                    .HasColumnName("Date_Closure")
                    .HasColumnType("date");

                entity.Property(e => e.DurationClosure)
                    .IsRequired()
                    .HasColumnName("Duration_Closure")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.Closures)
                    .HasForeignKey(d => d.AttrId)
                    .HasConstraintName("FK_Closures_Attractions");

                entity.HasOne(d => d.Conc)
                    .WithMany(p => p.Closures)
                    .HasForeignKey(d => d.ConcId)
                    .HasConstraintName("FK_Closures_Concessions");

                entity.HasOne(d => d.ReasonNavigation)
                    .WithMany(p => p.Closures)
                    .HasForeignKey(d => d.Reason)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Closures_Reason_Table");
            });

            modelBuilder.Entity<Concessions>(entity =>
            {
                entity.HasKey(e => e.ConcessionId);

                entity.Property(e => e.ConcessionId)
                    .HasColumnName("Concession_ID")
                    .HasColumnType("char(9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClosingTime).HasColumnName("Closing_Time");

                entity.Property(e => e.ConcessionName)
                    .IsRequired()
                    .HasColumnName("Concession_Name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConcessionStatus).HasColumnName("Concession_Status");

                entity.Property(e => e.ManagerId)
                    .IsRequired()
                    .HasColumnName("Manager_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.OpeningTime).HasColumnName("Opening_Time");

                entity.Property(e => e.StoreType).HasColumnName("Store_Type");

                entity.HasOne(d => d.ConcessionStatusNavigation)
                    .WithMany(p => p.Concessions)
                    .HasForeignKey(d => d.ConcessionStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Concessions_Concession_Status_Table");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Concessions)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Concessions_Employees");

                entity.HasOne(d => d.StoreTypeNavigation)
                    .WithMany(p => p.Concessions)
                    .HasForeignKey(d => d.StoreType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Concessions_Store_Type_Table");
            });

            modelBuilder.Entity<ConcessionStatusTable>(entity =>
            {
                entity.HasKey(e => e.ConcessionStatusIndex);

                entity.ToTable("Concession_Status_Table");

                entity.HasIndex(e => e.ConcessionStatusIndex)
                    .HasName("UQ__Concessi__2DC995F4071A2B03")
                    .IsUnique();

                entity.Property(e => e.ConcessionStatusIndex)
                    .HasColumnName("Concession_Status_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConcessionStatus)
                    .IsRequired()
                    .HasColumnName("Concession_Status")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("Customer_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerFirstName)
                    .IsRequired()
                    .HasColumnName("Customer_First_Name")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerLastName)
                    .IsRequired()
                    .HasColumnName("Customer_Last_Name")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.LastVisited)
                    .HasColumnName("Last_Visited")
                    .HasColumnType("date");

                entity.Property(e => e.NumberVisits).HasColumnName("Number_Visits");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("Employee_ID")
                    .HasColumnType("char(9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateBirth)
                    .HasColumnName("Date_Birth")
                    .HasColumnType("date");

                entity.Property(e => e.EndDate)
                    .HasColumnName("End_Date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_Name")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.JobTitle).HasColumnName("Job_Title");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last_Name")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_Date")
                    .HasColumnType("date");

                entity.Property(e => e.SupervisorId)
                    .HasColumnName("Supervisor_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.WorksAtAttr)
                    .HasColumnName("Works_AT_Attr")
                    .HasColumnType("char(9)");

                entity.Property(e => e.WorksAtConc)
                    .HasColumnName("Works_At_Conc")
                    .HasColumnType("char(9)");

                entity.HasOne(d => d.JobTitleNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.JobTitle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Job_Title_Table");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.InverseSupervisor)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_Employess_Employess");

                entity.HasOne(d => d.WorksAtAttrNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.WorksAtAttr)
                    .HasConstraintName("FK_Employess_Attractions");

                entity.HasOne(d => d.WorksAtConcNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.WorksAtConc)
                    .HasConstraintName("FK_Employess_Concessions");
            });

            modelBuilder.Entity<ItemTypeTable>(entity =>
            {
                entity.HasKey(e => e.ItemTypeIndex);

                entity.ToTable("Item_Type_Table");

                entity.HasIndex(e => e.ItemTypeIndex)
                    .HasName("UQ__Item_Typ__AC3804268261A328")
                    .IsUnique();

                entity.Property(e => e.ItemTypeIndex)
                    .HasColumnName("Item_Type_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.ItemType)
                    .IsRequired()
                    .HasColumnName("Item_Type")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobTitleTable>(entity =>
            {
                entity.HasKey(e => e.JobTitleIndex);

                entity.ToTable("Job_Title_Table");

                entity.HasIndex(e => e.JobTitleIndex)
                    .HasName("UQ__Job_Titl__3C0DD398AEA324DE")
                    .IsUnique();

                entity.Property(e => e.JobTitleIndex)
                    .HasColumnName("Job_Title_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasColumnName("Job_Title")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(e => e.WorkOrderId);

                entity.Property(e => e.WorkOrderId)
                    .HasColumnName("Work_Order_ID")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AttrId)
                    .HasColumnName("Attr_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.ConcId)
                    .HasColumnName("Conc_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.MaintenanceEmployeeId)
                    .IsRequired()
                    .HasColumnName("Maintenance_Employee_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.OrderType).HasColumnName("Order_Type");

                entity.Property(e => e.WorkFinishDate)
                    .HasColumnName("Work_Finish_Date")
                    .HasColumnType("date");

                entity.Property(e => e.WorkStartDate)
                    .HasColumnName("Work_Start_Date")
                    .HasColumnType("date");

                entity.Property(e => e.WorkStatus).HasColumnName("Work_Status");

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.AttrId)
                    .HasConstraintName("FK_Maintenance_Attractions");

                entity.HasOne(d => d.Conc)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.ConcId)
                    .HasConstraintName("FK_Maintenance_Concessions");

                entity.HasOne(d => d.MaintenanceEmployee)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.MaintenanceEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenance_Employees");

                entity.HasOne(d => d.OrderTypeNavigation)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.OrderType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenance_Order_Type_Table");

                entity.HasOne(d => d.WorkStatusNavigation)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.WorkStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Maintenance_Work_Status_Table");
            });

            modelBuilder.Entity<Merchandise>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId)
                    .HasColumnName("Item_ID")
                    .HasColumnType("char(9)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttrId)
                    .HasColumnName("Attr_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.ConcId)
                    .HasColumnName("Conc_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasColumnName("Item_Name")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ItemType).HasColumnName("Item_Type");

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.Merchandise)
                    .HasForeignKey(d => d.AttrId)
                    .HasConstraintName("FK_Merchandise_Attractions");

                entity.HasOne(d => d.Conc)
                    .WithMany(p => p.Merchandise)
                    .HasForeignKey(d => d.ConcId)
                    .HasConstraintName("FK_Merchandise_Concessions");

                entity.HasOne(d => d.ItemTypeNavigation)
                    .WithMany(p => p.Merchandise)
                    .HasForeignKey(d => d.ItemType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Merchadise_Item_Type_Table");
            });

            modelBuilder.Entity<OrderTypeTable>(entity =>
            {
                entity.HasKey(e => e.OrderTypeIndex);

                entity.ToTable("Order_Type_Table");

                entity.Property(e => e.OrderTypeIndex)
                    .HasColumnName("Order_Type_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasColumnName("Order_Type")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreation)
                    .HasColumnName("Date_Creation")
                    .HasColumnType("date");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("Employee_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.LastLoggedIn)
                    .HasColumnName("Last_Logged_In")
                    .HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(88)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnType("binary(64)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Profile)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_Employee");
            });

            modelBuilder.Entity<ReasonTable>(entity =>
            {
                entity.HasKey(e => e.ReasonIndex);

                entity.ToTable("Reason_Table");

                entity.HasIndex(e => e.ReasonIndex)
                    .HasName("UQ__Reason_T__53287D49C8EFC418")
                    .IsUnique();

                entity.Property(e => e.ReasonIndex)
                    .HasColumnName("Reason_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StoreTypeTable>(entity =>
            {
                entity.HasKey(e => e.StoreTypeIndex);

                entity.ToTable("Store_Type_Table");

                entity.HasIndex(e => e.StoreTypeIndex)
                    .HasName("UQ__Store_Ty__1E2EF40273004534")
                    .IsUnique();

                entity.Property(e => e.StoreTypeIndex)
                    .HasColumnName("Store_Type_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.StoreType)
                    .IsRequired()
                    .HasColumnName("Store_Type")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => new { e.TransactionId, e.DateOfSale });

                entity.Property(e => e.TransactionId)
                    .HasColumnName("Transaction_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.DateOfSale)
                    .HasColumnName("Date_Of_Sale")
                    .HasColumnType("date");

                entity.Property(e => e.GuestId).HasColumnName("Guest_ID");

                entity.Property(e => e.MerchId)
                    .IsRequired()
                    .HasColumnName("Merch_ID")
                    .HasColumnType("char(9)");

                entity.Property(e => e.SaleAmount)
                    .HasColumnName("Sale_Amount")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SellerEmployeeId)
                    .IsRequired()
                    .HasColumnName("Seller_Employee_ID")
                    .HasColumnType("char(9)");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Customers");

                entity.HasOne(d => d.Merch)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.MerchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Merchandise");

                entity.HasOne(d => d.SellerEmployee)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.SellerEmployeeId)
                    .HasConstraintName("FK_Transactions_Employees");
            });

            modelBuilder.Entity<WorkStatusTable>(entity =>
            {
                entity.HasKey(e => e.WorkStatusIndex);

                entity.ToTable("Work_Status_Table");

                entity.Property(e => e.WorkStatusIndex)
                    .HasColumnName("Work_Status_Index")
                    .ValueGeneratedNever();

                entity.Property(e => e.WorkStatus)
                    .HasColumnName("Work_Status")
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });
        }
    }
}
