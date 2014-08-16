'use strict';
var authModule: ng.IModule =
    angular.module('authModule', ['ui.router', 'ngStorage']);


interface IAuthSettings {
    apiServiceBaseUri: string;
    clientId: string
}

authModule.config(['$stateProvider', ($stateProvider) => {

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


