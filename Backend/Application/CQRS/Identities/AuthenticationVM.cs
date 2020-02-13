
namespace Application.CQRS.Identities
{
    public class AuthenticationVM : GenericVm<AuthenticationResult>
    {
        public override AuthenticationResult Data { get; set; }
    }
}
