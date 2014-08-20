'use strict';
var authModule = angular.module('authModule', ['ngStorage']);

authModule.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

authModule.run([
    '$rootScope', 'authService', function ($rootScope, authService) {
        $rootScope.isAuth = authService.isAuth;
        authService.refillAuthNames();
    }]);
//# sourceMappingURL=auth.js.map
