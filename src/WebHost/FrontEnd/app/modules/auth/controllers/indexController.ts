'use strict';
angular.module('authModule')
    .controller('indexController', ['$scope', '$location', 'authService',
        ($scope, $location : ng.ILocationService, authService : IAuthService) => {

            $scope.logOut = () => {
                authService.logOut();
                $location.path('/');
            }

            $scope.authentication = authService.authentication;

        }]);