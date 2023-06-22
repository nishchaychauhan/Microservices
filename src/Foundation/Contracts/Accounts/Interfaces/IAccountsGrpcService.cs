namespace Microservices.Accounts.Contract.Interfaces
{
    [ServiceContract]
    public interface IAccountsGrpcService
    {
        [OperationContract]
        public Task<ResponseStatus> UpsertPartnerUser(string request, CallContext context = default);
        
    }
}


