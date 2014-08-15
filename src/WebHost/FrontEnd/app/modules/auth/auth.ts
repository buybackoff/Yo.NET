'use strict';
var authModule: ng.IModule =
    angular.module('authModule', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);


interface IAuthSettings
{
    apiServiceBaseUri: string;
    clientId: string
}

authModule.config($routeProvider => {

    $routeProvider.when("/", {
        controller: "homeController",
        templateUrl: "/modules/auth/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/modules/auth/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/modules/auth/views/signup.html"
    });

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/modules/auth/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/modules/auth/views/associate.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

});
authModule.constant('ngAuthSettings', <IAuthSettings>{
    apiServiceBaseUri: 'http://localhost:37654/api/',
    clientId: 'ngAuthApp'
});
authModule.config($httpProvider => {
    $httpProvider.interceptors.push('authInterceptorService');
});
authModule.run(['authService', authService => {
    authService.fillAuthData();
}]);


