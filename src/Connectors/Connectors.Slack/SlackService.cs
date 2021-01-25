﻿using System;
using System.Collections.Generic;
using RestSharp;
using Tayra.Connectors.Common;
using Tayra.Connectors.Slack.DTOs;

namespace Tayra.Connectors.Slack
{
    public static class SlackService
    {
        #region Constants

        public const string CLIENT_ID = "698826045604.1130569857540";
        public const string CLIENT_SECRET = "14642ab9d50d2c898ef0720b4072c7ab";

        private const string BASE_REST_URL = "https://slack.com/api";
        private const string GET_ACCESS_TOKEN = "/oauth.v2.access";
        private const string GET_USERS_LIST = "/users.list";
        private const string POST_MESSAGE = "/chat.postMessage";

        public static IRestResponse<TokenResponse> ExchangeCodeForAccessToken(string code, string redirectUrl)
        {
            var request = new RestRequest(GET_ACCESS_TOKEN, Method.POST);
            request.AddParameter("client_id", CLIENT_ID);
            request.AddParameter("client_secret", CLIENT_SECRET);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUrl);

            request.RequestFormat = DataFormat.Json;

            var client = new RestClient(BASE_REST_URL);
            return client.Execute<TokenResponse>(request);
        }

        //api.slack.com/methods/users.list
        //limited to 1000 users on slack, needs pagination for more
        public static IRestResponse<UsersListResponse> GetUsersList(string botToken)
        {
            var request = new RestRequest(GET_USERS_LIST, Method.GET);
            request.AddHeader("Authorization", $"Bearer {botToken}");
            request.AddHeader("Accept", "application/vnd.github.v3+json");
            request.AddHeader("Accept", "application/vnd.github.v3+json");

            var client = new RestClient(BASE_REST_URL)
                .UseSerializer(() => new JsonNetSerializer());

            return client.Execute<UsersListResponse>(request);
        }

        public static IRestResponse<SlackMessageResponseDto> SendSlackMessage(string botToken,SlackMessageRequestDto requestDto)
        {
            var client = new RestClient(BASE_REST_URL).UseSerializer((() => new JsonNetSerializer()));
            var request = new RestRequest(POST_MESSAGE, Method.POST);
            request.AddHeader("Authorization", $"Bearer {botToken}");
            request.AddHeader("Content-type", "application/json;charset=UTF-8");
            request.AddJsonBody(requestDto);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<SlackMessageResponseDto>(request);
            if (response.Data.Ok == false)
            {
                throw new ApplicationException(response.Data.Error);
            }

            return response;
        }
        

        #endregion
    }
}