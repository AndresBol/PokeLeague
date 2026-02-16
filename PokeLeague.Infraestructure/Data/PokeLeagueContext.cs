using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PokeLeague.Infraestructure.Models;

namespace PokeLeague.Infraestructure.Data;

public partial class PokeLeagueContext : DbContext
{
    public PokeLeagueContext(DbContextOptions<PokeLeagueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auction> Auction { get; set; }

    public virtual DbSet<AuctionBid> AuctionBid { get; set; }

    public virtual DbSet<Card> Card { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<CategoryCard> CategoryCard { get; set; }

    public virtual DbSet<Image> Image { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }

    public virtual DbSet<Rarity> Rarity { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Set> Set { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auction__3213E83F8085A0C8");

            entity.ToTable("auction");

            entity.HasIndex(e => e.CardId, "IX_auction_card_id");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "IX_auction_dates");

            entity.HasIndex(e => e.UserId, "IX_auction_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.EndDate)
                .HasPrecision(0)
                .HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.MinIncrease)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("min_increase");
            entity.Property(e => e.StartDate)
                .HasPrecision(0)
                .HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Card).WithMany(p => p.Auction)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_auction_card");

            entity.HasOne(d => d.User).WithMany(p => p.Auction)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_auction_user");
        });

        modelBuilder.Entity<AuctionBid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__auction___3213E83FB182273A");

            entity.ToTable("auction_bid");

            entity.HasIndex(e => e.AuctionId, "IX_auction_bid_auction");

            entity.HasIndex(e => e.UserId, "IX_auction_bid_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuctionId).HasColumnName("auction_id");
            entity.Property(e => e.BidAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("bid_amount");
            entity.Property(e => e.BidDate)
                .HasPrecision(0)
                .HasColumnName("bid_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Auction).WithMany(p => p.AuctionBid)
                .HasForeignKey(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_auction_bid_auction");

            entity.HasOne(d => d.User).WithMany(p => p.AuctionBid)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_auction_bid_user");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__card__3213E83F70B06018");

            entity.ToTable("card");

            entity.HasIndex(e => e.LanguageCode, "IX_card_language_code");

            entity.HasIndex(e => e.RarityId, "IX_card_rarity_id");

            entity.HasIndex(e => e.SetId, "IX_card_set_id");

            entity.HasIndex(e => e.UserId, "IX_card_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Grade)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("grade");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsNew).HasColumnName("is_new");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("language_code");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.RarityId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("rarity_id");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");
            entity.Property(e => e.SetId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("set_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.LanguageCodeNavigation).WithMany(p => p.Card)
                .HasForeignKey(d => d.LanguageCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_card_language");

            entity.HasOne(d => d.Rarity).WithMany(p => p.Card)
                .HasForeignKey(d => d.RarityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_card_rarity");

            entity.HasOne(d => d.Set).WithMany(p => p.Card)
                .HasForeignKey(d => d.SetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_card_set");

            entity.HasOne(d => d.User).WithMany(p => p.Card)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_card_user");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__category__3213E83F2A56121C");

            entity.ToTable("category");

            entity.HasIndex(e => e.Name, "UQ_category_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CategoryCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__category__3213E83F0E00C653");

            entity.ToTable("category_card");

            entity.HasIndex(e => e.CardId, "IX_category_card_card_id");

            entity.HasIndex(e => e.CategoryId, "IX_category_card_category_id");

            entity.HasIndex(e => new { e.CardId, e.CategoryId }, "UQ_category_card").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            entity.HasOne(d => d.Card).WithMany(p => p.CategoryCard)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_category_card_card");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryCard)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_category_card_category");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__image__3213E83F8EECDE0C");

            entity.ToTable("image");

            entity.HasIndex(e => e.CardId, "IX_image_card_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.ImageData).HasColumnName("image_data");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            entity.HasOne(d => d.Card).WithMany(p => p.Image)
                .HasForeignKey(d => d.CardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_image_card");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageCode);

            entity.ToTable("language");

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("language_code");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(50)
                .HasColumnName("language_name");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83F8818E2AD");

            entity.ToTable("purchase_order");

            entity.HasIndex(e => e.UserId, "IX_purchase_order_user");

            entity.HasIndex(e => e.AuctionId, "UQ_purchase_order_auction").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuctionId).HasColumnName("auction_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.PurchaseAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("purchase_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Auction).WithOne(p => p.PurchaseOrder)
                .HasForeignKey<PurchaseOrder>(d => d.AuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_purchase_order_auction");

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseOrder)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_purchase_order_user");
        });

        modelBuilder.Entity<Rarity>(entity =>
        {
            entity.ToTable("rarity");

            entity.Property(e => e.Id)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__role__3213E83F56FDFDD2");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "UQ_role_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.ToTable("set");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F28804C88");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "IX_user_role_id");

            entity.HasIndex(e => e.Email, "UQ_user_email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsBlocked).HasColumnName("is_blocked");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("role_id");
            entity.Property(e => e.SignupDate).HasColumnName("signup_date");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.User)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
