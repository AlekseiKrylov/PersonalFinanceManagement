﻿using PersonalFinanceManagement.Domain.DTOModels;
using PersonalFinanceManagement.WebAPIClients.Clients.Base;

namespace PersonalFinanceManagement.WebAPIClients.Clients
{
    public class WalletsWebApiClient : EntitiesWebApiClient<WalletDTO, WalletCreateDTO>
    {
        public WalletsWebApiClient(HttpClient httpClient) : base(httpClient) { }
    }
}
