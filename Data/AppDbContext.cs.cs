using Microsoft.EntityFrameworkCore;
using backend_inventory.Models;

namespace backend_inventory.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Stock
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Adjustment> Adjustments { get; set; }
    public DbSet<StockOpname> StockOpnames { get; set; }
    public DbSet<StockOpnameItem> StockOpnameItems { get; set; }

    // Procurement
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<ReceiveGoods> ReceiveGoods { get; set; }
    public DbSet<ReceiveGoodsItem> ReceiveGoodsItems { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // PR → PO (1-1)
        builder.Entity<PurchaseRequest>()
            .HasOne(pr => pr.PurchaseOrder)
            .WithOne(po => po.PurchaseRequest)
            .HasForeignKey<PurchaseOrder>(po => po.PurchaseRequestId);

        // PO → ReceiveGoods (1-1)
        builder.Entity<PurchaseOrder>()
            .HasOne(po => po.ReceiveGoods)
            .WithOne(rg => rg.PurchaseOrder)
            .HasForeignKey<ReceiveGoods>(rg => rg.PurchaseOrderId);

        // ReceiveGoods → Invoice (1-1)
        builder.Entity<ReceiveGoods>()
            .HasOne(rg => rg.Invoice)
            .WithOne(inv => inv.ReceiveGoods)
            .HasForeignKey<Invoice>(inv => inv.ReceiveGoodsId);
    }
}