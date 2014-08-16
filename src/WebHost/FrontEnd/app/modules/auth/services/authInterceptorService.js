'use strict';
angular.module('authModule').factory('authInterceptorService', [
    '$q', '$injector', '$location', '$localStorage',
    function ($q, $injector, $location, $localStorage) {
        var authInterceptorServiceFactory = { request: null, responseError: null };

        var request = function (config) {
            config.headers = config.headers || {};

            var authData = $localStorage.authorizationData;
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }

            return config;
        };

        var responseError = function (rejection) {
            if (rejection.status === 401) {
                var authService = $injector.get('authService');
                var authData = $localStorage.authorizationData;

                authService.logOut();
                $location.path('/login');
            }
            return $q.reject(rejection);
        };

        authInterceptorServiceFactory.request = request;
        authInterceptorServiceFactory.responseError = responseError;

        return authInterceptorServiceFactory;
    }]);
//# sourceMappingURL=authInterceptorService.js.map
