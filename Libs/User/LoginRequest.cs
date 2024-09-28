using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace Webapi.Libs.User {
    public record LoginRequest(
        [property: DefaultValue("carlfearby@me.com"), SwaggerSchema] string Email,
        [property: DefaultValue("password1234!"), SwaggerSchema] string Password
    );
}