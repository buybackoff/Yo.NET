'use strict';
var authModule = angular.module('authModule', ['ui.router', 'ngStorage']);

authModule.config([
    '$stateProvider', function ($stateProvider) {
        $stateProvider.state("home", {
            url: "/",
            controller: "homeController",
            templateUrl: "/modules/auth/views/home.html"
        });

        $stateProvider.state("login", {
            url: "/login",
            controller: "loginController",
            templateUrl: "/modules/auth/views/login.html"
        });

        $stateProvider.state("signup", {
            url: "/signup",
            controller: "signupController",
            templateUrl: "/modules/auth/views/signup.html"
        });

        $stateProvider.state("refresh", {
            url: "/refresh",
            controller: "refreshController",
            templateUrl: "/app/views/refresh.html"
        });

        $stateProvider.state("tokens", {
            url: "/tokens",
            controller: "tokensManagerController",
            templateUrl: "/modules/auth/views/tokens.html"
        });

        $stateProvider.state("associate", {
            url: "/associate",
            controller: "associateController",
            templateUrl: "/modules/auth/views/associate.html"
        });
    }]);
authModule.constant('ngAuthSettings', {
    apiServiceBaseUri: 'http://localhost:37654/api/',
    clientId: 'ngAuthApp'
});
authModule.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});
authModule.run([
    'authService', function (authService) {
        authService.fillAuthData();
    }]);
//# sourceMappingURL=auth.js.map
