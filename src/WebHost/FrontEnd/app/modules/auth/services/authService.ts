'use strict';


interface IAuthData {
    isAuth: boolean;
    userName: string;
    useRefreshTokens: boolean;
}

interface IExternalAuthData {
    provider: string;
    userName: string;
    externalAccessToken: string;
}

interface IStoredAuthData {
    token: string;
    userName: string;
    refreshToken: string;
    useRefreshTokens: boolean;
}

interface IAuthService {
    saveRegistration: any;
    login: any;
    logOut: any;
    fillAuthData: any;
    authentication: any;
    refreshToken: any;
    obtainAccessToken: any;
    externalAuthData: any;
    registerExternal: any;
}

angular.module('authModule')
    .factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings',
        ($http, $q, localStorageService, ngAuthSettings) => {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;


            var authentication: IAuthData = {
                isAuth: false,
                userName: "",
                useRefreshTokens: false
            };

            var externalAuthData: IExternalAuthData = {
                provider: "",
                userName: "",
                externalAccessToken: ""
            };

            // TODO server type UserRegisterModel
            var saveRegistration = registration => {

                logOut();

                return $http
                    .post(serviceBase + 'auth/account/register', registration)
                    .then(response => response);
            };

            // TODO server type 
            var login = loginData => {

                var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

                if (loginData.useRefreshTokens) {
                    data = data + "&client_id=" + ngAuthSettings.clientId;
                }

                var deferred = $q.defer();

                $http
                    .post(serviceBase + 'token', data,
                    { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .success(response => {

                        if (loginData.useRefreshTokens) {
                            localStorageService.set('authorizationData',
                                <IStoredAuthData> {
                                    token: response.access_token,
                                    userName: loginData.userName,
                                    refreshToken: response.refresh_token,
                                    useRefreshTokens: true
                                });
                        } else {
                            localStorageService.set('authorizationData',
                                <IStoredAuthData> {
                                    token: response.access_token,
                                    userName: loginData.userName,
                                    refreshToken: "",
                                    useRefreshTokens: false
                                });
                        }
                        authentication.isAuth = true;
                        authentication.userName = loginData.userName;
                        authentication.useRefreshTokens = loginData.useRefreshTokens;

                        deferred.resolve(response);

                    }).error((err, status) => {
                        logOut();
                        deferred.reject(err);
                    });

                return deferred.promise;

            };

            var logOut = () => {

                localStorageService.remove('authorizationData');

                authentication.isAuth = false;
                authentication.userName = "";
                authentication.useRefreshTokens = false;

            };

            var fillAuthData = () => {

                var authData : IStoredAuthData = localStorageService.get('authorizationData');
                if (authData) {
                    authentication.isAuth = true;
                    authentication.userName = authData.userName;
                    authentication.useRefreshTokens = authData.useRefreshTokens;
                }

            };

            var refreshToken = () => {
                var deferred = $q.defer();

                var authData = localStorageService.get('authorizationData');

                if (authData) {

                    if (authData.useRefreshTokens) {

                        var data = "grant_type=refresh_token&refresh_token="
                            + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                        localStorageService.remove('authorizationData');

                        $http
                            .post(serviceBase + 'token', data,
                            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                            .success(response => {

                                localStorageService.set('authorizationData',
                                    <IStoredAuthData>{
                                        token: response.access_token,
                                        userName: response.userName,
                                        refreshToken: response.refresh_token,
                                        useRefreshTokens: true
                                    });

                                deferred.resolve(response);

                            }).error((err, status) => {
                                logOut();
                                deferred.reject(err);
                            });
                    }
                }

                return deferred.promise;
            };

            var obtainAccessToken = externalData => {

                var deferred = $q.defer();

                $http.get(serviceBase + 'api/account/ObtainLocalAccessToken',
                    {
                        params: {
                            provider: externalData.provider,
                            externalAccessToken: externalData.externalAccessToken
                        }
                    })
                    .success(response => {
                        localStorageService.set('authorizationData',
                            <IStoredAuthData> {
                                token: response.access_token,
                                userName: response.userName,
                                refreshToken: "",
                                useRefreshTokens: false
                            });

                        authentication.isAuth = true;
                        authentication.userName = response.userName;
                        authentication.useRefreshTokens = false;

                        deferred.resolve(response);

                    }).error((err, status) => {
                        logOut();
                        deferred.reject(err);
                    });

                return deferred.promise;

            };

            var registerExternal = registerExternalData => {

                var deferred = $q.defer();

                $http.post(serviceBase + 'account/registerexternal', registerExternalData)
                    .success(response => {

                        localStorageService.set('authorizationData',
                            <IStoredAuthData> {
                                token: response.access_token,
                                userName: response.userName,
                                refreshToken: "",
                                useRefreshTokens: false
                            });

                        authentication.isAuth = true;
                        authentication.userName = response.userName;
                        authentication.useRefreshTokens = false;

                        deferred.resolve(response);

                    }).error((err, status) => {
                        logOut();
                        deferred.reject(err);
                    });

                return deferred.promise;

            };

            var authServiceFactory: IAuthService = {
                saveRegistration: saveRegistration,
                login: login,
                logOut: logOut,
                fillAuthData: fillAuthData,
                authentication: authentication,
                refreshToken: refreshToken,
                obtainAccessToken: obtainAccessToken,
                externalAuthData: externalAuthData,
                registerExternal: registerExternal,
            }

            return authServiceFactory;
        }]);