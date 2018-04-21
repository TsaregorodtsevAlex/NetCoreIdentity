using NetCoreDomain;

namespace NetCoreIdentityDomain
{
    public class Email : ValueObject<Email>
    {
        private string _value;

        public Email()
        {

        }

        public Email(string value)
        {
            _value = value;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            _value = phoneNumber;
        }

        protected override bool EqualsCore(Email other)
        {
            return _value == other._value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }

        public static implicit operator string(Email phoneNumber)
        {
            return phoneNumber._value;
        }

        public static implicit operator Email(string phoneNumber)
        {
            return new Email(phoneNumber);
        }
    }
}
