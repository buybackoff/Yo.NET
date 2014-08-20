'use strict';
var authModule: ng.IModule =
    angular.module('authModule', ['ngStorage']);

authModule
    .config($httpProvider => {
        $httpProvider.interceptors.push('authInterceptorService');
    });

authModule.run(['$rootScope', 'authService', ($rootScope, authService: IAuthService) => {
    $rootScope.isAuth = authService.isAuth;
    authService.refillAuthNames();
}]);