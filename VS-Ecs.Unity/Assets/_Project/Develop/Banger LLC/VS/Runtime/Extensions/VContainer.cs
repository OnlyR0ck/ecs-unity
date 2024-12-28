using VContainer;

namespace VS.Runtime.Extensions
{
    public static class VContainer
    {
        public static T Instantiate<T>(this IObjectResolver resolver, Lifetime lifetime, params object[] args)
        {
            var registrationBuilder = new RegistrationBuilder(typeof(T), lifetime);

            if (args is { Length: > 0 })
            {
                foreach (var arg in args)
                {
                    registrationBuilder.WithParameter(arg.GetType(), arg);
                }
            }

            Registration registration = registrationBuilder.Build();
            return (T)resolver.Resolve(registration);
        }
        
        public static T Instantiate<T>(this IObjectResolver resolver, Lifetime lifetime)
        {
            var registrationBuilder = new RegistrationBuilder(typeof(T), lifetime);
            Registration registration = registrationBuilder.Build();
            return (T)resolver.Resolve(registration);
        }
    }
}