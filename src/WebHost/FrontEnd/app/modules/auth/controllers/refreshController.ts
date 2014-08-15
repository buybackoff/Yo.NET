'use strict';
angular.module('authModule')
    .controller('refreshController', ['$scope', '$location', 'authService',
        ($scope, $location, authService) => {

            $scope.authentication = authService.authentication;
            $scope.tokenRefreshed = false;
            $scope.tokenResponse = null;

            $scope.refreshToken = () => {

                authService.refreshToken().then(response => {
                        $scope.tokenRefreshed = true;
                        $scope.tokenResponse = response;
                    },
                    err => {
                        $location.path('/login');
                    });
            };
        }]);