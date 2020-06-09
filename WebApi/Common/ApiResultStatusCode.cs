using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Successful")]
        Success = 200,

        [Display(Name = "Server Error")]
        ServerError = 500,

        [Display(Name = "Bad Request")]
        BadRequest = 400,

        [Display(Name = "Not Found")]
        NotFound = 404,

        [Display(Name = "List is Empty")]
        ListEmpty = 4,

        [Display(Name = "Logic Error")]
        LogicError = 5,

        [Display(Name = "Authorization Error")]
        UnAuthorized = 401
    }
}
