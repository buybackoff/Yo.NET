'use strict';

angular.module('authModule').factory('authInterceptorService',
    ['$q', '$injector', '$location', 'localStorageService',
        ($q, $injector, $location, localStorageService) => {

            var authInterceptorServiceFactory = { request: null, responseError: null };

            var request = config => {

                config.headers = config.headers || {};

                var authData: IStoredAuthData = localStorageService.get('authorizationData');
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.token;
                }

                return config;
            }

            var responseError = rejection => {
                if (rejection.status === 401) {
                    var authService : IAuthService = $injector.get('authService');
                    var authData: IStoredAuthData = localStorageService.get('authorizationData');

                    // TODO should not have any explicit refresh path, it is at api level only
                    if (authData) {
                        if (authData.useRefreshTokens) {
                            $location.path('/refresh');
                            return $q.reject(rejection);
                        }
                    }
                    authService.logOut();
                    $location.path('/login');
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = request;
            authInterceptorServiceFactory.responseError = responseError;

            return authInterceptorServiceFactory;
        }]);