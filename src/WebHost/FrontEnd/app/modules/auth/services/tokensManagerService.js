'use strict';
angular.module('authModule').factory('tokensManagerService', [
    '$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var getRefreshTokens = function () {
            return $http.get(serviceBase + 'auth/refreshtokens').then(function (results) {
                return results;
            });
        };

        var deleteRefreshTokens = function (tokenid) {
            return $http.delete(serviceBase + 'auth/refreshtokens/?tokenid=' + tokenid).then(function (results) {
                return results;
            });
        };

        var tokenManagerServiceFactory = {
            deleteRefreshTokens: deleteRefreshTokens,
            getRefreshTokens: getRefreshTokens
        };

        return tokenManagerServiceFactory;
    }]);
//# sourceMappingURL=tokensManagerService.js.map
