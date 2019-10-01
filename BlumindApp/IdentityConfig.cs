using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlumindApp {
    public class IdentityConfig {
        public static string ApiResourceName = "app-api";
        public static string ApiSecret = "{157=mjy-66hji78h}_bku";
        public static string ClientSecret = "app-client";
        private static int _tokenLifetime = 3600 * 8;

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>() {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource{Name = "roles", UserClaims = {JwtClaimTypes.Role}}
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>() {
                new ApiResource(ApiResourceName, "Blumind API", new []{ JwtClaimTypes.Role }) {
                    ApiSecrets = { new Secret(ApiSecret.Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>(){
                new Client() {
                    ClientId = "app-client",
                    ClientName = "app-client",

                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(ClientSecret.Sha256()) },
                    AccessTokenLifetime = _tokenLifetime,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        ApiResourceName,
                        "roles"
                    },
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    UserSsoLifetime = _tokenLifetime,
                    IdentityTokenLifetime = _tokenLifetime
                }
            };
        }
    }

}
