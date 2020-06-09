using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
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

        public GenderType Gender { get; set; }

        public string NationalCode { get; set; }

        public string BirthCertificateNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset LastLoginDate { get; set; }

        //private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        //public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public RefreshToken RefreshToken { get; set; }

        //public ICollection<Address> Addresses { get; set; }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return RefreshToken != null && (RefreshToken.Active && RefreshToken.Token.Equals(refreshToken));
        }

        public void AddRefreshToken(string token, int userId, double daysToExpire = 30)
        {
            var rt = new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), userId);
            if (RefreshToken == null)
            {
                RefreshToken = rt;
            }
            else
            {
                RefreshToken.Token = rt.Token;
                RefreshToken.Expires = rt.Expires;
            }
        }

        //public bool HasValidRefreshToken(string refreshToken)
        //{
        //    return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        //}

        //public void AddRefreshToken(string token, int userId, double daysToExpire = 30)
        //{
        //    _refreshTokens.Clear();
        //    _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), userId));
        //}

        //public void RemoveRefreshToken(string refreshToken)
        //{
        //    _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        //}

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
    public enum GenderType
    {
        [EnumMember(Value = "Female")]
        //[Display(Name = "Female")]
        Female = 0,

        [EnumMember(Value = "Male")]
        //[Display(Name = "Male")]
        Male = 1
    }
}
