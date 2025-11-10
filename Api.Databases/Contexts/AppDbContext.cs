using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Api.Databases.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<IdentityUser, IdentityRole, string>(options)
{
    public virtual DbSet<Client> Clients => Set<Client>();
    public virtual DbSet<Commande> Commandes => Set<Commande>();
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Categorie> Categories => Set<Categorie>();
    public virtual DbSet<LigneCommande> LigneCommandes => Set<LigneCommande>();
    public virtual DbSet<Produit> Produits => Set<Produit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}