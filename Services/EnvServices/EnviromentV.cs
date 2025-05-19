using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Sysachad.Services {
    public class EnviromentVariables {
        private readonly IConfiguration _configuration;

        public EnviromentVariables(IConfiguration configuration) {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for the given user ID and admin status
        /// </summary>
        /// <returns>
        /// New JWT token
        /// </returns>
        public SymmetricSecurityKey GetJwtKey() {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key)) {
                throw new Exception("JWT key is not configured.");
            }
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}