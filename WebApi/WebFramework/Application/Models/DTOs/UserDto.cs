using System;
using System.ComponentModel.DataAnnotations;
using Entities.User;
using WebFramework.Api;

namespace WebFramework.Application.Models.DTOs
{
    public class UserDto : CustomDto<UserDto, User>/*, IValidatableObject*/
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(500)]
        public string Password { get; set; }

        /*[Required]
        [StringLength(100)]
        public string FullName { get; set; }*/

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string NationalCode { get; set; }

        public string BirthCertificateNumber { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
        //        yield return new ValidationResult("نام کاربری نمیتواند Test باشد", new[] { nameof(UserName) });
        //    if (Password.Equals("123456"))
        //        yield return new ValidationResult("رمز عبور نمیتواند 123456 باشد", new[] { nameof(Password) });
        //    //if (Gender == GenderType.Male && (DateTime.Today.Year - BirthDate.Year) > 30)
        //    //    yield return new ValidationResult("آقایان بیشتر از 30 سال معتبر نیستند", new[] { nameof(Gender), nameof(BirthDate) });
        //}
        
    }

    public class UserSelectDto : CustomDto<UserSelectDto, User>
    {
        public string UserName { get; set; }

        /*public string FullName { get; set; }*/

        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string NationalCode { get; set; }

        public string BirthCertificateNumber { get; set; }

        /*public int Age { get; set; }*/

        // For describing enums as string.. but no needed!
        //[EnumDataType(typeof(GenderType))]
        /*public GenderType Gender { get; set; }*/

        /*public override void CustomMappings(IMappingExpression<User, UserSelectDto> mapping)
        {
            mapping.ForMember(
                dest => dest.Age,
                config => config.MapFrom(src => (DateTime.Now.Year - src.BirthDate.Year)));
        }*/
    }
}
