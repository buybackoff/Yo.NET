'use strict';

interface ITokenManagerServiceFactory {
    deleteRefreshTokens: any;
    getRefreshTokens: any;
}

angular.module('authModule')
    .factory('tokensManagerService',
    ['$http', 'ngAuthSettings', ($http, ngAuthSettings) => {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var getRefreshTokens = () =>
            $http
                .get(serviceBase + 'auth/refreshtokens')
                .then(results => results);

        var deleteRefreshTokens = tokenid =>
            $http
                .delete(serviceBase + 'auth/refreshtokens/?tokenid=' + tokenid)
                .then(results => results);

        var tokenManagerServiceFactory: ITokenManagerServiceFactory =
            {
                deleteRefreshTokens: deleteRefreshTokens,
                getRefreshTokens: getRefreshTokens
            }

        return tokenManagerServiceFactory;

    }]);