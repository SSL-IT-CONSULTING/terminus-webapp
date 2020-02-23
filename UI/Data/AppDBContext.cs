

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using terminus.shared.models;

namespace terminus_webapp.Data
{
    public class AppDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Revenue> Revenues { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<GLAccount> GLAccounts { get; set; }
    
        public DbSet<JournalEntryHdr> JournalEntriesHdr { get; set; }
        public DbSet<JournalEntryDtl> JournalEntriesDtl { get; set; }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyDirectory> PropertyDirectory { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Billing> Billings { get; set; }

        public DbSet<BillingLineItem> BillingLineItems { get; set; }

        public DbSet<ReportParameterViewModel> ReportParameterViewModels { get; set; }

        public DbSet<ReferenceViewModal> ReferenceViewModals { get; set; }

        public DbSet<CompanyDefault> CompanyDefaults { get; set; }

        public DbSet<DocumentIdTable> DocumentIdTable { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GLAccount>()
               .Property(c => c.balance).HasColumnType("decimal(18,4)");

            builder.Entity<Revenue>()
          .Property(c => c.amount).HasColumnType("decimal(18,4)");

            builder.Entity<Revenue>()
.Property(c => c.taxAmount).HasColumnType("decimal(18,4)");

            builder.Entity<RevenueLineItem>()
.Property(c => c.amount).HasColumnType("decimal(18,4)");

            builder.Entity<Expense>()
          .Property(c => c.amount).HasColumnType("decimal(18,4)");

            builder.Entity<Expense>()
      .Property(c => c.taxAmount).HasColumnType("decimal(18,4)");

            builder.Entity<CheckDetails>()
.Property(c => c.amount).HasColumnType("decimal(18,4)");

            builder.Entity<PropertyDirectory>()
.Property(c => c.monthlyRate).HasColumnType("decimal(18,4)");
           
            builder.Entity<PropertyDirectory>()
.Property(c => c.ratePerSQM).HasColumnType("decimal(18,4)");

            builder.Entity<PropertyDirectory>()
.Property(c => c.ratePerSQMAssocDues).HasColumnType("decimal(18,4)");

            builder.Entity<PropertyDirectory>()
.Property(c => c.penaltyPct).HasColumnType("decimal(18,4)");

            builder.Entity<PropertyDirectory>()
.Property(c => c.associationDues).HasColumnType("decimal(18,4)");

            builder.Entity<PropertyDirectory>()
.Property(c => c.totalBalance).HasColumnType("decimal(18,4)");

            builder.Entity<Property>()
.Property(c => c.areaInSqm).HasColumnType("decimal(18,4)");

            builder.Entity<GLAccount>()
            .HasIndex("companyId", "accountCode").IsUnique(true);

            builder.Entity<GLAccount>()
           .HasIndex(a=>a.rowOrder);


            builder.Entity<Vendor>()
           .HasIndex(a => a.rowOrder);


            builder.Entity<Billing>()
           .HasIndex("companyId", "documentId").IsUnique(true);

            builder.Entity<Billing>()
.Property(c => c.amountPaid).HasColumnType("decimal(18,4)");
            
            builder.Entity<Billing>()
.Property(c => c.totalAmount).HasColumnType("decimal(18,4)");

            builder.Entity<Billing>()
.Property(c => c.balance).HasColumnType("decimal(18,4)");
            
            builder.Entity<BillingLineItem>()
.Property(c => c.amount).HasColumnType("decimal(18,4)");

            builder.Entity<BillingLineItem>()
.Property(c => c.amountPaid).HasColumnType("decimal(18,4)");

            builder.Entity<Revenue>()
.Property(c => c.beforeTax).HasColumnType("decimal(18,4)");

            builder.Entity<Expense>()
.Property(c => c.beforeTax).HasColumnType("decimal(18,4)");

            builder.Entity<Billing>()
            .HasIndex(a=>a.MonthYear);

            builder.Entity<DocumentIdTable>()
          .HasKey("IdKey", "CompanyId");
        }
    }
}
