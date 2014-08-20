'use strict';
angular.module('authModule').factory('authInterceptorService', [
    '$q', '$injector',
    function ($q, $injector) {
        var authInterceptorServiceFactory = { request: null, responseError: null };

        var responseError = function (rejection) {
            if (rejection.status === 401) {
                var authService = $injector.get('authService');
                authService.logout("/account/login");
            }
            return $q.reject(rejection);
        };

        authInterceptorServiceFactory.responseError = responseError;

        return authInterceptorServiceFactory;
    }]);
//# sourceMappingURL=authInterceptorService.js.map
