using ESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<CUSTOMER> CUSTOMER { get; set; }
        public DbSet<ESG_CHECKLIST_ITEM> ESG_CHECKLIST_ITEM { get; set; }
        public DbSet<ESG_CHECKLIST_RESPONSE> ESG_CHECKLIST_RESPONSE { get; set; }
        public DbSet<ESG_CHECKLIST_ITEM_SCORE> ESG_CHECKLIST_ITEM_SCORE { get; set; }
        public DbSet<LOAN_APPLICATION> LOAN_APPLICATION { get; set; }
        public DbSet<ESG_CHECKLIST_ASSESSMENT> ESG_CHECKLIST_ASSESSMENT { get; set; }
        public DbSet<ESG_CHECKLIST_SUMMARY> ESG_CHECKLIST_SUMMARY { get; set; }
        public DbSet<APPROVAL_STATUS> APPROVAL_STATUS { get; set; }
    }
}