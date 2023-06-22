

namespace Microservices.Accounts.Contract.Client
{
    public class AccountsGrpcServiceClient : IAccountsGrpcService, IGrpcClient
    {
        private readonly IAccountsGrpcService _accountsService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountsGrpcServiceClient> _logger;


        public AccountsGrpcServiceClient(
            IConfiguration configuration,
            ILogger<AccountsGrpcServiceClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _accountsService = GetGrpcChannel().CreateGrpcService<IAccountsGrpcService>();
        }

        public async Task<ResponseStatus> UpsertPartnerUser(string request, CallContext context = default)
        {
            _logger.LogDebug("AccountsGrpcServiceClient : UpsertPartnerUser - Entering UpsertPartnerUser! for {@request}!", request);
            ResponseStatus response = new ResponseStatus();
            try
            {
                _logger.LogDebug("AccountsGrpcServiceClient : UpsertPartnerUser - Exiting UpsertPartnerUser! for {@request}!", request);
                return await _accountsService.UpsertPartnerUser(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AccountsGrpcServiceClient : UpsertPartnerUser - Failed to load data with {@message}", ex.Message);
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;

        }
        #region Helpers
        private GrpcChannel GetGrpcChannel()
        {
            var httpHandler = new HttpClientHandler();

            // Return `true` to allow certificates that are untrusted/invalid
            if (_configuration["ApplicationDetails:AppMode"]?.ToLower() == "server")
            {
                httpHandler.ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }


            string baseUri = _configuration.GetSection("EndPoints").GetValue<string>(nameof(AccountsGrpcServiceClient));
            var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpHandler));

            var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

            return channel;
        }
        #endregion
    }
}
