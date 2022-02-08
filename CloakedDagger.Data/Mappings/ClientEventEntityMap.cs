using System.Collections.Generic;
using CloakedDagger.Common.Converters;
using CloakedDagger.Common.Domain.Events.Client;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace CloakedDagger.Data.Mappings
{
    public class ClientEventEntityMap : IEntityTypeConfiguration<ClientEventEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEventEntity> builder)
        {
            builder.ToTable("client_event")
                .HasKey(ce => ce.Id);

            builder.Property(ce => ce.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(ce => ce.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            builder.Property(ce => ce.OccurredOn)
                .HasColumnName("occurred_on")
                .IsRequired();

            builder.Property(ce => ce.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(ce => ce.Event)
                .HasColumnName("event")
                .IsRequired()
                .HasConversion(
                    cde => JsonConvert.SerializeObject(cde, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    json => JsonConvert.DeserializeObject<ClientDomainEvent>(json, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Converters = new List<JsonConverter>()
                        {
                            new ClientDomainEventJsonConverter()
                        }
                    }));

        }
    }
}