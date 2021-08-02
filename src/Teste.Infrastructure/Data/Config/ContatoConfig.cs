using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Teste.Domain.Entities;

namespace Teste.Infrastructure.Data.Config
{
    public class ContatoConfig : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Nome).IsRequired().HasMaxLength(255);
            builder.Property(p => p.DataNascimento).HasColumnType("datetime");
            builder.Property(p => p.Sexo);
            builder.Property(p => p.Idade);
            builder.Property(p => p.Ativo);
            builder.Property(e => e.DataCriacao).HasColumnType("datetime");
            builder.Property(e => e.DataAlteracao).HasColumnType("datetime");
        }
    }
}
