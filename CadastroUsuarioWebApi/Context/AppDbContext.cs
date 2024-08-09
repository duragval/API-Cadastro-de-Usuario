﻿using CadastroUsuarioWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuarioWebApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cadastro>? Cadastros { get; set; }
}
