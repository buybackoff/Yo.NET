'use strict';

angular.module('authModule').factory('authInterceptorService',
    ['$q', '$injector', 
        ($q, $injector) => {
            var authInterceptorServiceFactory = { request: null, responseError: null };

            var responseError = rejection => {
                if (rejection.status === 401) {
                    var authService : IAuthService = $injector.get('authService');
                    authService.logout("/account/login");
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.responseError = responseError;

            return authInterceptorServiceFactory;
        }]);