using NetCoreDomain;

namespace NetCoreIdentityDomain
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        private string _value;

        public PhoneNumber()
        {

        }

        public PhoneNumber(string value)
        {
            _value = value;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            _value = phoneNumber;
        }

        protected override bool EqualsCore(PhoneNumber other)
        {
            return _value == other._value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }

        public static implicit operator string(PhoneNumber phoneNumber)
        {
            return phoneNumber._value;
        }

        public static implicit operator PhoneNumber(string phoneNumber)
        {
            return new PhoneNumber(phoneNumber);
        }
    }
}
