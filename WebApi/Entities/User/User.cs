using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Common.Utilities;
using Entities.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Entities.User
{
    public class User : IdentityUser<int>, IEntity
    {
        public User()
        {
            IsActive = true;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        [Column("Gender")]
        public string GenderString
        {
            get => Gender.ToString();
            private set => Gender = value.ParseEnum<Gender>();
        }

        [NotMapped]
        public Gender Gender { get; set; }

        public string NationalCode { get; set; }

        public string BirthCertificateNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset LastLoginDate { get; set; }

        public RefreshToken RefreshToken { get; set; }

        //public ICollection<Address> Addresses { get; set; }

    }

    //public class UserConfiguration : IEntityTypeConfiguration<User>
    //{
    //    public void Configure(EntityTypeBuilder<User> builder)
    //    {
    //        builder.HasOne(a => a.RefreshToken)
    //            .WithOne(b => b.User)
    //            .HasForeignKey<RefreshToken>(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
    //        //builder.Property(p => p.UserName).IsRequired().HasMaxLength(50);
    //        /*builder.Property(p => p.FullName).HasMaxLength(100);*/
    //    }
    //}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        [EnumMember(Value = "Female")]
        //[Display(Name = "Female")]
        Female = 0,

        [EnumMember(Value = "Male")]
        //[Display(Name = "Male")]
        Male = 1
    }
}
