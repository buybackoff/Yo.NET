'use strict';
angular.module('authModule').factory('authService', [
    '$http', '$q', '$localStorage', 'ngAuthSettings',
    function ($http, $q, $localStorage, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var authentication = {
            isAuth: false,
            userName: ""
        };

        // TODO server type UserRegisterModel
        var saveRegistration = function (registration) {
            logOut();

            return $http.post(serviceBase + 'auth/account/register', registration).then(function (response) {
                return response;
            });
        };

        // TODO server type
        var login = function (loginData) {
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            if (loginData.useRefreshTokens) {
                data = data + "&client_id=" + ngAuthSettings.clientId;
            }

            var deferred = $q.defer();

            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                if (loginData.useRefreshTokens) {
                    $localStorage.authorizationData = {
                        token: response.access_token,
                        userName: loginData.userName,
                        refreshToken: response.refresh_token,
                        useRefreshTokens: true
                    };
                } else {
                    $localStorage.authorizationData = {
                        token: response.access_token,
                        userName: loginData.userName,
                        refreshToken: "",
                        useRefreshTokens: false
                    };
                }
                authentication.isAuth = true;
                authentication.userName = loginData.userName;

                deferred.resolve(response);
            }).error(function (err, status) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        var logOut = function () {
            delete $localStorage.authorizationData;

            authentication.isAuth = false;
            authentication.userName = "";
        };

        var fillAuthData = function () {
            var authData = $localStorage.authorizationData;
            if (authData) {
                authentication.isAuth = true;
                authentication.userName = authData.userName;
            }
        };

        var refreshToken = function () {
            var deferred = $q.defer();

            var authData = $localStorage.authorizationData;

            if (authData) {
                if (authData.useRefreshTokens) {
                    var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                    delete $localStorage.authorizationData;

                    $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                        $localStorage.authorizationData = {
                            token: response.access_token,
                            userName: response.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true
                        };

                        deferred.resolve(response);
                    }).error(function (err, status) {
                        logOut();
                        deferred.reject(err);
                    });
                }
            }

            return deferred.promise;
        };

        var obtainAccessToken = function (externalData) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', {
                params: {
                    provider: externalData.provider,
                    externalAccessToken: externalData.externalAccessToken
                }
            }).success(function (response) {
                $localStorage.authorizationData = {
                    token: response.access_token,
                    userName: response.userName,
                    refreshToken: "",
                    useRefreshTokens: false
                };

                authentication.isAuth = true;
                authentication.userName = response.userName;

                deferred.resolve(response);
            }).error(function (err, status) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        var registerExternal = function (registerExternalData) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'account/registerexternal', registerExternalData).success(function (response) {
                $localStorage.authorizationData = {
                    token: response.access_token,
                    userName: response.userName,
                    refreshToken: "",
                    useRefreshTokens: false
                };

                authentication.isAuth = true;
                authentication.userName = response.userName;

                deferred.resolve(response);
            }).error(function (err, status) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        var authServiceFactory = {
            saveRegistration: saveRegistration,
            login: login,
            logOut: logOut,
            fillAuthData: fillAuthData,
            authentication: authentication,
            refreshToken: refreshToken,
            obtainAccessToken: obtainAccessToken,
            registerExternal: registerExternal
        };

        return authServiceFactory;
    }]);
//# sourceMappingURL=authService.js.map
