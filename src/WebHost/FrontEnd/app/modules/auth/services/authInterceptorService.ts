'use strict';

angular.module('authModule').factory('authInterceptorService',
    ['$q', '$injector', '$location', '$localStorage',
        ($q, $injector, $location, $localStorage) => {

            var authInterceptorServiceFactory = { request: null, responseError: null };

            var request = config => {

                config.headers = config.headers || {};

                var authData: IStoredAuthData = $localStorage.authorizationData;
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.token;
                }

                return config;
            }

            var responseError = rejection => {
                if (rejection.status === 401) {
                    var authService : IAuthService = $injector.get('authService');
                    var authData: IStoredAuthData = $localStorage.authorizationData;

                    authService.logOut();
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = request;
            authInterceptorServiceFactory.responseError = responseError;

            return authInterceptorServiceFactory;
        }]);