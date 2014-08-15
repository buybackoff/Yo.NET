﻿'use strict';

interface ILoginData {
    userName: string;
    password: string;
    useRefreshTokens: boolean;
}


angular.module('authModule')
    .controller('loginController', ['$scope', '$location', '$window', 'authService', 'ngAuthSettings',
        ($scope, $location, $window, authService, ngAuthSettings : IAuthSettings) => {

            $scope.loginData = <ILoginData>{
                userName: "",
                password: "",
                useRefreshTokens: false
            };

            $scope.message = "";

            $scope.login = () => {

                authService.login($scope.loginData).then(response => {
                        $location.path('/');
                    },
                    err => {
                        $scope.message = err.error_description;
                    });
            };

            $scope.authExternalProvider = provider => {

                var redirectUri = location.protocol + '//' +
                    location.host + '/authcomplete.html';

                var externalProviderUrl = ngAuthSettings.apiServiceBaseUri +
                    "auth/Account/ExternalLogin?provider=" + provider
                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                    + "&redirect_uri=" + redirectUri;
                // TODO check & fix
                $window.$windowScope = $scope;

                var oauthWindow = window.open(externalProviderUrl,
                    "Authenticate Account", "location=0,status=0,width=600,height=750");
            };

            $scope.authCompletedCB = fragment => {

                $scope.$apply(function () {

                    if (fragment.haslocalaccount == 'False') {

                        authService.logOut();

                        authService.externalAuthData = {
                            provider: fragment.provider,
                            userName: fragment.external_user_name,
                            externalAccessToken: fragment.external_access_token
                        };

                        $location.path('/associate');

                    }
                    else {
                        //Obtain access token and redirect to orders
                        var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                        authService.obtainAccessToken(externalData).then(response => {
                                $location.path('/');
                            },
                            err => {
                                $scope.message = err.error_description;
                            });
                    }

                });
            }
        }]);
