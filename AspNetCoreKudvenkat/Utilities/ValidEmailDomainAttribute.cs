using System.ComponentModel.DataAnnotations;

namespace AspNetCoreKudvenkat.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string _allowedDomain;
        public ValidEmailDomainAttribute(string allowedDomain)
        {
            _allowedDomain = allowedDomain;

        }
        public override bool IsValid(object value)
        {
            var strings = value.ToString().Split('@');
            return _allowedDomain.ToLower() == strings[^1].ToLower(); 
        }
    }
}