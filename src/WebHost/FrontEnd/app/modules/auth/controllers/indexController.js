'use strict';
angular.module('authModule').controller('indexController', [
    '$scope', '$location', 'authService',
    function ($scope, $location, authService) {
        $scope.logOut = function () {
            authService.logOut();
            $location.path('/');
        };

        $scope.authentication = authService.authentication;
    }]);
//# sourceMappingURL=indexController.js.map
