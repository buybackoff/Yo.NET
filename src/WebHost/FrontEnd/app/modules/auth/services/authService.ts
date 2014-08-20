'use strict';

interface IAuthNames {
    email: string;
    fullName: string;
    userName: string;
}

interface IAuthService {
    getAuthNames: () => IAuthNames;
    refillAuthNames: () => IAuthNames;
    isAuth: () => boolean;
    login: (string) => void;
    logout: (string) => void;
}

angular.module('authModule')
    .factory('authService', ['$http', '$q', '$window', '$location', '$localStorage',
        ($http: ng.IHttpService, $q, $window: ng.IWindowService, $location: ng.ILocationService, $localStorage) => {

            //if ($localStorage.authorizationData) {
            //    console.log('have data: ' + $localStorage.authorizationData);
            //}

            // start from local cache
            var authorizationData: IAuthNames = $localStorage.authorizationData;

            // redirect to login page + return url
            var login = (redirect: string) => {
                // will leave angular so when back refillNames
                // will be fired in module.run
                if (redirect) {
                    $window.location.href = "/account/login?returnUrl=" + redirect;
                } else {
                    $window.location.href = "/account/login";
                }
            };

            var logout = (redirect: string) => {
                delete $localStorage.authorizationData;
                authorizationData = null;
                $http
                    .post('/account/logoff', {})
                    .success(() => {
                        if (redirect) {
                            $window.location.href = redirect;
                        } else {
                            $window.location.href = '/';
                        }
                    }).error((err, status) => {
                        console.log('Cannot logout properly, status: ' + status + ', error: ' + err);
                    });
            };

            var getAuthorizationData = () => {
                if (isAuth()) {
                    return authorizationData;
                } else if ($localStorage.authorizationData) {
                    authorizationData = $localStorage.authorizationData;
                    return authorizationData;
                }
                return null;
            }

            var refillAuthData = () => {
                $http
                    .get('/api/account/names', {cache: false})
                    .success((names) => {
                        if (names && names != "null") {
                            authorizationData = <IAuthNames>names;
                            console.log('received non null names: ' + names);
                        } else {
                            authorizationData = null;
                            console.log('as guest');
                        }
                        $localStorage.authorizationData = authorizationData;
                        return authorizationData;
                    }).error((err, status) => {
                        console.log('Cannot get auth names, status: ' + status + ', error: ' + err);
                    });
                return null;
            };

            var isAuth = () => {
                if (authorizationData && authorizationData.email) {
                    //console.log('isAuth = true');
                    return true;
                } else if ($localStorage.authorizationData) {
                    authorizationData = $localStorage.authorizationData;
                    if (authorizationData && authorizationData.email) {
                        return true;
                    }
                }
                //console.log('isAuth = false');
                return false;
            }

            var authServiceFactory: IAuthService = {
                login: login,
                logout: logout,
                getAuthNames: getAuthorizationData,
                refillAuthNames: refillAuthData,
                isAuth: isAuth
            }

            return authServiceFactory;
        }]);