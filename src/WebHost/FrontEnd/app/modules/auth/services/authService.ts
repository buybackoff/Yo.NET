'use strict';


interface IAuthData {
    isAuth: boolean;
    userName: string;
}

interface IStoredAuthData {
    token: string;
    userName: string;
}

interface IAuthService {
    saveRegistration: any;
    login: any;
    logOut: any;
    fillAuthData: any;
    authentication: any;
    refreshToken: any;
    obtainAccessToken: any;
    registerExternal: any;
}

angular.module('authModule')
    .factory('authService', ['$http', '$q', '$localStorage', 'ngAuthSettings',
        ($http, $q, $localStorage, ngAuthSettings) => {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;


            var authentication: IAuthData = {
                isAuth: false,
                userName: ""
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
                            $localStorage.authorizationData = 
                                <IStoredAuthData> {
                                    token: response.access_token,
                                    userName: loginData.userName,
                                    refreshToken: response.refresh_token,
                                    useRefreshTokens: true
                                };
                        } else {
                            $localStorage.authorizationData =
                                <IStoredAuthData> {
                                    token: response.access_token,
                                    userName: loginData.userName,
                                    refreshToken: "",
                                    useRefreshTokens: false
                                };
                        }
                        authentication.isAuth = true;
                        authentication.userName = loginData.userName;

                        deferred.resolve(response);

                    }).error((err, status) => {
                        logOut();
                        deferred.reject(err);
                    });

                return deferred.promise;

            };

            var logOut = () => {

                delete $localStorage.authorizationData;

                authentication.isAuth = false;
                authentication.userName = "";

            };

            var fillAuthData = () => {

                var authData: IStoredAuthData = $localStorage.authorizationData;
                if (authData) {
                    authentication.isAuth = true;
                    authentication.userName = authData.userName;
                }

            };

            var refreshToken = () => {
                var deferred = $q.defer();

                var authData = $localStorage.authorizationData;

                if (authData) {

                    if (authData.useRefreshTokens) {

                        var data = "grant_type=refresh_token&refresh_token="
                            + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                        delete $localStorage.authorizationData;

                        $http
                            .post(serviceBase + 'token', data,
                            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                            .success(response => {

                                $localStorage.authorizationData =
                                    <IStoredAuthData>{
                                        token: response.access_token,
                                        userName: response.userName,
                                        refreshToken: response.refresh_token,
                                        useRefreshTokens: true
                                    };

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
                        $localStorage.authorizationData =
                            <IStoredAuthData> {
                                token: response.access_token,
                                userName: response.userName,
                                refreshToken: "",
                                useRefreshTokens: false
                            };

                        authentication.isAuth = true;
                        authentication.userName = response.userName;

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

                        $localStorage.authorizationData = 
                            <IStoredAuthData> {
                                token: response.access_token,
                                userName: response.userName,
                                refreshToken: "",
                                useRefreshTokens: false
                            };

                        authentication.isAuth = true;
                        authentication.userName = response.userName;

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
                registerExternal: registerExternal,
            }

            return authServiceFactory;
        }]);