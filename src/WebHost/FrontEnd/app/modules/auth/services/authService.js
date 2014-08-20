'use strict';
angular.module('authModule').factory('authService', [
    '$http', '$q', '$window', '$location', '$localStorage',
    function ($http, $q, $window, $location, $localStorage) {
        //if ($localStorage.authorizationData) {
        //    console.log('have data: ' + $localStorage.authorizationData);
        //}
        // start from local cache
        var authorizationData = $localStorage.authorizationData;

        // redirect to login page + return url
        var login = function (redirect) {
            // will leave angular so when back refillNames
            // will be fired in module.run
            if (redirect) {
                $window.location.href = "/account/login?returnUrl=" + redirect;
            } else {
                $window.location.href = "/account/login";
            }
        };

        var logout = function (redirect) {
            delete $localStorage.authorizationData;
            authorizationData = null;
            $http.post('/account/logoff', {}).success(function () {
                if (redirect) {
                    $window.location.href = redirect;
                } else {
                    $window.location.href = '/';
                }
            }).error(function (err, status) {
                console.log('Cannot logout properly, status: ' + status + ', error: ' + err);
            });
        };

        var getAuthorizationData = function () {
            if (isAuth()) {
                return authorizationData;
            } else if ($localStorage.authorizationData) {
                authorizationData = $localStorage.authorizationData;
                return authorizationData;
            }
            return null;
        };

        var refillAuthData = function () {
            $http.get('/api/account/names', { cache: false }).success(function (names) {
                if (names && names != "null") {
                    authorizationData = names;
                    console.log('received non null names: ' + names);
                } else {
                    authorizationData = null;
                    console.log('as guest');
                }
                $localStorage.authorizationData = authorizationData;
                return authorizationData;
            }).error(function (err, status) {
                console.log('Cannot get auth names, status: ' + status + ', error: ' + err);
            });
            return null;
        };

        var isAuth = function () {
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
        };

        var authServiceFactory = {
            login: login,
            logout: logout,
            getAuthNames: getAuthorizationData,
            refillAuthNames: refillAuthData,
            isAuth: isAuth
        };

        return authServiceFactory;
    }]);
//# sourceMappingURL=authService.js.map
